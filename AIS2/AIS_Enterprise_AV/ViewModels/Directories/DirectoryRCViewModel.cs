using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data;
using AIS_Enterprise_Data.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AIS_Enterprise_Global.Helpers.Attributes;

namespace AIS_Enterprise_AV.ViewModels
{
    public class DirectoryRCViewModel : ViewModelGlobal
    {
        #region Base
        private const int MAXIMUM_PERCENTAGE = 100;
       
        public DirectoryRCViewModel() : base()
        {
            RefreshDirectoryRCs();

            AddCommand = new RelayCommand(Add, CanAdding);
            RemoveCommand = new RelayCommand(Remove, CanRemoving);
            
            MinimumPercentes = 0;
        }

        private void RefreshDirectoryRCs()
        {
            DirectoryRCs = new ObservableCollection<DirectoryRC>(BC.GetDirectoryRCs().ToList());
            MaximumPercentes = MAXIMUM_PERCENTAGE - DirectoryRCs.Sum(r => r.Percentes);
        }

        private void ClearInputData()
        {
            DirectoryRCName = null;
            Percentes = 0;
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryRC> DirectoryRCs { get; set; }

        public DirectoryRC SelectedDirectoryRC { get; set; }

        [Required]
        [Display(Name = "Название ЦО")]
        public string DirectoryRCName { get; set; }

        public int Percentes { get; set; }

        public int MinimumPercentes { get; set; }
        public int MaximumPercentes { get; set; }

        [Required]
        [Display(Name = "Компания")]
        public string DescriptionName { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public void Add(object parameter)
        {
            BC.AddDirectoryRC(DirectoryRCName, DescriptionName, Percentes);

            RefreshDirectoryRCs();

            ClearInputData();
        }

        public bool CanAdding(object parameter)
        {
            return IsValidateAllProperties();
        }

        public void Remove(object parameter)
        {
            BC.RemoveDirectoryRC(SelectedDirectoryRC.Id);

            RefreshDirectoryRCs();
            
            if (DirectoryRCs.Any())
            {
                SelectedDirectoryRC = DirectoryRCs.Last();
            }
        }

        public bool CanRemoving(object parameter)
        {
            return SelectedDirectoryRC != null;
        }

        #endregion
    }
}
