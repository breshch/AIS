﻿using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_AV.Models;
using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Directories;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels;
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.ViewModels.Infos;
using AIS_Enterprise_Global.Views.Currents;
using AIS_Enterprise_Global.Views.Directories;
using AIS_Enterprise_Global.Views.Infos;
using Numerizr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AIS_Enterprise_AV.Views
{
    public partial class MonthTimeSheetView : Window
    {
        private BusinessContextAV _bc = new BusinessContextAV();
        private int _currentYear;
        private int _currentMonth;
        private int _prevCountLastDaysInMonth;
        private List<MonthTimeSheetWorker> _monthTimeSheetWorkers = new List<MonthTimeSheetWorker>();
        private List<DateTime> _listDatesOfOverTime = new List<DateTime>();
        private List<DateTime> _weekends;

        private const int COLUMN_FULL_NAME = 1;
        private const int COLUMN_POST_NAME = 4;
        private const int COUNT_COLUMNS_BEFORE_DAYS = 5;
        private const int COUNT_COLUMNS_AFTER_DAYS = 4;
        private const string WEEKEND_DEFINITION = "В";
        private const int COLUMN_PANALTIES_AFTER_DAYS = 11;
        private const int COLUMN_FINAL_SALARY_AFTER_DAYS = 15;

        private System.Windows.Media.Brush _brushNoOdd;
        private System.Windows.Media.Brush _brushOdd;
        private System.Windows.Media.Brush _brushWeekend;
        private System.Windows.Media.Brush _brushLessThanEight;
        private System.Windows.Media.Brush _brushVocation;
        private System.Windows.Media.Brush _brushMissDay;
        private System.Windows.Media.Brush _brushSickDay;


        public MonthTimeSheetView()
        {
            InitializeComponent();

            if (System.Windows.Forms.Screen.AllScreens.Count() > 1)
            {
                WindowMonthTimeSheet.SizeToContent = System.Windows.SizeToContent.Height;
                WindowMonthTimeSheet.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            }

            _bc.InitializeAbsentDates();

            InitializeBrushes();

            ComboboxYears.ItemsSource = _bc.GetYears().ToList();
            if (ComboboxYears.Items.Count != 0)
            {
                ComboboxYears.SelectedIndex = ComboboxYears.Items.Count - 1;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (_listDatesOfOverTime.Any())
            {
                ShowOverTimeView();
            }

            _bc.EditParameter("LastDate", DateTime.Now.ToString());

            _bc.Dispose();
        }

        private void ComboboxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentYear = int.Parse(ComboboxYears.SelectedItem.ToString());
            ComboboxMonthes.ItemsSource = _bc.GetMonthes(_currentYear).ToList();

            if (ComboboxMonthes.Items.Count != 0)
            {
                ComboboxMonthes.SelectedIndex = ComboboxMonthes.Items.Count - 2;
            }
        }

        private void FillDataGrid()
        {
            _bc.RefreshContext();

            DataGridMonthTimeSheet.ItemsSource = null;
            if (_prevCountLastDaysInMonth != 0)
            {
                for (int i = 0; i < _prevCountLastDaysInMonth; i++)
                {
                    DataGridMonthTimeSheet.Columns.RemoveAt(6);
                }
            }

            _monthTimeSheetWorkers.Clear();

            int countWorkDaysInMonth = _bc.GetCountWorkDaysInMonth(_currentYear, _currentMonth);

            TextBlockCountWorkDays.Text = countWorkDaysInMonth + " " + 
                NumerizrFactory.Numerize("ru", countWorkDaysInMonth, "рабочий", "рабочих", "рабочих") + " " + 
                NumerizrFactory.Numerize("ru", countWorkDaysInMonth, "день", "дня", "дней");

            var firstDateInMonth = new DateTime(_currentYear, _currentMonth, 1);
            var lastDateInMonth = HelperMethods.GetLastDateInMonth(_currentYear, _currentMonth);

            _prevCountLastDaysInMonth = lastDateInMonth.Day;

            for (int i = 0; i < lastDateInMonth.Day; i++)
            {
                var column = new DataGridTextColumn
                {
                    Header = (i + 1).ToString() + Environment.NewLine + new DateTime(_currentYear, _currentMonth, i + 1).ToString("ddd"),
                    Binding = new Binding
                    {
                        Path = new PropertyPath("Hours[" + i + "]")
                    },
                    MinWidth = 30,
                    CellStyle = Resources["CenterTextAlignmentCellWithRightClick"] as Style
                };
                
                DataGridMonthTimeSheet.Columns.Insert(6 + i, column);
            }

            _weekends = _bc.GetHolidays(_currentYear, _currentMonth).ToList();

            var workers = _bc.GetDirectoryWorkers(_currentYear, _currentMonth).ToList();

            int indexWorker = 0;
            foreach (var worker in workers)
            {
                indexWorker++;

                var currentPosts = _bc.GetCurrentPosts(worker.Id, _currentYear, _currentMonth, _prevCountLastDaysInMonth).ToList();

                bool isFirst = false;

                double salary = 0;
                foreach (var currentPost in currentPosts)
                {
                    var monthTimeSheetWorker = _monthTimeSheetWorkers.FirstOrDefault(m => m.WorkerId == worker.Id && m.DirectoryPostId == currentPost.DirectoryPostId);

                    if (monthTimeSheetWorker == null)
                    {
                        monthTimeSheetWorker = new MonthTimeSheetWorker();

                        _monthTimeSheetWorkers.Add(monthTimeSheetWorker);

                        monthTimeSheetWorker.WorkerId = worker.Id;

                        if (!isFirst)
                        {
                            monthTimeSheetWorker.FullName = worker.FullName;
                            isFirst = true;
                        }

                        monthTimeSheetWorker.IsOdd = indexWorker % 2 == 0;

                        monthTimeSheetWorker.DirectoryPostId = currentPost.DirectoryPostId;
                        monthTimeSheetWorker.PostChangeDate = currentPost.ChangeDate;
                        monthTimeSheetWorker.PostName = currentPost.DirectoryPost.Name;
                        monthTimeSheetWorker.SalaryInHour = Math.Round(currentPost.DirectoryPost.AdminWorkerSalary.Value / countWorkDaysInMonth / 8, 2);

                        monthTimeSheetWorker.Hours = new string[lastDateInMonth.Day];
                    }

                    int indexHour = 0;
                    for (DateTime date = firstDateInMonth; worker.FireDate == null && date.Date <= lastDateInMonth.Date || date.Date <= lastDateInMonth.Date && worker.FireDate != null && date.Date <= worker.FireDate.Value.Date; date = date.AddDays(1))
                    {
                        if (date.Date >= currentPost.ChangeDate.Date && (currentPost.FireDate == null || currentPost.FireDate != null && currentPost.FireDate.Value.Date >= date.Date))
                        {
                            var day = worker.InfoDates.FirstOrDefault(d => d.Date.Date == date.Date);

                            if (day != null)
                            {
                                string hour = null;

                                switch (day.DescriptionDay)
                                {
                                    case DescriptionDay.Был:
                                        if (day.CountHours != null)
                                        {
                                            hour = day.CountHours.ToString();
                                            if (!_weekends.Any(h => h.Date.Date == date.Date))
                                            {
                                                monthTimeSheetWorker.OverTime += day.CountHours.Value > 8 ? day.CountHours.Value - 8 : 0;
                                                salary += day.CountHours.Value <= 8 ? day.CountHours.Value * monthTimeSheetWorker.SalaryInHour : (8 + ((day.CountHours.Value - 8) * 2)) * monthTimeSheetWorker.SalaryInHour;
                                            }
                                            else
                                            {
                                                monthTimeSheetWorker.OverTime += day.CountHours.Value;
                                                salary += day.CountHours.Value * monthTimeSheetWorker.SalaryInHour * 2;
                                            }
                                        }

                                        break;
                                    case DescriptionDay.Б:
                                        if (monthTimeSheetWorker.SickDays < 5)
                                        {
                                            monthTimeSheetWorker.SickDays++;
                                            salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        }
                                        else
                                        {
                                            monthTimeSheetWorker.MissDays++;
                                        }
                                        break;
                                    case DescriptionDay.О:
                                        monthTimeSheetWorker.VocationDays++;
                                        salary += 8 * monthTimeSheetWorker.SalaryInHour;
                                        break;
                                    case DescriptionDay.ДО:
                                        break;
                                    case DescriptionDay.П:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    case DescriptionDay.С:
                                        monthTimeSheetWorker.MissDays++;
                                        break;
                                    default:
                                        break;
                                }

                                if (day.DescriptionDay != DescriptionDay.Был)
                                {
                                    hour = day.DescriptionDay.ToString();
                                }
                                else
                                {
                                    if (hour == null)
                                    {
                                        hour = WEEKEND_DEFINITION;
                                    }
                                }

                                monthTimeSheetWorker.Hours[indexHour] = hour;
                            }
                        }

                        indexHour++;
                    }
                }

                var monthTimeSheetWorkerFinalSalary = _monthTimeSheetWorkers.First(m => m.WorkerId == worker.Id && m.FullName != null);

                var infoMonth = worker.InfoMonthes.FirstOrDefault(m => m.Date.Year == _currentYear && m.Date.Month == _currentMonth);
                if (infoMonth != null)
                {
                    monthTimeSheetWorkerFinalSalary.PrepaymentCash = infoMonth.PrepaymentCash.ToString();
                    monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction = infoMonth.PrepaymentBankTransaction.ToString();
                    monthTimeSheetWorkerFinalSalary.Compensation = infoMonth.Compensation.ToString();
                    monthTimeSheetWorkerFinalSalary.VocationPayment = infoMonth.VocationPayment.ToString();
                    monthTimeSheetWorkerFinalSalary.CardAV = infoMonth.CardAV.ToString();
                    monthTimeSheetWorkerFinalSalary.CardFenox = infoMonth.CardFenox.ToString();
                    monthTimeSheetWorkerFinalSalary.Panalty = _bc.GetInfoDatePanalties(worker.Id, _currentYear, _currentMonth).Sum(d => d.InfoPanalty.Summ).ToString();
                    monthTimeSheetWorkerFinalSalary.Inventory = infoMonth.Inventory.ToString();
                    monthTimeSheetWorkerFinalSalary.BirthDays = infoMonth.BirthDays;
                    monthTimeSheetWorkerFinalSalary.Bonus = infoMonth.Bonus.ToString();
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - 
                        double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
                }
            }

            DataGridMonthTimeSheet.ItemsSource = _monthTimeSheetWorkers;
            DataGridMonthTimeSheet.Items.Refresh();
        }

        private void ComboboxMonthes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentMonth = int.Parse(ComboboxMonthes.SelectedItem.ToString());

            ButtonOverTimes.IsEnabled = _bc.GetInfoOverTimeDates(_currentYear, _currentMonth).Any();

            FillDataGrid();
        }

        private void DataGridMonthTimeSheet_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = (TextBox)e.EditingElement;

            int indexRow = e.Row.GetIndex();
            int indexColumn = e.Column.DisplayIndex;
            textBox.Text = textBox.Text.ToUpper().Replace(".", ",");
            string value = textBox.Text;


            var monthTimeSheetWorker = _monthTimeSheetWorkers[indexRow];
            int workerId = monthTimeSheetWorker.WorkerId;

            int rowIndexOfFullRow = _monthTimeSheetWorkers.IndexOf(_monthTimeSheetWorkers.First(w => w.WorkerId == workerId && w.FullName != null));
            int columnIndexFinalSalary = DataGridMonthTimeSheet.Columns.Count - 1;

            if (indexColumn > COUNT_COLUMNS_BEFORE_DAYS && indexColumn <= _prevCountLastDaysInMonth + COUNT_COLUMNS_BEFORE_DAYS)
            {
                int day = indexColumn - COUNT_COLUMNS_BEFORE_DAYS;
                DateTime date = new DateTime(_currentYear, _currentMonth, day);
                string prevValue = monthTimeSheetWorker.Hours[day - 1];

                if (!_weekends.Any(w => w.Date == date.Date))
                {
                    if (value == WEEKEND_DEFINITION)
                    {
                        MessageBox.Show("Это не выходной день.");
                        textBox.Text = prevValue;
                        return;
                    }

                    if (!Enum.IsDefined(typeof(DescriptionDay), value))
                    {
                        double result;
                        if (!double.TryParse(value, out result))
                        {
                            MessageBox.Show("Введите общепринятые сокращения.");
                            textBox.Text = prevValue;
                            return;
                        }

                        if (result <= 0 || result > 16)
                        {
                            MessageBox.Show("Введите только число, большее 0 и меньшее, либо равное 16.");
                            textBox.Text = prevValue;
                            return;
                        }
                    }
                }
                else
                {
                    if (value != WEEKEND_DEFINITION && value != DescriptionDay.О.ToString())
                    {
                        double result;
                        if (!double.TryParse(value, out result))
                        {
                            MessageBox.Show("Введите выходной или отпуск.");
                            textBox.Text = prevValue;
                            return;
                        }

                        if (result <= 0 || result > 16)
                        {
                            MessageBox.Show("Введите только число, большее 0 и меньшее, либо равное 16.");
                            textBox.Text = prevValue;
                            return;
                        }
                    }
                }

                _bc.EditInfoDateHour(workerId, date, value);

                monthTimeSheetWorker.Hours[day - 1] = value;

                double resultValue;
                if (double.TryParse(value, out resultValue))
                {
                    if ((resultValue > 8 || _bc.IsWeekend(date)) && !_listDatesOfOverTime.Contains(date))
                    {
                        _listDatesOfOverTime.Add(date);

                        if (!ButtonOverTimes.IsEnabled)
                        {
                            ButtonOverTimes.IsEnabled = true;
                        }
                    }
                }

                double prevResultValue = 0;
                if (double.TryParse(prevValue, out prevResultValue))
                {
                    if (!_bc.IsWeekend(date))
                    {
                        if (prevResultValue > 8 && (Enum.IsDefined(typeof(DescriptionDay), value) || resultValue <= 8))
                        {
                            bool isOverTime = false;
                            foreach (var monthTimeSheetWorkerTemp in _monthTimeSheetWorkers)
                            {
                                string hour = monthTimeSheetWorkerTemp.Hours[day - 1];
                                if (hour != null)
                                {
                                    if (!Enum.IsDefined(typeof(DescriptionDay), hour))
                                    {
                                        double hourValue = double.Parse(hour);
                                        if (hourValue > 8)
                                        {
                                            isOverTime = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!isOverTime)
                            {
                                _listDatesOfOverTime.Remove(date);

                                ButtonOverTimes.IsEnabled = _bc.GetInfoOverTimeDates(_currentYear, _currentMonth).Concat(_listDatesOfOverTime).Any();
                            }
                        }
                    }
                    else if (value == WEEKEND_DEFINITION || Enum.IsDefined(typeof(DescriptionDay), value))
                    {
                        bool isOverTime = false;
                        foreach (var monthTimeSheetWorkerTemp in _monthTimeSheetWorkers)
                        {
                            string hour = monthTimeSheetWorkerTemp.Hours[day - 1];
                            if (hour != null)
                            {
                                double hourValue;
                                if (double.TryParse(hour, out hourValue))
                                {
                                    isOverTime = true;
                                    break;
                                }
                            }
                        }

                        if (!isOverTime)
                        {
                            _listDatesOfOverTime.Remove(date);

                            ButtonOverTimes.IsEnabled = _bc.GetInfoOverTimeDates(_currentYear, _currentMonth).Concat(_listDatesOfOverTime).Any();
                        }
                    }
                }

                ColorizeCell(value, indexRow, indexColumn, monthTimeSheetWorker.IsOdd);

                double salary = 0;
                foreach (var monthTimeSheetWorkerTemp in _monthTimeSheetWorkers.Where(w => w.WorkerId == workerId))
                {
                    monthTimeSheetWorkerTemp.OverTime = 0;
                    monthTimeSheetWorkerTemp.VocationDays = 0;
                    monthTimeSheetWorkerTemp.SickDays = 0;
                    monthTimeSheetWorkerTemp.MissDays = 0;

                    for (int currentDay = 0; currentDay < monthTimeSheetWorkerTemp.Hours.Count(); currentDay++)
                    {
                        string hour = monthTimeSheetWorkerTemp.Hours[currentDay];

                        if (hour != null)
                        {
                            if (Enum.IsDefined(typeof(DescriptionDay), hour))
                            {
                                var descriptionDay = (DescriptionDay)Enum.Parse(typeof(DescriptionDay), hour);

                                switch (descriptionDay)
                                {
                                    case DescriptionDay.Б:
                                        if (monthTimeSheetWorkerTemp.SickDays < 5)
                                        {
                                            monthTimeSheetWorkerTemp.SickDays++;
                                            salary += 8 * monthTimeSheetWorkerTemp.SalaryInHour;
                                        }
                                        else
                                        {
                                            monthTimeSheetWorkerTemp.MissDays++;
                                        }
                                        break;
                                    case DescriptionDay.О:
                                        monthTimeSheetWorkerTemp.VocationDays++;
                                        salary += 8 * monthTimeSheetWorkerTemp.SalaryInHour;
                                        break;
                                    case DescriptionDay.ДО:
                                        break;
                                    case DescriptionDay.П:
                                        monthTimeSheetWorkerTemp.MissDays++;
                                        break;
                                    case DescriptionDay.С:
                                        monthTimeSheetWorkerTemp.MissDays++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                double countHours;
                                if (double.TryParse(hour, out countHours))
                                {
                                    if (!_weekends.Any(h => h.Date.Date == new DateTime(_currentYear, _currentMonth, currentDay + 1)))
                                    {
                                        monthTimeSheetWorkerTemp.OverTime += countHours > 8 ? countHours - 8 : 0;
                                        salary += countHours <= 8 ? countHours * monthTimeSheetWorkerTemp.SalaryInHour : (8 + ((countHours - 8) * 2)) * monthTimeSheetWorkerTemp.SalaryInHour;
                                    }
                                    else
                                    {
                                        monthTimeSheetWorkerTemp.OverTime += countHours;
                                        salary += countHours * monthTimeSheetWorkerTemp.SalaryInHour * 2;
                                    }
                                }
                            }
                        }
                    }

                    ChangeCellValue(monthTimeSheetWorkerTemp.OverTime, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 1);
                    ChangeCellValue(monthTimeSheetWorkerTemp.VocationDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 2);
                    ChangeCellValue(monthTimeSheetWorkerTemp.SickDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 3);
                    ChangeCellValue(monthTimeSheetWorkerTemp.MissDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 4);
                }

                var monthTimeSheetWorkerFinalSalary = _monthTimeSheetWorkers.First(m => m.WorkerId == workerId && m.FullName != null);

                salary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - 
                    double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays.Value + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);

                monthTimeSheetWorkerFinalSalary.FinalSalary = salary;

                ChangeCellValue(monthTimeSheetWorkerFinalSalary.FinalSalary.Value, rowIndexOfFullRow, columnIndexFinalSalary);
            }
            else
            {
                var date = new DateTime(_currentYear, _currentMonth, 1);
                switch (indexColumn - (COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + COUNT_COLUMNS_AFTER_DAYS))
                {
                    case 1:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.PrepaymentCash))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "PrepaymentCash", double.Parse(value));
                            ChangeFinalSalary(double.Parse(monthTimeSheetWorker.PrepaymentCash) - double.Parse(value), rowIndexOfFullRow);
                            monthTimeSheetWorker.PrepaymentCash = value;
                        }
                        break;
                    case 2:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.PrepaymentBankTransaction))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "PrepaymentBankTransaction", double.Parse(value));
                            monthTimeSheetWorker.PrepaymentBankTransaction = value;
                        }
                        break;
                    case 3:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.Compensation))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "Compensation", double.Parse(value));
                            monthTimeSheetWorker.Compensation = value;
                        }
                        break;
                    case 4:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.VocationPayment))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "VocationPayment", double.Parse(value));
                            monthTimeSheetWorker.VocationPayment = value;
                        }
                        break;
                    case 5:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.CardAV))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "CardAV", double.Parse(value));
                            monthTimeSheetWorker.CardAV = value;
                        }
                        break;
                    case 6:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.CardFenox))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "CardFenox", double.Parse(value));
                            monthTimeSheetWorker.CardFenox = value;
                        }
                        break;
                    case 8:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.Inventory))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "Inventory", double.Parse(value));
                            ChangeFinalSalary(double.Parse(monthTimeSheetWorker.Inventory) - double.Parse(value), rowIndexOfFullRow);
                            monthTimeSheetWorker.Inventory = value;
                        }
                        break;
                    case 10:
                        if (IsValidateDoubleValue(e, monthTimeSheetWorker.Bonus))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "Bonus", double.Parse(value));
                            ChangeFinalSalary(double.Parse(value) - double.Parse(monthTimeSheetWorker.Bonus), rowIndexOfFullRow);
                            monthTimeSheetWorker.Bonus = value;
                        }
                        break;
                }
            }
        }

        private bool IsValidateDoubleValue(DataGridCellEditEndingEventArgs e, string prevValue)
        {
            var textBox = (TextBox)e.EditingElement;

            string value = textBox.Text.Replace(".", ",");

            double result;
            if (!double.TryParse(value, out result))
            {
                MessageBox.Show("Введите только число.");
                textBox.Text = prevValue;
                return false;
            }

            return true;
        }

        private void ChangeFinalSalary(double change, int rowIndexOfFullRow)
        {
            var cell = DataGridMonthTimeSheet.GetCell(rowIndexOfFullRow, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + COLUMN_FINAL_SALARY_AFTER_DAYS);
            var t = cell.Content as TextBlock;
            t.Text = (double.Parse(t.Text.Replace(".", ",")) + change).ToString().Replace(",", ".");
            _monthTimeSheetWorkers[rowIndexOfFullRow].FinalSalary = double.Parse(t.Text.Replace(".", ","));
        }

        private void ChangeCellValue(double value, int rowIndex, int columnIndex)
        {
            var cell = DataGridMonthTimeSheet.GetCell(rowIndex, columnIndex);
            var t = cell.Content as TextBlock;
            t.Text = value.ToString();
        }

        private void DataGridMonthTimeSheet_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;
            var cell = DataGridMonthTimeSheet.GetCell(rowIndex, columnIndex);
            var textBox = (TextBlock)cell.Content;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
            }
        }

        private void ButtonOverTimes_Click(object sender, RoutedEventArgs e)
        {
            ShowOverTimeView();

            ButtonOverTimes.IsEnabled = _bc.GetInfoOverTimeDates(_currentYear, _currentMonth).Concat(_listDatesOfOverTime).Any();
        }

        private void ShowOverTimeView()
        {
            var infoOverTimeView = new InfoOverTimeView();
            var infoOverTimeViewModel = new InfoOverTimeViewModel(_listDatesOfOverTime, new DateTime(_currentYear, _currentMonth, 1), new DateTime(_currentYear, _currentMonth, _prevCountLastDaysInMonth));

            infoOverTimeView.DataContext = infoOverTimeViewModel;
            infoOverTimeView.ShowDialog();

            for (int i = 0; i < _listDatesOfOverTime.Count; )
            {
                if (_bc.IsInfoOverTimeDate(_listDatesOfOverTime[i]))
                {
                    _listDatesOfOverTime.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private void DataGridMonthTimeSheet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridMonthTimeSheet.SelectedCells.Any())
            {
                var cell = DataGridMonthTimeSheet.SelectedCells[0];
                var monthTimeSheetWorker = (MonthTimeSheetWorker)cell.Item;
                int columnIndex = cell.Column.DisplayIndex;

                switch (columnIndex)
                {
                    case COLUMN_FULL_NAME:
                        if (!string.IsNullOrWhiteSpace(monthTimeSheetWorker.FullName))
                        {
                            var directoryEditWorkerView = new DirectoryEditWorkerView();
                            var directoryEditWorkerViewModel = new DirectoryEditWorkerViewModel(monthTimeSheetWorker.WorkerId);
                            directoryEditWorkerView.DataContext = directoryEditWorkerViewModel;

                            directoryEditWorkerView.ShowDialog();

                            FillDataGrid();
                        }
                        break;
                    case COLUMN_POST_NAME:
                        var currentCompanyAndPostView = new CurrentCompanyAndPostView();
                        var currentCompanyAndPostViewModel = new CurrentCompanyAndPostViewModel(new DateTime(_currentYear, _currentMonth, 1),
                            new DateTime(_currentYear, _currentMonth, _prevCountLastDaysInMonth));
                        currentCompanyAndPostView.DataContext = currentCompanyAndPostViewModel;

                        currentCompanyAndPostView.ShowDialog();

                        var currentCompanyAndPost = currentCompanyAndPostViewModel.CurrentCompanyAndPost;

                        if (currentCompanyAndPost != null)
                        {
                            _bc.AddCurrentPost(monthTimeSheetWorker.WorkerId, currentCompanyAndPost);
                            FillDataGrid();
                        }
                        break;
                }

                if (columnIndex == COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + COLUMN_PANALTIES_AFTER_DAYS)
                {

                    var infoPanaltiesViewModel = new InfoPanaltiesViewModel(monthTimeSheetWorker.WorkerId, _currentYear, _currentMonth);
                    var infoPanaltiesView = new InfoPanaltiesView();
                    infoPanaltiesView.DataContext = infoPanaltiesViewModel;
                    infoPanaltiesView.ShowDialog();
                }
            }
        }

        private bool _isRightButtonDown = false;

        private void DataGridCell_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isRightButtonDown = true;
        }

        private void DataGridMonthTimeSheet_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (_isRightButtonDown)
            {
                _isRightButtonDown = false;

                if (DataGridMonthTimeSheet.SelectedCells.Any())
                {
                    var cell = DataGridMonthTimeSheet.SelectedCells[0];
                    int indexColumn = cell.Column.DisplayIndex;
                    var monthTimeSheetWorker = (MonthTimeSheetWorker)cell.Item;

                    int day = indexColumn - COUNT_COLUMNS_BEFORE_DAYS;
                    Debug.WriteLine(day);

                    if (indexColumn > COUNT_COLUMNS_BEFORE_DAYS && indexColumn <= COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth && monthTimeSheetWorker.Hours[day - 1] != null)
                    {
                        var contextMenu = new ContextMenu();
                        var itemContextMenu = new MenuItem
                        {
                            Header = "Штраф"
                        };

                        itemContextMenu.Click += itemContextMenu_Click;

                        contextMenu.Items.Add(itemContextMenu);
                        DataGridMonthTimeSheet.ContextMenu = contextMenu;
                    }
                    else
                    {
                        DataGridMonthTimeSheet.ContextMenu = null;
                    }
                }
            }
        }

        private void itemContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var cell = DataGridMonthTimeSheet.SelectedCells[0];
            var monthTimeSheetWorker = (MonthTimeSheetWorker)cell.Item;
            int workerId = monthTimeSheetWorker.WorkerId;

            int indexDisplay = cell.Column.DisplayIndex;
            int rowIndexOfFullRow = _monthTimeSheetWorkers.IndexOf(_monthTimeSheetWorkers.First(w => w.WorkerId == workerId && w.FullName != null));

            int day = cell.Column.DisplayIndex - COUNT_COLUMNS_BEFORE_DAYS;
            var date = new DateTime(_currentYear, _currentMonth, day);


            var infoPanaltyView = new InfoPanaltyView();
            var infoPanaltyViewModel = new InfoPanaltyViewModel(workerId, date);

            infoPanaltyView.DataContext = infoPanaltyViewModel;
            infoPanaltyView.ShowDialog();

            int indexColumnPanalty = COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + COLUMN_PANALTIES_AFTER_DAYS;

            var cellPanalty = DataGridMonthTimeSheet.GetCell(rowIndexOfFullRow, indexColumnPanalty);

            double totalPanalty = _bc.GetInfoDatePanaltiesWithoutCash(workerId, _currentYear, _currentMonth).Sum(d => d.InfoPanalty.Summ);

            var t = cellPanalty.Content as TextBlock;
            t.Text = totalPanalty.ToString();

            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndexOfFullRow].Panalty) - totalPanalty, rowIndexOfFullRow);
            _monthTimeSheetWorkers[rowIndexOfFullRow].Panalty = t.Text;
        }

        private void DataGridMonthTimeSheet_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => AlterRow(e)));
        }

        private void AlterRow(DataGridRowEventArgs e)
        {
            int indexRow = e.Row.GetIndex();
            var monthTimeSheetWorker = _monthTimeSheetWorkers[indexRow];

            e.Row.Background = monthTimeSheetWorker.IsOdd ? _brushOdd : _brushNoOdd;

            for (int indexHour = 0; indexHour < _prevCountLastDaysInMonth; indexHour++)
            {
                var value = monthTimeSheetWorker.Hours[indexHour];
                ColorizeCell(value, indexRow, COUNT_COLUMNS_BEFORE_DAYS + indexHour + 1, monthTimeSheetWorker.IsOdd);
            }
        }

        private void ColorizeCell(string value, int indexRow, int indexColumn, bool isOdd)
        {
            if (value != null)
            {
                var cell = DataGridMonthTimeSheet.GetCell(indexRow, indexColumn);

                double result = 0;
                if (double.TryParse(value, out result))
                {
                    if (result < 8)
                    {
                        cell.Background = _brushLessThanEight;
                    }
                    else
                    {
                        cell.Background = isOdd ? _brushOdd : _brushNoOdd;
                    }
                }
                else
                {
                    if (value == WEEKEND_DEFINITION)
                    {
                        cell.Background = _brushWeekend;
                    }
                    else
                    {
                        if (Enum.IsDefined(typeof(DescriptionDay), value))
                        {
                            var descriptionDay = (DescriptionDay)Enum.Parse(typeof(DescriptionDay), value);

                            switch (descriptionDay)
                            {
                                case DescriptionDay.Б:
                                    cell.Background = _brushSickDay;
                                    break;
                                case DescriptionDay.П:
                                    cell.Background = _brushMissDay;
                                    break;
                                case DescriptionDay.ДО:
                                case DescriptionDay.О:
                                case DescriptionDay.С:
                                    cell.Background = _brushVocation;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void InitializeBrushes()
        {
            var converter = new System.Windows.Media.BrushConverter();

            _brushNoOdd = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFFFFFF");
            _brushOdd = (System.Windows.Media.Brush)converter.ConvertFromString("#FFe8f4fa");
            _brushWeekend = (System.Windows.Media.Brush)converter.ConvertFromString("#FF9bbf9b");
            _brushLessThanEight = (System.Windows.Media.Brush)converter.ConvertFromString("#FFeebebe");
            _brushVocation = (System.Windows.Media.Brush)converter.ConvertFromString("#FFc4e5c1");
            _brushMissDay = (System.Windows.Media.Brush)converter.ConvertFromString("#FFff8282");
            _brushSickDay = (System.Windows.Media.Brush)converter.ConvertFromString("#FFffcc33");
        }

        private void Companies_Click(object sender, RoutedEventArgs e)
        {
            var directoryCompanyViewModel = new DirectoryCompanyViewModel();
            var directoryCompanyView = new DirectoryCompanyView();

            directoryCompanyView.DataContext = directoryCompanyViewModel;
            directoryCompanyView.ShowDialog();
        }

        private void RCs_Click(object sender, RoutedEventArgs e)
        {
            var directoryRCViewModel = new DirectoryRCViewModel();
            var directoryRCView = new DirectoryRCView();

            directoryRCView.DataContext = directoryRCViewModel;
            directoryRCView.ShowDialog();
        }

        private void TypeOfPosts_Click(object sender, RoutedEventArgs e)
        {
            var directoryTypeOfPostViewModel = new DirectoryTypeOfPostViewModel();
            var directoryTypeOfPostView = new DirectoryTypeOfPostView();

            directoryTypeOfPostView.DataContext = directoryTypeOfPostViewModel;
            directoryTypeOfPostView.ShowDialog();
        }

        private void ListOfPosts_Click(object sender, RoutedEventArgs e)
        {
            var directoryPostViewModel = new DirectoryPostViewModel();
            var directoryPostView = new DirectoryPostView();

            directoryPostView.DataContext = directoryPostViewModel;
            directoryPostView.ShowDialog();
        }

        private void ListOfWorkers_Click(object sender, RoutedEventArgs e)
        {
            var directoryWorkerListViewModel = new DirectoryWorkerListViewModel();
            var directoryWorkerListView = new DirectoryWorkerListView();

            directoryWorkerListView.DataContext = directoryWorkerListViewModel;
            directoryWorkerListView.ShowDialog();
        }

        private void AddingWorker_Click(object sender, RoutedEventArgs e)
        {
            var directoryWorkerViewModel = new DirectoryAddWorkerViewModel();
            var directoryWorkerView = new DirectoryAddWorkerView();

            directoryWorkerView.DataContext = directoryWorkerViewModel;
            directoryWorkerView.ShowDialog();
        }

        private void Salary_Click(object sender, RoutedEventArgs e)
        {
            var salaryViewModel = new SalaryViewModel();
            var salaryView = new SalaryView();

            salaryView.DataContext = salaryViewModel;
            salaryView.ShowDialog();
        }
    }
}
