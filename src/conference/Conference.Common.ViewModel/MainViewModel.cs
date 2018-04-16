namespace Conference.Common.ViewModel
{
    using System;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;

    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class MainViewModel : MultiPageViewModel<object>
    {
        private readonly IEventTracker eventTrackerService;

        private bool initialized;

        public MainViewModel(
            INavigationService navigationService,
            IConferenceRepository conferenceRepository,
            ITwitterDataService twitterDataService,
            IEventTracker eventTracker,
            IContentDialogService contentDialogService,
            IWebLauncher webLauncher,
            IEventTracker eventTrackerService)
            : base("MainView", navigationService)
        {
            this.ConferenceRepository = conferenceRepository;

            this.TwitterDataService = twitterDataService;

            this.ContentDialogService = contentDialogService;

            this.EventTracker = eventTracker;

            this.WebLauncher = webLauncher;

            this.eventTrackerService = eventTrackerService;
        }

        public IWebLauncher WebLauncher { get; }

        public IContentDialogService ContentDialogService { get; }

        public ITwitterDataService TwitterDataService { get; }

        public ConferenceData ConferenceData { get; set; }

        public IConferenceRepository ConferenceRepository { get; }

        public IEventTracker EventTracker { get; }

        protected override async void OnNavigatedTo(object parameter)
        {
            base.OnNavigatedTo(parameter);

            if (!this.initialized)
            {
                await this.Initialize();
            }
        }

        private async Task Initialize()
        {           
            this.PageItems.Add(new HomeTabViewModel(this));
            this.PageItems.Add(new SessionsTabViewModel(this));
            this.PageItems.Add(new SpeakersTabViewModel(this));
            this.PageItems.Add(new TwitterTabViewModel(this));
            this.PageItems.Add(new InfoTabViewModel(this));

            this.CurrentPage = this.PageItems[0];

            this.IsLoading = true;
            var result = await this.ConferenceRepository.GetConferenceData();
            this.IsLoading = false;

            if (result.Successful)
            {
                this.ConferenceData = result.Value;

                try
                {
                    this.eventTrackerService.TrackEvent("[Started]");
                }
                catch (Exception)
                {
                    // ignored
                }

                this.initialized = true;
            }
            else
            {
                await this.ContentDialogService.Alert("Error", result.ErrorMessage);
            }
        }
    }
}
