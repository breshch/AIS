using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class MonthTimeSheetWorker : ViewModel
    {
        public int WorkerId { get; set; }
        public DateTime Date { get; set; }
        public string FullName { get; set; }
        public int DirectoryPostId { get; set; }
        public DateTime PostChangeDate { get; set; }
        public string PostName { get; set; }
        public double SalaryInHour { get; set; }
        public ObservableCollection<HourWorker> Hours { get; set; }

        public double OverTime { get; set; }
        public int VocationDays { get; set; }
        public int SickDays { get; set; }
        public int MissDays { get; set; }

        private double? _prepaymentCash;
        [StopNotify]
        public double? PrepaymentCash
        {
            get
            {
                return _prepaymentCash;
            }
            set
            {
                var prevPrepaymentCash = _prepaymentCash;

                _prepaymentCash = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "PrepaymentCash", _prepaymentCash.Value);

                FinalSalary += prevPrepaymentCash - _prepaymentCash;
            }
        }

        private double? _prepaymentBankTransaction;
        [StopNotify]
        public double? PrepaymentBankTransaction
        {
            get
            {
                return _prepaymentBankTransaction;
            }
            set
            {
                var prevPrepaymentBankTransaction = _prepaymentBankTransaction;

                _prepaymentBankTransaction = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "PrepaymentBankTransaction", _prepaymentBankTransaction.Value);

                FinalSalary += prevPrepaymentBankTransaction - _prepaymentBankTransaction;
            }
        }

        private double? _vocationPayment;
        [StopNotify]
        public double? VocationPayment
        {
            get
            {
                return _vocationPayment;
            }
            set
            {
                var prevVocationPayment = _vocationPayment;

                _vocationPayment = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "VocationPayment", _vocationPayment.Value);

                FinalSalary += prevVocationPayment - _vocationPayment;
            }
        }

        private double? _salaryAV;
        [StopNotify]
        public double? SalaryAV
        {
            get
            {
                return _salaryAV;
            }
            set
            {
                var prevSalaryAV = _salaryAV;

                _salaryAV = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "SalaryAV", _salaryAV.Value);

                FinalSalary += prevSalaryAV - _salaryAV;
            }
        }

        private double? _salaryFenox;
        [StopNotify]
        public double? SalaryFenox
        {
            get
            {
                return _salaryFenox;
            }
            set
            {
                var prevSalaryFenox = _salaryFenox;

                _salaryFenox = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "SalaryFenox", _salaryFenox.Value);

                FinalSalary += prevSalaryFenox - _salaryFenox;
            }
        }

        private double? _panalty;
        [StopNotify]
        public double? Panalty
        {
            get
            {
                return _panalty;
            }
            set
            {
                var prevPanalty = _panalty;

                _panalty = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "Panalty", _panalty.Value);

                FinalSalary += prevPanalty - _panalty;
            }
        }

        private double? _inventory;
        [StopNotify]
        public double? Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                var prevInventory = _inventory;

                _inventory = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "Inventory", _inventory.Value);

                FinalSalary += prevInventory - _inventory;
            }
        }

        private double? _birthDays;
        [StopNotify]
        public double? BirthDays
        {
            get
            {
                return _birthDays;
            }
            set
            {
                var prevBirthDays = _birthDays;

                _birthDays = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "BirthDays", _birthDays.Value);

                FinalSalary += prevBirthDays - _birthDays;
            }
        }

        private double? _bonus;
        [StopNotify]
        public double? Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                var prevBonus = _bonus;

                _bonus = value;
                OnPropertyChanged();

                BC.EditInfoMonthPayment(WorkerId, Date, "Bonus", _bonus.Value);

                FinalSalary += prevBonus - _bonus;
            }
        }

        public double? FinalSalary { get; set; }
    }
}
