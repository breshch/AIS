using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise.Helpers.Attributes
{
    public class DoubleValueAttribute : ValidationAttribute
    {
        public double MinValue;
        public double MaxValue;
    }
}
