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

        public static string ValidateObj(object obj)
        {
            if (obj == null)
            {
                return "error";
            }

            return null;
        }
    }
}
