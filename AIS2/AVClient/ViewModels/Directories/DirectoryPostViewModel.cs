using System.Collections.ObjectModel;
using AVClient.AVServiceReference;
using AVClient.Helpers;
using AVClient.Views.Directories;

namespace AVClient.ViewModels.Directories
{
    public class DirectoryPostViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryPostViewModel()
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
