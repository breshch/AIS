using AIS_Enterprise_AV.ViewModels.Infos.Base;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_AV.ViewModels.Infos
{
    public class AddContainerCarPartViewModel : BaseContainerCarPartViewModel
    {
        #region Base

        public event Action<CurrentContainerCarPart> AddingCarPart = delegate { }; 

        private CurrentContainerCarPart _currentContainerCarPart;

        public AddContainerCarPartViewModel() : base()
        {
            AddEditCarPartCommand = new RelayCommand(AddCarPart);
        }

        private void ClearForm()
        {
            SelectedCarPart = null;
            CountCarParts = 0;
        }

        #endregion

        #region Commands

        private void AddCarPart(object parameter)
        {
            _currentContainerCarPart = new CurrentContainerCarPart
            {
                DirectoryCarPartId = SelectedCarPart.Id,
                CountCarParts = CountCarParts
            };

            AddingCarPart(_currentContainerCarPart);

            ClearForm();
        }

        #endregion
    }
}
