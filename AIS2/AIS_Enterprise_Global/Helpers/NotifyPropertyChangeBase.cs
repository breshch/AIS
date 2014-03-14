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
    [Notify]
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
