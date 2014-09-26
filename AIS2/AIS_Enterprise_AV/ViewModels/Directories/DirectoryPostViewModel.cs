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
using AIS_Enterprise_Global.ViewModels.Directories;
using AIS_Enterprise_Global.Views.Directories;

namespace AIS_Enterprise_Global.ViewModels
{
    public class DirectoryPostViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostViewModel() : base()
        {
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, IsSelected);
            RemoveCommand = new RelayCommand(Remove, IsSelected);

            RefreshDirectoryPosts();
        }

        private void RefreshDirectoryPosts()
        {
            DirectoryPosts = new ObservableCollection<DirectoryPost>(BC.GetDirectoryPosts());
        }

        #endregion


        #region Properties

        public ObservableCollection<DirectoryPost> DirectoryPosts { get; set; }

        public DirectoryPost SelectedDirectoryPost { get; set; }

        #endregion


        #region Commands

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private void Add(object parameter)
        {
            HelperMethods.ShowView(new DirectoryAddPostViewModel(), new AddEditDirectoryPostView());

            RefreshDirectoryPosts();
        }

        private void Edit(object parameter)
        {
            HelperMethods.ShowView(new DirectoryEditPostViewModel(SelectedDirectoryPost), new AddEditDirectoryPostView());

            BC.RefreshContext();
            RefreshDirectoryPosts();
        }

        private void Remove(object parameter)
        {
            BC.RemoveDirectoryPost(SelectedDirectoryPost);
            RefreshDirectoryPosts();
        }

        private bool IsSelected(object parameter)
        {
            return SelectedDirectoryPost != null;
        }

        #endregion
    }
}
