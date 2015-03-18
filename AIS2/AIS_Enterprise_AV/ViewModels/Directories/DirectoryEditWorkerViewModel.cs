using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Data.Temps;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.Views.Directories;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryEditWorkerViewModel : DirectoryWorkerBaseViewModel
    {
        #region Base

        private DirectoryWorker _selectedDirectoryWorker;

        public DirectoryEditWorkerViewModel(int workerId)
            : base()
        {
            _selectedDirectoryWorker = BC.GetDirectoryWorker(workerId);

            IsFireWorkerEnable = HelperMethods.IsPrivilege(BC, UserPrivileges.Workers_FireWorkers);

            if (!IsNotFireDate)
            {
                VisibilityBringAlive = Visibility.Visible;
                IsFireWorkerEnable = false;
            }
            else
            {
                VisibilityBringAlive = Visibility.Collapsed;
            }

            EditWorkerCommand = new RelayCommand(EditWorker, CanEditingWorker);
            FireWorkerCommand = new RelayCommand(FireWorker, CanEditingWorker);
            BringAliveCommand = new RelayCommand(BringAlive);

            FillInputData();
        }

        private void FillInputData()
        {
            DirectoryWorkerLastName = _selectedDirectoryWorker.LastName;
            DirectoryWorkerFirstName = _selectedDirectoryWorker.FirstName;
            DirectoryWorkerMidName = _selectedDirectoryWorker.MidName;
            DirectoryWorkerGender = _selectedDirectoryWorker.Gender;
            SelectedDirectoryWorkerBirthDay = _selectedDirectoryWorker.BirthDay;
            DirectoryWorkerAddress = _selectedDirectoryWorker.Address;
            DirectoryWorkerCellPhone = _selectedDirectoryWorker.CellPhone;
            DirectoryWorkerHomePhone = _selectedDirectoryWorker.HomePhone;
            SelectedDirectoryWorkerStartDate = _selectedDirectoryWorker.StartDate;

            if (_selectedDirectoryWorker.DirectoryPhoto != null && _selectedDirectoryWorker.DirectoryPhoto.Photo != null && _selectedDirectoryWorker.DirectoryPhoto.Photo.Length != 0)
            {
                using (var mem = new MemoryStream(_selectedDirectoryWorker.DirectoryPhoto.Photo))
                {
                    mem.Position = 0;

                    Photo = new BitmapImage();
                    Photo.BeginInit();
                    Photo.CacheOption = BitmapCacheOption.OnLoad;
                    Photo.StreamSource = mem;
                    Photo.EndInit();
                }
                AddPhotoName = "Изменить фото";
            }
            else
            {
                AddPhotoName = "Добавить фото";
            }

            CurrentCompaniesAndPosts = new ObservableCollection<CurrentCompanyAndPost>(_selectedDirectoryWorker.CurrentCompaniesAndPosts.
                Select(c => new CurrentCompanyAndPost { DirectoryPost = c.DirectoryPost, PostChangeDate = c.ChangeDate, PostFireDate = c.FireDate, IsTwoCompanies = c.IsTwoCompanies,
                Salary = IsAdminSalary ? c.DirectoryPost.DirectoryPostSalaries.OrderByDescending(s => s.Date).First().AdminWorkerSalary.Value : 
                c.DirectoryPost.DirectoryPostSalaries.OrderByDescending(s => s.Date).First().UserWorkerSalary}));

            IsDeadSpirit = _selectedDirectoryWorker.IsDeadSpirit;
        }

        #endregion


        #region Properties

        [NoMagic]
        public DateTime? SelectedDirectoryWorkerFireDate { get; set; }

        private bool _isNotFireDate;

        [NoMagic]
        public bool IsNotFireDate
        {
            get
            {
                return _selectedDirectoryWorker.FireDate == null || _isNotFireDate;
            }
            set
            {
                _isNotFireDate = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFireWorkerEnable { get; set; }

        public Visibility VisibilityBringAlive { get; set; }

        [NoMagic]
        public bool IsChangeWorker { get; private set; }

        #endregion


        #region Commands

        public RelayCommand EditWorkerCommand { get; set; }
        public RelayCommand FireWorkerCommand { get; set; }
        public RelayCommand BringAliveCommand { get; set; }

        private void EditWorker(object parameter)
        {
            IsChangeWorker = true;

            BC.EditDirectoryWorker(_selectedDirectoryWorker.Id, DirectoryWorkerLastName, DirectoryWorkerFirstName, DirectoryWorkerMidName, DirectoryWorkerGender, SelectedDirectoryWorkerBirthDay, DirectoryWorkerAddress,
                DirectoryWorkerHomePhone, DirectoryWorkerCellPhone, SelectedDirectoryWorkerStartDate, Photo, SelectedDirectoryWorkerFireDate, CurrentCompaniesAndPosts, IsDeadSpirit);

            var window = (Window)parameter;

            if (window != null)
            {
                window.Close();
            }
        }

        private bool CanEditingWorker(object parameter)
        {
            return IsValidateAllProperties() && CurrentCompaniesAndPosts.Any();
        }

        private void FireWorker(object parameter)
        {
            var directoryWorkerFireDateViewModel = new DirectoryWorkerFireDateViewModel(_selectedDirectoryWorker.Id);
            var directoryWorkerFireDateView = new DirectoryWorkerFireDateView();

            directoryWorkerFireDateView.DataContext = directoryWorkerFireDateViewModel;
            directoryWorkerFireDateView.ShowDialog();

            SelectedDirectoryWorkerFireDate = directoryWorkerFireDateViewModel.SelectedDirectoryWorkerFireDate;

            if (SelectedDirectoryWorkerFireDate != null)
            {
                EditWorker(parameter);
            }
        }

        public override void ViewClose(object parameter)
        {
            base.ViewClose(parameter);
        }

        private void BringAlive(object parameter)
        {
            IsNotFireDate = !IsNotFireDate;
        }

        #endregion
    }
}
