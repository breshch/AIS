using System.ComponentModel.DataAnnotations;

namespace AIS_Enterprise_Global.Helpers.Attributes
{
    public class DoubleValueAttribute : ValidationAttribute
    {
        public double MinValue;
        public double MaxValue;
    }
}
