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

            foreach (var attribute in property.CustomAttributes)
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
    }
}
