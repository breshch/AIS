using AIS_Enterprise_Global.Helpers;
using AIS_Enterprise_Data.Currents;
using AIS_Enterprise_Data.Directories;
using AIS_Enterprise_Global.ViewModels.Directories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIS_Enterprise_Global.ViewModels.Directories
{
    public class DirectoryEditUserStatusViewModel : DirectoryUserStatusBaseViewModel
    {

        #region Base
        private int _userStatusId;

        public DirectoryEditUserStatusViewModel(DirectoryUserStatus userStatus) : base()
        {
            _userStatusId = userStatus.Id;
            EditCommand = new RelayCommand(Edit);

            UserStatusName = userStatus.Name;

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

                    var mainParent = GroupPrivileges.First(p => p.Name == parents[0]);

                    var subParent = mainParent;
                    for (int j = 1; j <= i; j++)
                    {
                        subParent = subParent.Children.First(c => c.Name == parents[j]);

                        if (!subParent.Children.Any())
                        {
                            string privilegeName = "";
                            for (int k = 0; k < parents.Count; k++)
			                {
                                if (k != parents.Count - 1)
                                {
                                    privilegeName += parents[k] + "_";
                                }
                                else
                                {
                                    privilegeName += parents[k];
                                }
			                }

                            if (userStatus.Privileges.Select(p => p.DirectoryUserStatusPrivilege.Name).Contains(privilegeName))
                            {
                                subParent.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region Commands

        public RelayCommand EditCommand { get; set; }

        private void Edit(object parameter)
        {
            var privileges = new List<CurrentUserStatusPrivilege>();

            foreach (var mainParent in GroupPrivileges)
            {
                string privilageName = "";
                AddPrivilege(mainParent, privilageName, privileges);
            }

            BC.EditDirectoryUserStatus(_userStatusId, UserStatusName, privileges);

            var window = (Window)parameter;
            window.Close();
        }

        #endregion
    }
}
