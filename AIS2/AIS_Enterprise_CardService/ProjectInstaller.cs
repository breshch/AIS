using System.ComponentModel;
using System.Configuration.Install;

namespace AIS_Enterprise_CardService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
