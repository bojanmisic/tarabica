namespace Conference.Common.ViewModel
{
    using System.Collections.ObjectModel;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class FavoritesViewModel : PageViewModel<ObservableCollection<SessionGroupViewModel>>
    {
        private ObservableCollection<SessionGroupViewModel> favoriteSessionGroups;

        private IMvvmCommand goBackCommand;

        private IMvvmCommand sessionSelectedCommand;

        private IMvvmCommand removeFavoriteCommand;

        public FavoritesViewModel(INavigationService navigationService)
            : base("FavoritesView", navigationService)
        {
        }

        public IMvvmCommand GoBackCommand => this.goBackCommand ?? (this.goBackCommand = new ActionCommand(
            () =>
            {
                this.NavigationService.GoBack();
            }));

        public IMvvmCommand SessionSelectedCommand => this.sessionSelectedCommand ?? (this.sessionSelectedCommand = new ActionCommand<SessionItemViewModel>(
            (session) =>
            {
                this.NavigationService.NavigateTo("SessionView", session);
            }));

        public IMvvmCommand RemoveFavoriteCommand => this.removeFavoriteCommand ?? (this.removeFavoriteCommand = new ActionCommand<SessionItemViewModel>(
            (session) =>
            {
                session.IsFavorite = !session.IsFavorite;

                var newCollection = new ObservableCollection<SessionGroupViewModel>();

                foreach (var group in this.FavoriteSessionGroups)
                {
                    group.Sessions.Remove(session);

                    if (group.Sessions.Count > 0)
                    {
                        newCollection.Add(group);
                    }
                }

                this.FavoriteSessionGroups = newCollection;

                if (this.FavoriteSessionGroups.Count == 0)
                {
                    this.NavigationService.GoBack();
                }
            }));

        public ObservableCollection<SessionGroupViewModel> FavoriteSessionGroups
        {
            get
            {
                return this.favoriteSessionGroups;
            }

            set
            {
                this.Set(ref this.favoriteSessionGroups, value);
            }
        }

        protected override void OnNavigatedForwardTo(ObservableCollection<SessionGroupViewModel> favoriteSessionGroups)
        {
            base.OnNavigatedForwardTo(this.favoriteSessionGroups);

            this.FavoriteSessionGroups = favoriteSessionGroups;
        }
    }
}