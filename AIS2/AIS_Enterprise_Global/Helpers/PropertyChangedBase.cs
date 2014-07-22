using AIS_Enterprise_Global.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Global.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class MagicAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NoMagicAttribute : Attribute { }
    
    [Magic]
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static void Raise() { }

        protected virtual void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
