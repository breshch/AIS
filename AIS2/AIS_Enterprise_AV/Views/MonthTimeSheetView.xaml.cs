﻿using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AIS_Enterprise_AV.Views
{
    /// <summary>
    /// Логика взаимодействия для MonthTimeSheetView.xaml
    /// </summary>
    public partial class MonthTimeSheetView : Window
    {
        private BusinessContext _bc = new BusinessContext();
        private int _currentYear;
        private int _currentMonth;
        private int _prevCountLastDaysInMonth;
        private List<MonthTimeSheetWorker> _monthTimeSheetWorkers = new List<MonthTimeSheetWorker>();
        private List<DateTime> _listDatesOfOverTime = new List<DateTime>();

        private const int COUNT_COLUMNS_BEFORE_DAYS = 5;
        private const int COUNT_COLUMNS_AFTER_DAYS = 4;
        public MonthTimeSheetView()
        {
            InitializeComponent();


            ComboboxYears.ItemsSource = _bc.GetYears().ToList();
            if (ComboboxYears.Items.Count != 0)
            {
                ComboboxYears.SelectedIndex = ComboboxYears.Items.Count - 1;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
            DataGridMonthTimeSheet.ItemsSource = null;
            if (_prevCountLastDaysInMonth != 0)
            {
                for (int i = 0; i < _prevCountLastDaysInMonth; i++)
                {
                    DataGridMonthTimeSheet.Columns.RemoveAt(6);
                }
            }

            _monthTimeSheetWorkers.Clear();


            int countWorkDaysInMonth = HelperCalendar.GetCountWorkDaysInMonth(_currentYear, _currentMonth);
            var firstDateInMonth = new DateTime(_currentYear, _currentMonth, 1);
            DateTime lastDateInMonth = DateTime.Now.Year == _currentYear && DateTime.Now.Month == _currentMonth ? DateTime.Now :
                new DateTime(_currentYear, _currentMonth, DateTime.DaysInMonth(_currentYear, _currentMonth));

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
                    Width = 30
                };

                DataGridMonthTimeSheet.Columns.Insert(6 + i, column);
            }

            var workers = _bc.GetDirectoryWorkers(_currentYear, _currentMonth).ToList();
            
            foreach (var worker in workers)
            {
                var currentPosts = _bc.GetCurrentPosts(worker.Id, _currentYear, _currentMonth);

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

                        monthTimeSheetWorker.DirectoryPostId = currentPost.DirectoryPostId;
                        monthTimeSheetWorker.PostChangeDate = currentPost.ChangeDate;
                        monthTimeSheetWorker.PostName = currentPost.DirectoryPost.Name;
                        monthTimeSheetWorker.SalaryInHour = currentPost.DirectoryPost.UserWorkerSalary / countWorkDaysInMonth / 8;

                        monthTimeSheetWorker.Hours = new string[lastDateInMonth.Day];
                    }

                    int indexHour = 0;
                    for (DateTime date = firstDateInMonth; date.Date <= lastDateInMonth.Date; date = date.AddDays(1))
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
                                        hour = day.CountHours.ToString();
                                        monthTimeSheetWorker.OverTime += day.CountHours.Value > 8 ? day.CountHours.Value - 8 : 0;

                                        salary += day.CountHours.Value <= 8 ? day.CountHours.Value * monthTimeSheetWorker.SalaryInHour : (8 + ((day.CountHours.Value - 8) * 2)) * monthTimeSheetWorker.SalaryInHour;
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
                    monthTimeSheetWorkerFinalSalary.VocationPayment = infoMonth.VocationPayment.ToString();
                    monthTimeSheetWorkerFinalSalary.SalaryAV = infoMonth.SalaryAV.ToString();
                    monthTimeSheetWorkerFinalSalary.SalaryFenox = infoMonth.SalaryFenox.ToString();
                    monthTimeSheetWorkerFinalSalary.Panalty = infoMonth.Panalty.ToString();
                    monthTimeSheetWorkerFinalSalary.Inventory = infoMonth.Inventory.ToString();
                    monthTimeSheetWorkerFinalSalary.BirthDays = infoMonth.BirthDays;
                    monthTimeSheetWorkerFinalSalary.Bonus = infoMonth.Bonus.ToString();
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.VocationPayment) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryAV) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryFenox) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
                }
            }

            //Debug.WriteLine("start");

            //Thread.Sleep(1000);

            DataGridMonthTimeSheet.ItemsSource = _monthTimeSheetWorkers;
            DataGridMonthTimeSheet.Items.Refresh();

            //Thread.Sleep(1000);

            //Debug.WriteLine(DataGridMonthTimeSheet.Items.Count);


        }

        private void ComboboxMonthes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentMonth = int.Parse(ComboboxMonthes.SelectedItem.ToString());

            FillDataGrid();
        }

        private void DataGridMonthTimeSheet_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = (TextBox)e.EditingElement;

            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;
            textBox.Text = textBox.Text.ToUpper().Replace(".", ",");
            string value = textBox.Text;
            
            int workerId = _monthTimeSheetWorkers[rowIndex].WorkerId;

            int rowIndexOfFullRow = _monthTimeSheetWorkers.IndexOf(_monthTimeSheetWorkers.First(w => w.WorkerId == workerId && w.FullName != null));
            int columnIndexFinalSalary = DataGridMonthTimeSheet.Columns.Count - 1;

            if (columnIndex > COUNT_COLUMNS_BEFORE_DAYS && columnIndex <= _prevCountLastDaysInMonth + COUNT_COLUMNS_BEFORE_DAYS)
            {
                int day = columnIndex - COUNT_COLUMNS_BEFORE_DAYS;
                string prevValue = _monthTimeSheetWorkers[rowIndex].Hours[day - 1];

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

                var date = new DateTime(_currentYear, _currentMonth, day);
                _bc.EditInfoDateHour(workerId, date, value);

                _monthTimeSheetWorkers[rowIndex].Hours[day - 1] = value;

                double resultValue;
                if (double.TryParse(value, out resultValue))
                {
                    if (resultValue > 8 && !_listDatesOfOverTime.Contains(date))
                    {
                        _listDatesOfOverTime.Add(date);
                    }
                }

                double prevResultValue = 0;
                if (double.TryParse(prevValue, out prevResultValue))
                {
                    if (prevResultValue > 8)
                    {
                        if (Enum.IsDefined(typeof(DescriptionDay), value) || resultValue <= 8)
                        {
                            bool isOverTime = false;
                            foreach (var monthTimeSheetWorker in _monthTimeSheetWorkers)
                            {
                                string hour = monthTimeSheetWorker.Hours[day - 1];
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
                            }
                        }
                    }
                }


                double salary = 0;
                foreach (var monthTimeSheetWorker in _monthTimeSheetWorkers.Where(w => w.WorkerId == workerId))
                {
                    monthTimeSheetWorker.OverTime = 0;
                    monthTimeSheetWorker.VocationDays = 0;
                    monthTimeSheetWorker.SickDays = 0;
                    monthTimeSheetWorker.MissDays = 0;

                    foreach (var hour in monthTimeSheetWorker.Hours.Where(h => h != null))
                    {
                        if (Enum.IsDefined(typeof(DescriptionDay), hour))
                        {
                            var descriptionDay = (DescriptionDay)Enum.Parse(typeof(DescriptionDay), hour);

                            switch (descriptionDay)
                            {
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
                        }
                        else
                        {
                            double countHours = double.Parse(hour);

                            monthTimeSheetWorker.OverTime += countHours > 8 ? countHours - 8 : 0;
                            salary += countHours <= 8 ? countHours * monthTimeSheetWorker.SalaryInHour : (8 + ((countHours - 8) * 2)) * monthTimeSheetWorker.SalaryInHour;
                        }
                    }

                    ChangeCellValue(monthTimeSheetWorker.OverTime, rowIndex, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 1);
                    ChangeCellValue(monthTimeSheetWorker.VocationDays, rowIndex, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 2);
                    ChangeCellValue(monthTimeSheetWorker.SickDays, rowIndex, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 3);
                    ChangeCellValue(monthTimeSheetWorker.MissDays, rowIndex, COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + 4);
                }

                var monthTimeSheetWorkerFinalSalary = _monthTimeSheetWorkers.First(m => m.WorkerId == workerId && m.FullName != null);

                salary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.VocationPayment) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryAV) - double.Parse(monthTimeSheetWorkerFinalSalary.SalaryFenox) -
                       double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) - double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays.Value + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
               
                monthTimeSheetWorkerFinalSalary.FinalSalary = salary;

                ChangeCellValue(monthTimeSheetWorkerFinalSalary.FinalSalary.Value, rowIndexOfFullRow, columnIndexFinalSalary);
            }
            else
            {
                var date = new DateTime(_currentYear, _currentMonth, 1);
                switch (columnIndex - (COUNT_COLUMNS_BEFORE_DAYS + _prevCountLastDaysInMonth + COUNT_COLUMNS_AFTER_DAYS))
                {
                    case 1:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].PrepaymentCash))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "PrepaymentCash", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].PrepaymentCash) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].PrepaymentCash = value;
                            
                        }
                        break;
                    case 2:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].PrepaymentBankTransaction))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "PrepaymentBankTransaction", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].PrepaymentBankTransaction) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].PrepaymentBankTransaction = value;
                        }
                        break;
                    case 3:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].VocationPayment))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "VocationPayment", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].VocationPayment) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].VocationPayment = value;
                        }
                        break;
                    case 4:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].SalaryAV))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "SalaryAV", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].SalaryAV) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].SalaryAV = value;
                        }
                        break;
                    case 5:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].SalaryFenox))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "SalaryFenox", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].SalaryFenox) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].SalaryFenox = value;
                        }
                        break;
                    case 7:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].Inventory))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "Inventory", double.Parse(value));
                            ChangeFinalSalary(double.Parse(_monthTimeSheetWorkers[rowIndex].Inventory) - double.Parse(value), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].Inventory = value;
                        }
                        break;
                    case 9:
                        if (IsValidateDoubleValue(e, _monthTimeSheetWorkers[rowIndex].Bonus))
                        {
                            _bc.EditInfoMonthPayment(workerId, date, "Bonus", double.Parse(value));
                            ChangeFinalSalary(double.Parse(value) - double.Parse(_monthTimeSheetWorkers[rowIndex].Bonus), rowIndexOfFullRow, columnIndexFinalSalary);
                            _monthTimeSheetWorkers[rowIndex].Bonus = value;
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

        private void ChangeFinalSalary(double change, int rowIndexOfFullRow, int columnIndexFinalSalary)
        {
            var cell = DataGridMonthTimeSheet.GetCell(rowIndexOfFullRow, columnIndexFinalSalary);
            var t = cell.Content as TextBlock;
            t.Text = (double.Parse(t.Text) + change).ToString();
            _monthTimeSheetWorkers[rowIndexOfFullRow].FinalSalary = double.Parse(t.Text);
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
            var infoOverTimeView = new InfoOverTimeView();
            var infoOverTimeViewModel = new InfoOverTimeViewModel(_listDatesOfOverTime);

            infoOverTimeView.DataContext = infoOverTimeViewModel;
            infoOverTimeView.ShowDialog();

            for (int i = 0; i < _listDatesOfOverTime.Count; i++)
            {
                if (_bc.IsInfoOverTimeDate(_listDatesOfOverTime[i]))
                {
                    _listDatesOfOverTime.RemoveAt(i);
                }
            }
        }
    }
}
