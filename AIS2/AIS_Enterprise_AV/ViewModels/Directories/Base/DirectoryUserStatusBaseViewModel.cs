using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Global.Helpers.Temps;

namespace AIS_Enterprise_Global.ViewModels.Directories.Base
{
    public class DirectoryUserStatusBaseViewModel : ViewModelGlobal
    {
        #region Base

        public DirectoryUserStatusBaseViewModel()
        {
            InitializePrivileges();
        }

        private void InitializePrivileges()
        {
            GroupPrivileges = new ObservableCollection<PrivilegeTempViewModel>();

            foreach (var privilege in Enum.GetNames(typeof(UserPrivileges)))
            {
                int indexBeginingParent = 0;
                int countParents = privilege.Count(c => c == '_');

                var parents = new List<string>();
                for (int i = 0; i < countParents + 1; i++)
                {
                    string parent;
                    if (privilege.IndexOf("_", indexBeginingParent) != -1)
                    {
                        parent = privilege.Substring(indexBeginingParent, privilege.IndexOf("_", indexBeginingParent) - indexBeginingParent);
                    }
                    else
                    {
                        parent = privilege.Substring(indexBeginingParent);
                    }

                    parents.Add(parent);
                    indexBeginingParent = privilege.IndexOf("_", indexBeginingParent) + 1;

                    var mainParent = GroupPrivileges.FirstOrDefault(p => p.Name == parents[0]);
                    if (mainParent == null)
                    {
                        mainParent = new PrivilegeTempViewModel(parents[0]);
                        GroupPrivileges.Add(mainParent);
                        mainParent.Initialize();
                    }

                    var subParent = mainParent;
                    for (int j = 1; j <= i; j++)
                    {
                        var child = subParent.Children.FirstOrDefault(c => c.Name == parents[j]);
                        if (child == null)
                        {
                            child = new PrivilegeTempViewModel(parents[j]);
                            subParent.Children.Add(child);
                        }

                        subParent.Initialize();
                        subParent = subParent.Children.First(c => c.Name == parents[j]);
                    }
                }
            }
        }

        protected void AddPrivilege(PrivilegeTempViewModel paretnNode, string privilegeName, List<CurrentUserStatusPrivilege> privileges)
        {
            if (paretnNode.Children.Any())
            {
                privilegeName += paretnNode.Name + "_";
            }
            else
            {
                privilegeName += paretnNode.Name;

                if (paretnNode.IsChecked == true)
                {
                    var directoryPrivilege = BC.GetDirectoryUserStatusPrivilege(privilegeName);
                    privileges.Add(new CurrentUserStatusPrivilege { DirectoryUserStatusPrivilege = directoryPrivilege });
                }
            }

            foreach (var child in paretnNode.Children)
            {
                AddPrivilege(child, privilegeName, privileges);

                if (!child.Children.Any())
                {
                    privilegeName = privilegeName.Substring(0, privilegeName.LastIndexOf("_") + 1);

                }
            }
        }


        #endregion

        #region Properties

        public string UserStatusName { get; set; }
        public ObservableCollection<PrivilegeTempViewModel> GroupPrivileges { get; set; }


        #endregion

    }
}
