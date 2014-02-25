using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Helpers
{
    public class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            //var validationContext = new ValidationContext(this)
            //{
            //    MemberName = propertyName
            //};

            //var results = new Collection<ValidationResult>();
            //bool isValid = Validator.TryValidateObject(this, validationContext, results, true);

            //return !isValid ? results[0].ErrorMessage : null;
            return null;
        }
    }
}
