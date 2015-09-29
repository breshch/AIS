using System.ComponentModel;
using System.Configuration.Install;

namespace AIS_Enterprise_Gas
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
