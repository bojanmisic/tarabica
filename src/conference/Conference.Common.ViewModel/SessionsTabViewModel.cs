namespace Conference.Common.ViewModel
{
    using System.Collections.ObjectModel;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;

    public class SessionsTabViewModel : PageItemViewModel<object>
    {
        private bool initialized;

        private ObservableCollection<SessionGroupViewModel> sessionGroups;

        private IMvvmCommand sessionSelectedCommand;

        private IMvvmCommand favoritesCommand;

        public SessionsTabViewModel(MultiPageViewModel<object> parentMultiPage)
            : base(parentMultiPage)
        {
            this.Title = "Agenda";
            this.Icon = "ticon-agenda";
        }

        public IMvvmCommand SessionSelectedCommand => this.sessionSelectedCommand ?? (this.sessionSelectedCommand = new ActionCommand<SessionItemViewModel>(
            (session) =>
            {
                this.ParentMultiPage.NavigationService.NavigateTo("SessionView", session);
            }));

        public IMvvmCommand FavoritesCommand => this.favoritesCommand ?? (this.favoritesCommand = new ActionCommand(
            () =>
            {
                var mainViewModel = this.ParentMultiPage as MainViewModel;

                if (mainViewModel == null)
                {
                    return;
                }

                this.ParentMultiPage.NavigationService.NavigateTo("FavoritesView", ConferenceHelpers.GroupSessions(mainViewModel.ConferenceRepository, mainViewModel.ConferenceData, true));
            }));

        public ObservableCollection<SessionGroupViewModel> SessionGroups
        {
            get
            {
                return this.sessionGroups;
            }

            set
            {
                this.Set(ref this.sessionGroups, value);
            }
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            this.ParentMultiPage.IsLoading = true;
            var mainViewModel = this.ParentMultiPage as MainViewModel;

            if (mainViewModel == null)
            {
                return;
            }

            if (!this.initialized && mainViewModel.ConferenceData != null)
            {
                this.SessionGroups = ConferenceHelpers.GroupSessions(mainViewModel.ConferenceRepository, mainViewModel.ConferenceData);
                
                this.initialized = true;
            }
            this.ParentMultiPage.IsLoading = false;
        }       
    }
}
