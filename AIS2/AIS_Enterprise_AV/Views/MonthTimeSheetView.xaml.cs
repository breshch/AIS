using AIS_Enterprise_AV.Costs.ViewModels;
using AIS_Enterprise_AV.Costs.Views;
using AIS_Enterprise_AV.Helpers.Temps;
using AIS_Enterprise_AV.Reports;
using AIS_Enterprise_AV.ViewModels;
using AIS_Enterprise_AV.ViewModels.Helpers;
using AIS_Enterprise_AV.ViewModels.Infos;
using AIS_Enterprise_AV.Views.Directories;
using AIS_Enterprise_AV.Views.Helpers;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Models;
using AIS_Enterprise_Global.Models.Directories;
using AIS_Enterprise_Global.Models.Infos;
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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace AIS_Enterprise_AV.Views
{
    public partial class MonthTimeSheetView : Window
    {
        private BusinessContext _bc = new BusinessContext();
        private int _currentYear;
        private int _currentMonth;
        private int _countLastDaysInMonth;
        private List<MonthTimeSheetWorker> _monthTimeSheetWorkers = new List<MonthTimeSheetWorker>();
        private List<DateTime> _listDatesOfOverTime = new List<DateTime>();
        private List<DateTime> _weekends;

        private List<string> _privileges;
        private bool _isFirstLoad = true;

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

            ScreenWight();

            //Notes to upper 
            //var notes = _bc.GetDirectoryNotes();
            //foreach (var note in notes)
            //{
            //    note.Description = note.Description.Substring(0, 1).ToUpper() + note.Description.Substring(1);
            //}
            //_bc.SaveChanges();


            //var infoCash = _bc.DataContext.InfoCashes.First();
            //infoCash.Cash = 7388883.40;
            //_bc.SaveChanges();

            //foreach (var cost in _bc.DataContext.InfoCosts)
            //{
            //    cost.GroupId = Guid.NewGuid();
            //}

            //_bc.SaveChanges();


            InitializeGif();
            InitializePrivileges();
            _bc.InitializeAbsentDates();
            InitializeBrushes();
            InitializeDefaultCosts();
            InitializeYears();
        }

        private void InitializeDefaultCosts()
        {
            _bc.InitializeDefaultCosts();
        }

        private void InitializeYears()
        {
            ComboboxYears.ItemsSource = _bc.GetYears().OrderBy(y => y).ToList();
            if (ComboboxYears.Items.Count != 0)
            {
                ComboboxYears.SelectedIndex = ComboboxYears.Items.Count - 1;
            }
        }

        private MemoryStream _streamLoadingPicture;

        private void InitializeGif()
        {
            _streamLoadingPicture = new MemoryStream();
            var bitmap = Properties.Resources.LoadingPicture;
            bitmap.Save(_streamLoadingPicture, ImageFormat.Gif);
            _streamLoadingPicture.Position = 0;

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = _streamLoadingPicture;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            ImageBehavior.SetAnimatedSource(PictureLoading, image);
        }

        private void ScreenWight()
        {
            if (System.Windows.Forms.Screen.AllScreens.Count() > 1)
            {
                WindowMonthTimeSheet.SizeToContent = System.Windows.SizeToContent.Height;

                if (this.Width > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width)
                {
                    this.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
                }
            }

            this.Left = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Width / 2 - (this.Width / 2);
        }

        private void SettingMenuVisibility(string privilege, List<MenuItem> menuItems, int indexUnderLine)
        {
            int prevIndexUnderLine = indexUnderLine;
            indexUnderLine = privilege.IndexOf("_", prevIndexUnderLine + 1);

            string rule = "";
            if (indexUnderLine != -1)
            {
                rule = privilege.Substring(prevIndexUnderLine + 1, indexUnderLine - prevIndexUnderLine - 1);
            }
            else
            {
                rule = privilege.Substring(prevIndexUnderLine + 1);
            }

            var menuItem = menuItems.FirstOrDefault(m => m.Name == "Menu" + rule);
            if (menuItem != null)
            {
                if (menuItem.Visibility == System.Windows.Visibility.Collapsed)
                {
                    menuItem.Visibility = System.Windows.Visibility.Visible;
                }

                if (indexUnderLine != -1)
                {
                    var newMenuItems = menuItem.Items.Cast<MenuItem>().ToList();
                    SettingMenuVisibility(privilege, newMenuItems, indexUnderLine);
                }
            }
        }

        private void SettingMonthTimeSheetColumnsVisibility(string privilege)
        {
            var column = this.FindName("MonthTimeSheet" + privilege) as DataGridTextColumn;
            if (column != null && column.Visibility == System.Windows.Visibility.Collapsed)
            {
                column.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void SettingMonthTimeSheetColumnsNotReadOnly(string privilege)
        {
            var column = this.FindName("MonthTimeSheet" + privilege) as DataGridTextColumn;
            if (column != null && column.IsReadOnly)
            {
                column.IsReadOnly = false;
            }
        }

        private void InitializePrivileges()
        {
            var user = _bc.GetDirectoryUser(DirectoryUser.CurrentUserId);
            _privileges = user.CurrentUserStatus.DirectoryUserStatus.Privileges.Select(p => p.DirectoryUserStatusPrivilege.Name).ToList();

            foreach (var privilege in _privileges)
            {
                int indexUndelLine = privilege.IndexOf("_");
                string rule = privilege.Substring(0, indexUndelLine);

                var ruleEnum = (Rules)Enum.Parse(typeof(Rules), rule);
                string subPrivilege = privilege.Substring(indexUndelLine + 1);

                switch (ruleEnum)
                {
                    case Rules.MenuVisibility:
                        var menuItems = Menu.Items.Cast<MenuItem>().ToList();
                        SettingMenuVisibility(subPrivilege, menuItems, -1);
                        break;
                    case Rules.MonthTimeSheetColumnsVisibility:
                        SettingMonthTimeSheetColumnsVisibility(subPrivilege);
                        break;
                    case Rules.MonthTimeSheetColumnsNotReadOnly:
                        SettingMonthTimeSheetColumnsNotReadOnly(subPrivilege);
                        break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (_listDatesOfOverTime.Any())
            {
                ShowOverTimeView();
            }

            _streamLoadingPicture.Close();

            _bc.EditParameter("LastDate", DateTime.Now.ToString());

            _bc.Dispose();
        }

        private void ComboboxYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentYear = int.Parse(ComboboxYears.SelectedItem.ToString());
            ComboboxMonthes.ItemsSource = _bc.GetMonthes(_currentYear).OrderBy(m => m).ToList();

            if (ComboboxMonthes.Items.Count != 0)
            {
                ComboboxMonthes.SelectedIndex = ComboboxMonthes.Items.Count - 1;
            }
        }



        private void FillDataGrid()
        {
            var blur = new BlurEffect();
            //var current = this.Background;
            blur.Radius = 5;
            //this.Background = new SolidColorBrush(Colors.WhiteSmoke);
            //this.Effect = blur;
            DataGridMonthTimeSheet.Effect = blur;
            ComboboxYears.Effect = blur;
            ComboboxMonthes.Effect = blur;
            ButtonOverTimes.Effect = blur;
            TextBlockYear.Effect = blur;
            TextBlockMonth.Effect = blur;

            DataGridMonthTimeSheet.IsEnabled = false;
            ComboboxYears.IsEnabled = false;
            ComboboxMonthes.IsEnabled = false;

            PictureLoading.Visibility = System.Windows.Visibility.Visible;

            int countWorkDaysInMonth = _bc.GetCountWorkDaysInMonth(_currentYear, _currentMonth);

            var firstDateInMonth = new DateTime(_currentYear, _currentMonth, 1);
            var lastDateInMonth = HelperMethods.GetLastDateInMonth(_currentYear, _currentMonth);

            int prevCountLastDaysInMonth = _countLastDaysInMonth;
            _countLastDaysInMonth = lastDateInMonth.Day;

            var tmpMonthTimeSheetWorkers = new List<MonthTimeSheetWorker>();

            Task.Factory.StartNew(() =>
                {
                    bool isAdminSalary = _privileges.Contains(UserPrivileges.Salary_AdminSalary.ToString()) ? true : false;

                    _weekends = _bc.GetHolidays(_currentYear, _currentMonth).ToList();

                    var workers = _bc.GetDirectoryWorkersMonthTimeSheet(_currentYear, _currentMonth).ToList();

                    var infoDates = _bc.GetInfoDates(_currentYear, _currentMonth).ToList();

                    var infoMonthes = _bc.GetInfoMonthes(_currentYear, _currentMonth).ToList();
                    int indexWorker = 0;

                    var workerWarehouses = workers.Where(w => !w.IsDeadSpirit && _bc.GetDirectoryTypeOfPost(w.Id, lastDateInMonth).Name == "Склад").ToList();

                    AddingRowWorkers(workerWarehouses, tmpMonthTimeSheetWorkers, ref indexWorker, isAdminSalary, countWorkDaysInMonth, lastDateInMonth, firstDateInMonth, infoDates, infoMonthes);

                    if (HelperMethods.IsPrivilege(_privileges, UserPrivileges.WorkersVisibility_DeadSpirit))
                    {
                        var workerDeadSpirits = workers.Where(w => w.IsDeadSpirit).ToList();
                        AddingRowWorkers(workerDeadSpirits, tmpMonthTimeSheetWorkers, ref indexWorker, isAdminSalary, countWorkDaysInMonth, lastDateInMonth, firstDateInMonth, infoDates, infoMonthes);
                    }

                    if (HelperMethods.IsPrivilege(_privileges, UserPrivileges.WorkersVisibility_Office))
                    {
                        var workerOffices = workers.Where(w => !w.IsDeadSpirit && _bc.GetDirectoryTypeOfPost(w.Id, lastDateInMonth).Name == "Офис").ToList();
                        AddingRowWorkers(workerOffices, tmpMonthTimeSheetWorkers, ref indexWorker, isAdminSalary, countWorkDaysInMonth, lastDateInMonth, firstDateInMonth, infoDates, infoMonthes);
                    }
                }).ContinueWith((task) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            _bc.RefreshContext();

                            DataGridMonthTimeSheet.ItemsSource = null;
                            if (!_isFirstLoad)
                            {
                                for (int i = 0; i < prevCountLastDaysInMonth; i++)
                                {
                                    DataGridMonthTimeSheet.Columns.RemoveAt(6);
                                }
                            }
                            else
                            {
                                _isFirstLoad = false;
                            }

                            TextBlockCountWorkDays.Text = countWorkDaysInMonth + " " +
                                NumerizrFactory.Numerize("ru", countWorkDaysInMonth, "рабочий", "рабочих", "рабочих") + " " +
                                NumerizrFactory.Numerize("ru", countWorkDaysInMonth, "день", "дня", "дней");

                            var visibility = _privileges.Contains(UserPrivileges.MonthTimeSheetColumnsVisibility_Hours.ToString()) ? Visibility.Visible : Visibility.Collapsed;
                            bool isReadOnly = _privileges.Contains(UserPrivileges.MonthTimeSheetColumnsNotReadOnly_Hours.ToString()) ? false : true;

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
                                    Visibility = visibility,
                                    IsReadOnly = isReadOnly,
                                    CellStyle = Resources["CenterTextAlignmentCellWithRightClick"] as Style
                                };

                                DataGridMonthTimeSheet.Columns.Insert(6 + i, column);
                            }


                            _monthTimeSheetWorkers.Clear();
                            _monthTimeSheetWorkers.AddRange(tmpMonthTimeSheetWorkers);
                            DataGridMonthTimeSheet.ItemsSource = _monthTimeSheetWorkers;
                            DataGridMonthTimeSheet.Items.Refresh();

                            ScreenWight();

                            PictureLoading.Visibility = System.Windows.Visibility.Collapsed;

                            DataGridMonthTimeSheet.Effect = null;
                            ComboboxYears.Effect = null;
                            ComboboxMonthes.Effect = null;
                            ButtonOverTimes.Effect = null;
                            TextBlockYear.Effect = null;
                            TextBlockMonth.Effect = null;

                            DataGridMonthTimeSheet.IsEnabled = true;
                            ComboboxYears.IsEnabled = true;
                            ComboboxMonthes.IsEnabled = true;

                            //this.Effect = null;
                            //this.Background = current;
                            //this.IsEnabled = true;
                        }));

                    });


        }

        private void AddingRowWorkers(List<DirectoryWorker> workers, List<MonthTimeSheetWorker> monthTimeSheetWorkers, ref int indexWorker, bool isAdminSalary, int countWorkDaysInMonth,
            DateTime lastDateInMonth, DateTime firstDateInMonth, List<InfoDate> infoDates, List<InfoMonth> infoMonthes)
        {
            var sw2 = new Stopwatch();
            foreach (var worker in workers)
            {
                indexWorker++;

                var currentPosts = _bc.GetCurrentPosts(worker.Id, _currentYear, _currentMonth, _countLastDaysInMonth).ToList();

                bool isFirst = false;

                double salary = 0;
                foreach (var currentPost in currentPosts)
                {
                    var monthTimeSheetWorker = monthTimeSheetWorkers.FirstOrDefault(m => m.WorkerId == worker.Id && m.DirectoryPostId == currentPost.DirectoryPostId);

                    if (monthTimeSheetWorker == null)
                    {
                        monthTimeSheetWorker = new MonthTimeSheetWorker();

                        monthTimeSheetWorkers.Add(monthTimeSheetWorker);

                        monthTimeSheetWorker.WorkerId = worker.Id;

                        if (!isFirst)
                        {
                            monthTimeSheetWorker.FullName = worker.FullName;
                            isFirst = true;
                        }

                        monthTimeSheetWorker.IsOdd = indexWorker % 2 == 0;

                        monthTimeSheetWorker.DirectoryPostId = currentPost.DirectoryPostId;
                        monthTimeSheetWorker.PostChangeDate = currentPost.ChangeDate;

                        string postName = currentPost.DirectoryPost.Name;
                        if (postName.IndexOf('_') != -1)
                        {
                            postName = postName.Substring(0, postName.IndexOf('_'));
                        }

                        monthTimeSheetWorker.PostName = postName;
                        monthTimeSheetWorker.SalaryInHour = Math.Round((isAdminSalary ? currentPost.DirectoryPost.AdminWorkerSalary.Value :
                            currentPost.DirectoryPost.UserWorkerSalary) / countWorkDaysInMonth / 8, 2);
                        monthTimeSheetWorker.IsDeadSpirit = worker.IsDeadSpirit;

                        monthTimeSheetWorker.Hours = new string[lastDateInMonth.Day];
                    }



                    int indexHour = 0;
                    for (DateTime date = firstDateInMonth; worker.FireDate == null && date.Date <= lastDateInMonth.Date || date.Date <= lastDateInMonth.Date && worker.FireDate != null && date.Date <= worker.FireDate.Value.Date; date = date.AddDays(1))
                    {
                        if (date.Date >= currentPost.ChangeDate.Date && (currentPost.FireDate == null || currentPost.FireDate != null && currentPost.FireDate.Value.Date >= date.Date))
                        {
                            var day = infoDates.FirstOrDefault(d => d.DirectoryWorkerId == worker.Id && d.Date.Date == date.Date);

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

                var monthTimeSheetWorkerFinalSalary = monthTimeSheetWorkers.First(m => m.WorkerId == worker.Id && m.FullName != null);

                var infoMonth = infoMonthes.FirstOrDefault(m => m.DirectoryWorkerId == worker.Id && m.Date.Year == _currentYear && m.Date.Month == _currentMonth);
                if (infoMonth != null)
                {
                    monthTimeSheetWorkerFinalSalary.PrepaymentCash = infoMonth.PrepaymentCash.ToString();
                    monthTimeSheetWorkerFinalSalary.PrepaymentBankTransaction = infoMonth.PrepaymentBankTransaction.ToString();
                    monthTimeSheetWorkerFinalSalary.Compensation = infoMonth.Compensation.ToString();
                    monthTimeSheetWorkerFinalSalary.VocationPayment = infoMonth.VocationPayment.ToString();
                    monthTimeSheetWorkerFinalSalary.CardAV = infoMonth.CardAV.ToString();
                    monthTimeSheetWorkerFinalSalary.CardFenox = infoMonth.CardFenox.ToString();
                    monthTimeSheetWorkerFinalSalary.Panalty = infoDates.Where(d => d.DirectoryWorkerId == worker.Id && d.InfoPanalty != null).Sum(d => d.InfoPanalty.Summ).ToString();
                    monthTimeSheetWorkerFinalSalary.Inventory = infoMonth.Inventory.ToString();
                    monthTimeSheetWorkerFinalSalary.BirthDays = infoMonth.BirthDays;
                    monthTimeSheetWorkerFinalSalary.Bonus = infoMonth.Bonus.ToString();
                    monthTimeSheetWorkerFinalSalary.FinalSalary = salary - double.Parse(monthTimeSheetWorkerFinalSalary.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalary.Panalty) -
                        double.Parse(monthTimeSheetWorkerFinalSalary.Inventory) - monthTimeSheetWorkerFinalSalary.BirthDays + double.Parse(monthTimeSheetWorkerFinalSalary.Bonus);
                }
            }

            Debug.WriteLine(sw2.ElapsedMilliseconds);
        }

        private void ComboboxMonthes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboboxMonthes.SelectedItem != null)
            {
                _currentMonth = int.Parse(ComboboxMonthes.SelectedItem.ToString());

                ButtonOverTimes.IsEnabled = _bc.GetInfoOverTimeDates(_currentYear, _currentMonth).Any();

                FillDataGrid();
            }
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

            if (indexColumn > COUNT_COLUMNS_BEFORE_DAYS && indexColumn <= _countLastDaysInMonth + COUNT_COLUMNS_BEFORE_DAYS)
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


                bool isWeekend = _bc.IsWeekend(date);
                double resultValue;
                if (double.TryParse(value, out resultValue))
                {
                    if ((resultValue > 8 || isWeekend))
                    {
                        if (!_listDatesOfOverTime.Contains(date))
                        {
                            _listDatesOfOverTime.Add(date);

                            if (!ButtonOverTimes.IsEnabled)
                            {
                                ButtonOverTimes.IsEnabled = true;
                            }
                        }

                        double sumOverTimes = 0;
                        int countOverTimedWorkers = 0;
                        foreach (var monthTimeSheetWorkerTemp in _monthTimeSheetWorkers.Where(w => !w.IsDeadSpirit))
                        {
                            string hour = monthTimeSheetWorkerTemp.Hours[day - 1];
                            if (hour != null)
                            {
                                double hourValue;
                                if (double.TryParse(hour, out hourValue))
                                {
                                    if (!isWeekend)
                                    {
                                        if (hourValue > 8)
                                        {
                                            sumOverTimes += hourValue - 8;
                                            countOverTimedWorkers++;
                                        }
                                    }
                                    else
                                    {
                                        sumOverTimes += hourValue;
                                        countOverTimedWorkers++;
                                    }
                                }
                            }
                        }

                        double hoursSpiritWorker;
                        if (!isWeekend)
                        {
                            hoursSpiritWorker = 8 + Math.Round(sumOverTimes / countOverTimedWorkers, 1);
                        }
                        else
                        {
                            hoursSpiritWorker = Math.Round(sumOverTimes / countOverTimedWorkers, 1);
                        }

                        var deadSpiritWorkers = _bc.GetDeadSpiritDirectoryWorkers(date).ToList();

                        foreach (var deadSpiritWorker in deadSpiritWorkers)
                        {
                            _bc.EditDeadSpiritHours(deadSpiritWorker.Id, date, hoursSpiritWorker);

                            if (HelperMethods.IsPrivilege(_privileges, UserPrivileges.WorkersVisibility_DeadSpirit))
                            {
                                var monthTimeSheetDeadSpiritWorker = _monthTimeSheetWorkers.First(w => w.WorkerId == deadSpiritWorker.Id);

                                int indexRowDeadSpirit = _monthTimeSheetWorkers.IndexOf(monthTimeSheetDeadSpiritWorker);
                                ChangeCellValue(hoursSpiritWorker, indexRowDeadSpirit, COUNT_COLUMNS_BEFORE_DAYS + day);
                                monthTimeSheetDeadSpiritWorker.Hours[day - 1] = hoursSpiritWorker.ToString();

                                monthTimeSheetDeadSpiritWorker.OverTime = 0;
                                double salaryDeadSpirit = 0;
                                for (int currentDay = 0; currentDay < monthTimeSheetDeadSpiritWorker.Hours.Count(); currentDay++)
                                {
                                    string hour = monthTimeSheetDeadSpiritWorker.Hours[currentDay];

                                    if (hour != null)
                                    {
                                        double countHours;
                                        if (double.TryParse(hour, out countHours))
                                        {
                                            if (!_weekends.Any(h => h.Date.Date == new DateTime(_currentYear, _currentMonth, currentDay + 1)))
                                            {
                                                monthTimeSheetDeadSpiritWorker.OverTime += countHours > 8 ? countHours - 8 : 0;
                                                salaryDeadSpirit += countHours <= 8 ? countHours * monthTimeSheetDeadSpiritWorker.SalaryInHour : (8 + ((countHours - 8) * 2)) * monthTimeSheetDeadSpiritWorker.SalaryInHour;
                                            }
                                            else
                                            {
                                                monthTimeSheetDeadSpiritWorker.OverTime += countHours;
                                                salaryDeadSpirit += countHours * monthTimeSheetDeadSpiritWorker.SalaryInHour * 2;
                                            }
                                        }
                                    }

                                    ChangeCellValue(monthTimeSheetDeadSpiritWorker.OverTime, indexRowDeadSpirit, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + 1);

                                }

                                var monthTimeSheetWorkerFinalSalaryDeadSpirit = _monthTimeSheetWorkers.First(m => m.WorkerId == workerId && m.FullName != null);

                                salaryDeadSpirit = salaryDeadSpirit - double.Parse(monthTimeSheetWorkerFinalSalaryDeadSpirit.PrepaymentCash) - double.Parse(monthTimeSheetWorkerFinalSalaryDeadSpirit.Panalty) -
                                    double.Parse(monthTimeSheetWorkerFinalSalaryDeadSpirit.Inventory) - monthTimeSheetWorkerFinalSalaryDeadSpirit.BirthDays.Value + double.Parse(monthTimeSheetWorkerFinalSalaryDeadSpirit.Bonus);

                                monthTimeSheetWorkerFinalSalaryDeadSpirit.FinalSalary = salaryDeadSpirit;

                                ChangeCellValue(monthTimeSheetWorkerFinalSalaryDeadSpirit.FinalSalary.Value, indexRowDeadSpirit, columnIndexFinalSalary);
                            }
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

                    ChangeCellValue(monthTimeSheetWorkerTemp.OverTime, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + 1);
                    ChangeCellValue(monthTimeSheetWorkerTemp.VocationDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + 2);
                    ChangeCellValue(monthTimeSheetWorkerTemp.SickDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + 3);
                    ChangeCellValue(monthTimeSheetWorkerTemp.MissDays, indexRow, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + 4);
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
                switch (indexColumn - (COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + COUNT_COLUMNS_AFTER_DAYS))
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
            var cell = DataGridMonthTimeSheet.GetCell(rowIndexOfFullRow, COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + COLUMN_FINAL_SALARY_AFTER_DAYS);
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
            var infoOverTimeViewModel = new InfoOverTimeViewModel(_listDatesOfOverTime, new DateTime(_currentYear, _currentMonth, 1), new DateTime(_currentYear, _currentMonth, _countLastDaysInMonth));

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
                            new DateTime(_currentYear, _currentMonth, _countLastDaysInMonth));
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

                if (columnIndex == COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + COLUMN_PANALTIES_AFTER_DAYS)
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

                    if (indexColumn > COUNT_COLUMNS_BEFORE_DAYS && indexColumn <= COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth && monthTimeSheetWorker.Hours[day - 1] != null)
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

            int indexColumnPanalty = COUNT_COLUMNS_BEFORE_DAYS + _countLastDaysInMonth + COLUMN_PANALTIES_AFTER_DAYS;

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

            for (int indexHour = 0; indexHour < _countLastDaysInMonth; indexHour++)
            {
                var value = monthTimeSheetWorker.Hours[indexHour];
                ColorizeCell(value, indexRow, COUNT_COLUMNS_BEFORE_DAYS + indexHour + 1, monthTimeSheetWorker.IsOdd);
            }

            this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2 - (this.Height / 2);
        }

        private void ColorizeCell(string value, int indexRow, int indexColumn, bool isOdd)
        {
            if (value != null)
            {
                var cell = DataGridMonthTimeSheet.GetCell(indexRow, indexColumn);

                if (cell != null)
                {

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

        private void MenuCompanies_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new DirectoryCompanyViewModel(), new DirectoryCompanyView());
        }

        private void MenuRCs_Click(object sender, RoutedEventArgs e)
        {
            var directoryRCViewModel = new DirectoryRCViewModel();
            var directoryRCView = new DirectoryRCView();

            directoryRCView.DataContext = directoryRCViewModel;
            directoryRCView.ShowDialog();
        }

        private void MenuTypeOfPosts_Click(object sender, RoutedEventArgs e)
        {
            var directoryTypeOfPostViewModel = new DirectoryTypeOfPostViewModel();
            var directoryTypeOfPostView = new DirectoryTypeOfPostView();

            directoryTypeOfPostView.DataContext = directoryTypeOfPostViewModel;
            directoryTypeOfPostView.ShowDialog();
        }

        private void MenuListOfPosts_Click(object sender, RoutedEventArgs e)
        {
            var directoryPostViewModel = new DirectoryPostViewModel();
            var directoryPostView = new DirectoryPostView();

            directoryPostView.DataContext = directoryPostViewModel;
            directoryPostView.ShowDialog();
        }

        private void MenuListOfWorkers_Click(object sender, RoutedEventArgs e)
        {
            var directoryWorkerListViewModel = new DirectoryWorkerListViewModel();
            var directoryWorkerListView = new DirectoryWorkerListView();

            directoryWorkerListView.DataContext = directoryWorkerListViewModel;
            directoryWorkerListView.ShowDialog();
        }

        private void MenuAddingWorker_Click(object sender, RoutedEventArgs e)
        {
            var directoryWorkerViewModel = new DirectoryAddWorkerViewModel();
            var directoryWorkerView = new DirectoryAddWorkerView();

            directoryWorkerView.DataContext = directoryWorkerViewModel;
            directoryWorkerView.ShowDialog();
        }

        private void MenuReportSalary_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new MonthReportViewModel(
                "Зарплата",
                (BC, SelectedYear, SelectedMonth) =>
                {
                    WorkerSalaryReports.SalaryOvertimeTransport(BC, SelectedYear, SelectedMonth);
                    WorkerSalaryReports.ComplitedReportSalaryWorkers(BC, SelectedYear, SelectedMonth);
                },
                (BC) => BC.GetYears().ToList(),
                (BC, year) => BC.GetMonthes(year).ToList()
                ), new MonthReportView());
        }

        private void MenuUserStatuses_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new DirectoryUserStatusesViewModel(), new DirectoryUserStatusesView());
        }

        private void MenuUsers_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new DirectoryUsersViewModel(), new DirectoryUsersView());
        }

        private void MenuReportCosts_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new MonthReportViewModel(
               "Затраты",
               (BC, SelectedYear, SelectedMonth) =>
               {
                   var directoryRCs = BC.GetDirectoryRCsMonthIncoming(SelectedYear, SelectedMonth).ToList();

                   foreach (var rc in directoryRCs)
                   {
                       CashReports.IncomingRC(rc, BC, SelectedYear, SelectedMonth);
                   }

                   CashReports.Incoming26(BC, SelectedYear, SelectedMonth);

                   CashReports.Expense26(BC, SelectedYear, SelectedMonth);

                   CashReports.ExpenseRCs(BC, SelectedYear, SelectedMonth);
               },
               (BC) => BC.GetInfoCostYears().ToList(),
               (BC, year) => BC.GetInfoCostMonthes(year).ToList()
               ), new MonthReportView());
        }

        private void MenuDayCosts_Click(object sender, RoutedEventArgs e)
        {
            var costView = new DayCostsView(DateTime.Now);
            costView.ShowDialog();
        }

        private void MenuMonthCosts_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new MonthCostsViewModel(), new MonthCostsView());
        }

        private void MenuDefaultCosts_Click(object sender, RoutedEventArgs e)
        {
            HelperMethods.ShowView(new DefaultCostsViewModel(), new DefaultCostsView());
        }


    }


}
