using AIS_Enterprise_Global.Helpers.Attributes;
using AIS_Enterprise_Data;
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

namespace AIS_Enterprise_Global.Helpers
{
    public class ViewModelGlobal : ViewModelBase
    {
        protected BusinessContext BC = new BusinessContext();

        public ViewModelGlobal()
        {
            ViewCloseCommand = new RelayCommand(ViewClose);
        }

        public RelayCommand ViewCloseCommand { get; set; }

        public virtual void ViewClose(object parameter)
        {
            BC.Dispose();
        }
    }
}
