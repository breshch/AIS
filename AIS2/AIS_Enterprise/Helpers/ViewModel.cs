using AIS_Enterprise.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Helpers
{
    public class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        protected BusinessContext BC = new BusinessContext();

        public ViewModel()
        {
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        public RelayCommand ViewCloseCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ViewClose(object parameter)
        {
            Debug.WriteLine("dispose view");
            BC.Dispose();
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        private string ValidateAttrbutes(IEnumerable<CustomAttributeData> attributes, object propertyValue, string displayName = "")
        {
            foreach (var attribute in attributes)
            {
                switch (attribute.AttributeType.Name)
                {
                    case "RequiredAttribute":
                        if (propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.ToString()))
                        {
                            return string.Format("Не заполнено поле \"{0}\"", displayName);
                        }
                        break;
                    default:
                        break;
                }
            }

            return null;
        }

        protected virtual string OnValidate(string propertyName)
        {
            var property = this.GetType().GetProperty(propertyName);

            var propertyValue = property.GetValue(this);

            string displayName = "";

            var attributeDisplay = property.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(DisplayAttribute));
            if (attributeDisplay != null)
            {
                var attributeDisplayName = attributeDisplay.NamedArguments.FirstOrDefault(a => a.MemberName == "Name");
                if (attributeDisplayName != null)
                {
                    displayName = attributeDisplayName.TypedValue.Value.ToString();
                }
            }
            else
            {
                Debug.WriteLine("У " + property.Name + " отсутствует атрибут Display");
            }

            return ValidateAttrbutes(property.CustomAttributes, propertyValue, displayName);
        }

        protected bool IsValidateAllProperties()
        {
            var properties = this.GetType().GetProperties().Where(p => p.CustomAttributes.Any());

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(this);

                if (ValidateAttrbutes(property.CustomAttributes, propertyValue) != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
