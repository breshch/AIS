using AIS_Enterprise.Helpers;
using AIS_Enterprise.Models;
using AIS_Enterprise.Models.Directories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AIS_Enterprise.ViewModels
{
    public class DirectoryPostViewModel : ViewModel
    {
        private BusinessContext _bc = new BusinessContext();

        public DirectoryPostViewModel()
        {
            RefreshDirectoryPosts();
        }

        private void RefreshDirectoryPosts()
        {
            DirectoryPosts = new ObservableCollection<DirectoryPost>(_bc.GetDirectoryPosts());
        }

        private ObservableCollection<DirectoryPost> _directoryPosts;

        public ObservableCollection<DirectoryPost> DirectoryPosts
        {
            get
            {
                return _directoryPosts;
            }
            set
            {
                _directoryPosts = value;
                OnPropertyChanged();
            }
        }

        private string _directoryPostName;

        public string DirectoryPostName
        {
            get 
            {
                return _directoryPostName;
            }
            set
            {
                _directoryPostName = value;
                OnPropertyChanged();
                OnPropertyChanged("ValidateDirectoryPostName");
            }
        }

        public string ValidateDirectoryPostName
        {
            get
            {
                return Validations.ValidateText(DirectoryPostName, "Должность", 32);
            }
        }

        protected override string OnValidate(string propertyName)
        {
            switch (propertyName)
            {
                case "DirectoryPostName":
                    return ValidateDirectoryPostName;
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}
