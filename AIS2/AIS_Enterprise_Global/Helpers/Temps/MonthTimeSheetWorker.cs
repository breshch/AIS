using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Global.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private string _prepaymentCash;
        [StopNotify]
        public string PrepaymentCash
        {
            get
            {
                return _prepaymentCash;
            }
            set
            {
                _prepaymentCash = EditProperty(value, _prepaymentCash, "PrepaymentCash");
            }
        }

        private string _prepaymentBankTransaction;
        [StopNotify]
        public string PrepaymentBankTransaction
        {
            get
            {
                return _prepaymentBankTransaction;
            }
            set
            {
                _prepaymentBankTransaction = EditProperty(value, _prepaymentBankTransaction, "PrepaymentBankTransaction");
            }
        }

        private string _vocationPayment;
        [StopNotify]
        public string VocationPayment
        {
            get
            {
                return _vocationPayment;
            }
            set
            {
                _vocationPayment = EditProperty(value, _vocationPayment, "VocationPayment");
            }
        }

        private string _salaryAV;
        [StopNotify]
        public string SalaryAV
        {
            get
            {
                return _salaryAV;
            }
            set
            {
                _salaryAV = EditProperty(value, _salaryAV, "SalaryAV");
            }
        }

        private string _salaryFenox;
        [StopNotify]
        public string SalaryFenox
        {
            get
            {
                return _salaryFenox;
            }
            set
            {
                _salaryFenox = EditProperty(value, _salaryFenox, "SalaryFenox");
            }
        }

        private string _panalty;
        [StopNotify]
        public string Panalty
        {
            get
            {
                return _panalty;
            }
            set
            {
                _panalty = EditProperty(value, _panalty, "Panalty"); 
            }
        }

        private string _inventory;
        [StopNotify]
        public string Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                _inventory = EditProperty(value, _inventory, "Inventory"); 
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

        private string _bonus;
        [StopNotify]
        public string Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = EditProperty(value, _bonus, "Bonus");
            }
        }

        public double? FinalSalary { get; set; }

        private string EditProperty(string value, string property, string propertyName)
        {
            value = value.ToUpper().Replace(".", ",");

            if (property == value)
            {
                return property;
            }

            double prevProperty = 0;
            if (property != null)
            {
                prevProperty = double.Parse(property);
            }

            double result;
            if (!double.TryParse(value, out result))
            {
                MessageBox.Show("Введите только число.");
                return property;
            }

            if (result < 0)
            {
                MessageBox.Show("Введите только число, большее, либо равное 0.");
                return property;
            }

            property = value;
            OnPropertyChanged();

            BC.EditInfoMonthPayment(WorkerId, Date, propertyName, result);

            FinalSalary += prevProperty - result;

            return property;
        }
    }
}
