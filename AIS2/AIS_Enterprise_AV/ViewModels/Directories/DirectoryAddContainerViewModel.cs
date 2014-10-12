using AIS_Enterprise_AV.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Directories
{
    public class DirectoryAddContainerViewModel : DirectoryContainerBaseViewModel
    {
        #region Base

        public DirectoryAddContainerViewModel()
            : base()
        {
            TitleContainerName = "Добавление контейнера";
            ButtonAddEditContainerName = "Добавить контейнер";
            Date = DateTime.Now;
        }

        #endregion

        #region Commands

        

        #endregion
    }
}
