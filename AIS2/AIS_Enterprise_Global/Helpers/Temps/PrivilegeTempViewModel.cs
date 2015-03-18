using System.Collections.Generic;

namespace AIS_Enterprise_Global.Helpers.Temps
{
    public class PrivilegeTempViewModel : PropertyChangedBase
    {
        public PrivilegeTempViewModel(string name)
        {
            Name = name;
            Children = new List<PrivilegeTempViewModel>();
        }

        #region Properties

        public string Name { get; private set; }
        public List<PrivilegeTempViewModel> Children { get; private set; }
        public bool IsInitiallySelected { get; private set; }

        bool? _isChecked = false;
        PrivilegeTempViewModel _parent;

        #region IsChecked

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null) _parent.VerifyCheckedState();

            RaisePropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion

        #endregion

        public void Initialize()
        {
            foreach (PrivilegeTempViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }
    }
}
