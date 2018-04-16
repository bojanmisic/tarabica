namespace Conference.Common.ViewModel
{
    using System.Collections.ObjectModel;

    public class SessionGroupViewModel
    {
        public SessionGroupViewModel(string groupName)
        {
            this.GroupName = groupName.ToUpper();
            this.Sessions = new ObservableCollection<SessionItemViewModel>();
        }

        public string GroupName { get; }

        public ObservableCollection<SessionItemViewModel> Sessions { get; }
    }
}

