using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Global.Views.Currents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS_Enterprise_Data.Temps;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryAddWorkerViewModel : DirectoryWorkerBaseViewModel
    {
        #region Base

        public DirectoryAddWorkerViewModel()
            : base()
        {
            DirectoryWorkerGender = Gender.Male;

            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>();
            
            AddWorkerCommand = new RelayCommand(AddWorker, CanAddingWorker);
            AddPhotoName = "Добавить фото";

            SelectedDirectoryWorkerStartDate = DateTime.Now;
            SelectedDirectoryWorkerBirthDay = DateTime.Now;
        }

        #endregion


        #region Commands

        public RelayCommand AddWorkerCommand { get; set; }

        private void AddWorker(object parameter)
        {
            BC.AddDirectoryWorker(DirectoryWorkerLastName, DirectoryWorkerFirstName, DirectoryWorkerMidName, DirectoryWorkerGender, SelectedDirectoryWorkerBirthDay, DirectoryWorkerAddress,
                DirectoryWorkerHomePhone, DirectoryWorkerCellPhone, SelectedDirectoryWorkerStartDate, Photo, null, CurrentCompaniesAndPosts, IsDeadSpirit);

            ClearInputData();
        }

        private bool CanAddingWorker(object parameter)
        {
            return IsValidateAllProperties() && CurrentCompaniesAndPosts.Any();
        }

        #endregion
    }
}
