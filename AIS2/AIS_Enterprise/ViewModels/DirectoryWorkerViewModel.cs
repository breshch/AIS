using AIS_Enterprise.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.ViewModels
{
    public class DirectoryWorkerViewModel : ViewModel
    {
        public DirectoryWorkerViewModel()
        {
            
        }

        #region DirectoryWorkerLastName
        
        private string _directoryWorkerLastName;
        public string DirectoryWorkerLastName
        {
            get
            {
                return _directoryWorkerLastName; 
            }
            set
            {
                _directoryWorkerLastName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerLastName");
            }
        }

        public string ValidateDirectoryWorkerLastName
        {
            get 
            {
                return Validations.ValidateText(DirectoryWorkerLastName, "Фамилия", 32);
            }
        }

        #endregion

        #region DirectoryWorkerFirstName

        private string _directoryWorkerFirstName;
        public string DirectoryWorkerFirstName
        {
            get
            {
                return _directoryWorkerFirstName;
            }
            set
            {
                _directoryWorkerFirstName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerFirstName");
            }
        }

        public string ValidateDirectoryWorkerFirstName
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerFirstName, "Имя", 32);
            }
        }

        #endregion

        #region DirectoryWorkerMidName

        private string _directoryWorkerMidName;
        public string DirectoryWorkerMidName
        {
            get
            {
                return _directoryWorkerMidName;
            }
            set
            {
                _directoryWorkerMidName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerMidName");
            }
        }

        public string ValidateDirectoryWorkerMidName
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerMidName, "Отчество", 32);
            }
        }

        #endregion

        #region DirectoryWorkerGender

        private bool _isDirectoryWorkerGenderMale;
        public bool IsDirectoryWorkerMale
        {
            get 
            {
                return _isDirectoryWorkerGenderMale;
            }
            set
            {
                IsDirectoryWorkerMale = value;
            }
        }
        
        #endregion

        #region DirectoryWorkerBirthDate

        private DateTime _selectedDirectoryWorkerBirthDate;
        public DateTime SelectedDirectoryWorkerBirthDate
        {
            get
            {
                return _selectedDirectoryWorkerBirthDate;
            }
            set
            {
                _selectedDirectoryWorkerBirthDate = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region DirectoryWorkerAddrres

        private string _directoryWorkerAddrres;
        public string DirectoryWorkerAddrres
        {
            get
            {
                return _directoryWorkerAddrres;
            }
            set
            {
                _directoryWorkerAddrres = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerAddrres");
            }
        }

        public string ValidateDirectoryWorkerAddrres
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerAddrres, "Адрес", 256);
            }
        }

        #endregion

        #region DirectoryWorkerMobilePhone

        private string _directoryWorkerMobilePhone;
        public string DirectoryWorkerMobilePhone
        {
            get
            {
                return _directoryWorkerMobilePhone;
            }
            set
            {
                _directoryWorkerMobilePhone = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerMobilePhone");
            }
        }

        public string ValidateDirectoryWorkerMobilePhone
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerMobilePhone, "Мобильный телефон", 16);
            }
        }

        #endregion

        #region DirectoryWorkerHomePhone

        private string _directoryWorkerHomePhone;
        public string DirectoryWorkerHomePhone
        {
            get
            {
                return _directoryWorkerHomePhone;
            }
            set
            {
                _directoryWorkerHomePhone = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryWorkerHomePhone");
            }
        }

        public string ValidateDirectoryWorkerHomePhone
        {
            get
            {
                return Validations.ValidateText(DirectoryWorkerHomePhone, "Домашний телефон", 16);
            }
        }

        #endregion

        #region DirectoryWorkerSartDate

        private DateTime _selectedDirectoryWorkerStartDate;
        public DateTime SelectedDirectoryWorkerStartDate
        {
            get
            {
                return _selectedDirectoryWorkerStartDate;
            }
            set
            {
                _selectedDirectoryWorkerStartDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "DirectoryWorkerLastName":
                    return ValidateDirectoryWorkerLastName;
                
                case "DirectoryWorkerFirstName":
                    return ValidateDirectoryWorkerFirstName;
               
                case "DirectoryWorkerMidName":
                    return ValidateDirectoryWorkerMidName;
               
                case "DirectoryWorkerAddrres":
                    return ValidateDirectoryWorkerAddrres;
                
                case "DirectoryWorkerMobilePhone":
                    return ValidateDirectoryWorkerMobilePhone;
               
                case "DirectoryWorkerHomePhone":
                    return ValidateDirectoryWorkerHomePhone;
                
                //case "DirectoryWorkerMidName":
                //    return ValidateDirectoryWorkerMidName;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
