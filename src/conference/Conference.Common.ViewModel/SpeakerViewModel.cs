namespace Conference.Common.ViewModel
{
    using System;
    using System.Collections.Generic;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class SpeakerViewModel : PageViewModel<SpeakerItemViewModel>
    {
        private readonly IEventTracker eventTrackerService;

        private SpeakerItemViewModel speaker;

        private string bio;

        private string imageUrl;

        private string name;

        private string company;

        private List<SessionItemViewModel> sessions;

        private IMvvmCommand goBackCommand;

        private IMvvmCommand sessionSelectedCommand;

        public SpeakerViewModel(INavigationService navigationService, IEventTracker eventTrackerService)
            : base("SpeakerView", navigationService)
        {
            this.eventTrackerService = eventTrackerService;
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

        public string Bio
        {
            get
            {
                return this.bio;
            }

            set
            {
                this.Set(ref this.bio, value);
            }
        }

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }

            set
            {
                this.Set(ref this.imageUrl, value);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.Set(ref this.name, value);
            }
        }

        public string Company
        {
            get
            {
                return this.company;
            }

            set
            {
                this.Set(ref this.company, value);
            }
        }

        public List<SessionItemViewModel> Sessions
        {
            get
            {
                return this.sessions;
            }

            set
            {
                this.Set(ref this.sessions, value);
            }
        }

        protected override void OnNavigatedForwardTo(SpeakerItemViewModel speaker)
        {
            base.OnNavigatedForwardTo(speaker);

            this.speaker = speaker;

            this.Bio = speaker.Bio;
            this.Company = speaker.Company;
            this.Name = speaker.Name;
            this.ImageUrl = speaker.ImageUrl;
            this.Sessions = speaker.Sessions;

            try
            {
                this.eventTrackerService.TrackEvent($"[Speaker] {this.speaker.GetModel().Id}");
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
