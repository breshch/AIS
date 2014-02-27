using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Helpers
{
    public static class Validations 
    {
        public static string ValidateText(string text, string labelName, int maxLength = -1)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Format("Не заполнено поле \"{0}\"", labelName);
            }

            if (maxLength != -1 && text.Length > maxLength)
            {
                 return string.Format("Длина поля \"{0}\" должна быть не больше {1} символов", labelName, maxLength);
            }
            return null;
        }

        public static string ValidateObject(object obj, string labelName)
        {
            if (obj == null)
            {
                return string.Format("Не выбрано значение в списке \"{0}\"", labelName) ;
            }
            return null;
        }

        public static string ValidateDoubleMoreAndEqualZero(string number, string labelName)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Format("Не заполнено поле \"{0}\"", labelName); 
            }

            number = number.Replace(".", ",");
            double validateNumber;
            if (!double.TryParse(number,out validateNumber))
            {
                return "Вводите только числовое значение"; 
            }

            if (validateNumber < 0)
            {
                return "Введите числовое значение, большее, либо равное 0";
            }
            return null;
        }

        public static string ValidateDoubleMoreZero(string number, string labelName)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Format("Не заполнено поле \"{0}\"", labelName);
            }
            
            number = number.Replace(".", ",");
            double validateNumber;
            if (!double.TryParse(number, out validateNumber))
            {
                return "Вводите только числовое значение";
            }

            if (validateNumber < 0)
            {
                return "Введите числовое значение, большее 0";
            }
            return null;
        }
    }
}
