using System.ComponentModel.DataAnnotations;

namespace AVClient.Helpers.Attributes
{
    public class DoubleValueAttribute : ValidationAttribute
    {
        public double MinValue;
        public double MaxValue;
    }
}
