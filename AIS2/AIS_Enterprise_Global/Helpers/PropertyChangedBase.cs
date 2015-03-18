using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIS_Enterprise_Global.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class MagicAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class NoMagicAttribute : Attribute { }
    
    [Magic]
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
