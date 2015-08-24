using System;
using System.Collections.ObjectModel;
using System.Linq;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.ViewModels.Directories.Base;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryAddWorkerViewModel : DirectoryWorkerBaseViewModel
    {
        #region Base

        public DirectoryAddWorkerViewModel()
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
            BC.AddDirectoryWorkerWithMultiplyPosts(DirectoryWorkerLastName, DirectoryWorkerFirstName, DirectoryWorkerMidName, DirectoryWorkerGender, SelectedDirectoryWorkerBirthDay, DirectoryWorkerAddress,
                DirectoryWorkerHomePhone, DirectoryWorkerCellPhone, SelectedDirectoryWorkerStartDate, Photo, null, CurrentCompaniesAndPosts.ToArray(), IsDeadSpirit);

            ClearInputData();
        }

        private bool CanAddingWorker(object parameter)
        {
            return IsValidateAllProperties() && CurrentCompaniesAndPosts.Any();
        }

        #endregion
    }
}
