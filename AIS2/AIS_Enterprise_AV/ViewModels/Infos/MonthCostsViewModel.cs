using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using AIS_Enterprise_AV.Auth;
using AIS_Enterprise_AV.Models;
using AIS_Enterprise_AV.Views.Infos;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Infos;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class MonthCostsViewModel : ViewModelGlobal
    {
        #region Base

        private bool _isNotTransportOnly;
        private List<InfoCost> _costs;

        public MonthCostsViewModel()
        {
            var firstWorkingArea = Screen.AllScreens[0].WorkingArea;
            MaxHeightForm = firstWorkingArea.Height - 100;

            _isNotTransportOnly = Privileges.HasAccess(UserPrivileges.CostsVisibility_IsNotTransportOnly);

            Costs = new ObservableCollection<InfoCost>();

            Years = new ObservableCollection<int>(BC.GetInfoCostYears());
            if (Years.Any())
            {
                SelectedYear = Years.Last();
            }

            IncomingsAndExpenses = new ObservableCollection<IncomingAndExpense>();
            IncomingsAndExpenses.Add(new IncomingAndExpense());

            ShowDayCostsCommand = new RelayCommand(ShowDayCosts);
            FilterCommand = new RelayCommand(FilterShow);
            ReturnCommand = new RelayCommand(Return);

            ReturnName = "Компенсация";
            ReturnVisibility = Visibility.Collapsed;

            ReturnButtonVisibility = _isNotTransportOnly ? Visibility.Visible : Visibility.Collapsed;

            VisibilityCash = _isNotTransportOnly ? Visibility.Visible : Visibility.Collapsed;


            BC.DataContext.Configuration.AutoDetectChangesEnabled = false;

            CostItems = new ObservableCollection<DirectoryCostItem>(BC.GetDirectoryCostItems());
            var costItemEmpty = new DirectoryCostItem { Name = null };
            CostItems.Insert(0, costItemEmpty);

            InOuts = new ObservableCollection<string> 
            {
                "",
                "Приход",
                "Расход"
            };
            SelectedInOut = InOuts.First();

            TransportCompanies = new ObservableCollection<DirectoryTransportCompany>(BC.GetDirectoryTransportCompanies());
            var transportCompanyEmpty = new DirectoryTransportCompany { Name = null };
            TransportCompanies.Insert(0, transportCompanyEmpty);
            SelectedTransportCompany = TransportCompanies.First();

            Notes = new ObservableCollection<DirectoryNote>(BC.GetDirectoryNotes().ToList());
            var noteEmpty = new DirectoryNote { Description = null };
            Notes.Insert(0, noteEmpty);

            SelectedNote = Notes.First();

            SelectedCostItem = CostItems.First();

            BC.DataContext.Configuration.AutoDetectChangesEnabled = true;

			InfoCost.OnChangeIsReturn += InfoCost_OnChangeIsReturn;
        }

		private void InfoCost_OnChangeIsReturn()
		{
			IncomingsAndExpenses.First().ExpenseCompensation = Costs.Where(x => x.IsReturn).Sum(c => !c.IsIncoming ? c.Summ : 0);
		}

        private void Filter()
        {
            Costs.Clear();
            var costs = _costs;

            if (costs == null)
            {
                costs = new List<InfoCost>();
            }

            if (SelectedCostItem != null && SelectedCostItem.Name != null)
            {
                costs = costs.Where(c => c.DirectoryCostItem.Name == SelectedCostItem.Name).ToList();
            }

            if (SelectedRC != null && SelectedRC.Name != null)
            {
                costs = costs.Where(c => c.DirectoryRC.Name == SelectedRC.Name).ToList();
            }

            if (SelectedInOut != null && SelectedInOut != "")
            {
                costs = costs.Where(c => SelectedInOut == "Приход" ? c.IsIncoming : !c.IsIncoming).ToList();
            }

            if (SelectedTransportCompany != null && SelectedTransportCompany.Name != null)
            {
                costs = costs.Where(c => c.DirectoryTransportCompany != null && c.DirectoryTransportCompany.Name == SelectedTransportCompany.Name).ToList();
            }

            if (SelectedNote != null && SelectedNote.Description != null)
            {
                costs = costs.Where(c => c.CurrentNotes.Any(n => n.DirectoryNote.Description == SelectedNote.Description)).ToList();
            }


            costs = costs.OrderByDescending(c => c.Date).ToList();

            foreach (var cost in costs)
            {
                Costs.Add(cost);
            }

            SummChange();
        }

        private void OrderNotes()
        {
            var noteEmpty = Notes.Where(n => n.Description == null).ToList();
            var noteLetters = Notes.Where(n => n.Description != null && char.IsLetter(n.Description[0])).OrderBy(n => n.Description).ToList();
            var noteDigits = Notes.Where(n => n.Description != null && char.IsDigit(n.Description[0])).OrderBy(n => n.Description).ToList();

            Notes.Clear();

            foreach (var note in noteEmpty)
            {
                Notes.Add(note);
            }

            foreach (var note in noteLetters)
            {
                Notes.Add(note);
            }

            foreach (var note in noteDigits)
            {
                Notes.Add(note);
            }
        }

        private void SummChange()
        {
            IncomingsAndExpenses.First().Incoming = Costs.Sum(c => c.IsIncoming ? c.Summ : 0);
            IncomingsAndExpenses.First().Expense = Costs.Sum(c => !c.IsIncoming ? c.Summ : 0);
        }

        #endregion


        #region Properties

        public ObservableCollection<IncomingAndExpense> IncomingsAndExpenses { get; set; }

        public ObservableCollection<int> Years { get; set; }

        private int _selectedYear;
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                RaisePropertyChanged();

                Monthes = new ObservableCollection<int>(BC.GetInfoCostMonthes(_selectedYear));
                if (Monthes.Any())
                {
                    SelectedMonth = Monthes.Last();
                }
            }
        }
        public ObservableCollection<int> Monthes { get; set; }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged();

				RCs = new ObservableCollection<DirectoryRC>(BC.GetDirectoryRCs());
				var rcEmpty = new DirectoryRC { Name = null };
				RCs.Insert(0, rcEmpty);

                Costs.Clear();
                _costs = new List<InfoCost>();

                if (_isNotTransportOnly)
                {
                    foreach (var cost in BC.GetInfoCosts(SelectedYear, _selectedMonth))
                    {
                        Costs.Add(cost);
                    }
                }
                else
                {
                    foreach (var cost in BC.GetInfoCostsTransportAndNoAllAndExpenseOnly(SelectedYear, _selectedMonth))
                    {
                        Costs.Add(cost);
                    }
                }

                _costs.AddRange(Costs);
                RefreshFilters();
            }
        }
        public double Summ { get; set; }

        public ObservableCollection<InfoCost> Costs { get; set; }

        public InfoCost SelectedCost { get; set; }

        public Visibility VisibilityCash { get; set; }

        public ObservableCollection<DirectoryCostItem> CostItems { get; set; }


        private DirectoryCostItem _selectedCostItem;
        public DirectoryCostItem SelectedCostItem
        {
            get
            {
                return _selectedCostItem;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedCostItem = value;
                RaisePropertyChanged();

                RefreshFilters();
            }
        }

        private void RefreshFilters()
        {
            if (RCs == null || InOuts == null || TransportCompanies == null || Notes == null)
            {
                return;
            }

            SelectedRC = RCs.First();
            SelectedInOut = InOuts.First();
            SelectedTransportCompany = TransportCompanies.First();
            SelectedNote = Notes.First();

            Filter();

            BC.DataContext.Configuration.AutoDetectChangesEnabled = false;

            RCs.Clear();
            var rcEmpty = new DirectoryRC { Name = null };
            RCs.Add(rcEmpty);

            foreach (var rc in Costs.Select(c => c.DirectoryRC).Distinct())
            {
                RCs.Add(rc);
            }

            InOuts.Clear();
            InOuts.Add("");

            if (Costs.Any(c => c.IsIncoming))
            {
                InOuts.Add("Приход");
            }

            if (Costs.Any(c => !c.IsIncoming))
            {
                InOuts.Add("Расход");
            }


            TransportCompanies.Clear();
            var transportCompanyEmpty = new DirectoryTransportCompany { Name = null };
            TransportCompanies.Add(transportCompanyEmpty);

            foreach (var transportCompany in Costs.Select(c => c.DirectoryTransportCompany).Where(t => t != null).Distinct())
            {
                TransportCompanies.Add(transportCompany);
            }


            Notes.Clear();
            var noteEmpty = new DirectoryNote { Description = null };
            Notes.Add(noteEmpty);

            foreach (var masNotes in Costs.Select(c => c.CurrentNotes.Select(n => n.DirectoryNote)))
            {
                foreach (var note in masNotes)
                {
                    if (!Notes.Contains(note))
                    {
                        Notes.Add(note);
                    }
                }
            }

            OrderNotes();

            BC.DataContext.Configuration.AutoDetectChangesEnabled = true;
        }

        public ObservableCollection<DirectoryRC> RCs { get; set; }


        private DirectoryRC _selectedRC;
        public DirectoryRC SelectedRC
        {
            get
            {
                return _selectedRC;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedRC = value;
                RaisePropertyChanged();

                Filter();
            }
        }

        public ObservableCollection<string> InOuts { get; set; }

        private string _selectedInOut;
        public string SelectedInOut
        {
            get
            {
                return _selectedInOut;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedInOut = value;

                Filter();
            }
        }

        public ObservableCollection<DirectoryTransportCompany> TransportCompanies { get; set; }

        private DirectoryTransportCompany _selectedTransportCompany;
        public DirectoryTransportCompany SelectedTransportCompany
        {
            get
            {
                return _selectedTransportCompany;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedTransportCompany = value;
                RaisePropertyChanged();

                Filter();
            }
        }

        public ObservableCollection<DirectoryNote> Notes { get; set; }

        private DirectoryNote _selectedNote;
        public DirectoryNote SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedNote = value;
                RaisePropertyChanged();

                Filter();
            }
        }

        public string ReturnName { get; set; }

        public Visibility ReturnVisibility { get; set; }
        public Visibility ReturnButtonVisibility { get; set; }

        public int MaxHeightForm { get; set; }

        #endregion


        #region Commands

        public RelayCommand ShowDayCostsCommand { get; set; }
        public RelayCommand FilterCommand { get; set; }
        public RelayCommand ReturnCommand { get; set; }

        private void FilterShow(object parameter)
        {
            var grid = parameter as Grid;
            grid.Visibility = grid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

            if (grid.Visibility == Visibility.Collapsed)
            {
                SelectedCostItem = CostItems.First();
                SelectedRC = RCs.First();
                SelectedInOut = InOuts.First();
                SelectedTransportCompany = TransportCompanies.First();
                SelectedNote = Notes.First();
            }
        }

        private void ShowDayCosts(object parameter)
        {
            if (SelectedCost != null)
            {
                var dayCostsView = new DayCostsView(SelectedCost.Date);
                dayCostsView.ShowDialog();

                _costs.Clear();
                _costs = BC.GetInfoCosts(SelectedYear, SelectedMonth).ToList();
                Filter();

                BC.RefreshContext();
            }

        }

        private void Return(object parameter)
        {
            if (ReturnName == "Добавить компесацию")
            {
                var returnCosts = Costs.Where(c => c.IsReturn && !c.IsIncoming);
                foreach (var cost in returnCosts)
                {
                    var transports = new List<Transport>();

                    bool isFirst = false;
                    foreach (var note in cost.CurrentNotes)
                    {
                        transports.Add(new Transport
                            {
                                DirectoryRC = BC.GetDirectoryRC("КО-5"),
                                DirectoryNote = note.DirectoryNote,
                                Weight = !isFirst ? cost.Weight : 0
                            });

                        if (!isFirst)
                        {
                            isFirst = true;
                        }
                    }

                    BC.AddInfoCosts(cost.Date, cost.DirectoryCostItem, true, cost.DirectoryTransportCompany, cost.Summ, cost.Currency, transports);
                }

                _costs.Clear();
                _costs = BC.GetInfoCosts(SelectedYear, SelectedMonth).ToList();
                foreach (var cost in _costs)
                {
                    if (cost.IsReturn)
                    {
                        cost.IsReturn = false;    
                    }
                }

                Filter();

                BC.RefreshContext();
            }

            ReturnName = ReturnName == "Компенсация" ? "Добавить компесацию" : "Компенсация";
            ReturnVisibility = ReturnVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

    }
}
