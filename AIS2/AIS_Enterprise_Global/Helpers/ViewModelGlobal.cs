using AIS_Enterprise_Data;

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
