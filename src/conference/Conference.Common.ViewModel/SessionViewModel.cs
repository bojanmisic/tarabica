namespace Conference.Common.ViewModel
{
    using System;
    using System.Collections.Generic;

    using Conference.Common.Contracts.Data;

    using OpenMVVM.Core;
    using OpenMVVM.Core.Pages;
    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Core.PlatformServices.Navigation;

    public class SessionViewModel : PageViewModel<SessionItemViewModel>
    {
        private readonly IConferenceRepository conferenceRepository;

        private readonly IReminderService reminderService;

        private readonly IContentDialogService contentDialogService;

        private readonly IEventTracker eventTrackerService;

        private SessionItemViewModel session;

        private string time;

        private List<SpeakerItemViewModel> speakers;

        private int level;

        private string track;

        private string room;

        private string description;

        private string title;

        private bool isFavorite;

        private IMvvmCommand goBackCommand;

        private IMvvmCommand speakerSelectedCommand;

        private IMvvmCommand toggleFavoriteCommand;

        private IMvvmCommand favoritesCommand;

        private IMvvmCommand addToCalendarCommand;

        public SessionViewModel(
            INavigationService navigationService,
            IConferenceRepository conferenceRepository,
            IReminderService reminderService,
            IContentDialogService contentDialogService,
            IEventTracker eventTrackerService)
            : base("SessionView", navigationService)
        {
            this.conferenceRepository = conferenceRepository;

            this.reminderService = reminderService;

            this.contentDialogService = contentDialogService;

            this.eventTrackerService = eventTrackerService;
        }

        public IMvvmCommand GoBackCommand => this.goBackCommand ?? (this.goBackCommand = new ActionCommand(
            () =>
            {
                this.NavigationService.GoBack();
            }));

        public IMvvmCommand SpeakerSelectedCommand => this.speakerSelectedCommand ?? (this.speakerSelectedCommand = new ActionCommand<SpeakerItemViewModel>(
            (speaker) =>
            {
                this.NavigationService.NavigateTo("SpeakerView", speaker);
            }));

        public IMvvmCommand ToggleFavoriteCommand => this.toggleFavoriteCommand ?? (this.toggleFavoriteCommand = new ActionCommand(
            () =>
            {
                this.IsFavorite = !this.IsFavorite;

                if (this.IsFavorite)
                {
                    try
                    {
                        this.eventTrackerService.TrackEvent($"[Favorite] {this.session.GetModel().Id}");
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }                                                                                                         
            }));

        public IMvvmCommand FavoritesCommand => this.favoritesCommand ?? (this.favoritesCommand = new ActionCommand(
            () =>
            {
                this.NavigationService.NavigateTo("FavoritesView", ConferenceHelpers.GroupSessions(this.conferenceRepository, this.session.GetConferenceData(), true));
            }));

        public IMvvmCommand AddToCalendarCommand => this.addToCalendarCommand ?? (this.addToCalendarCommand = new ActionCommand(this.AddToCalendar));

        public string Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.Set(ref this.time, value);
            }
        }

        public List<SpeakerItemViewModel> Speakers
        {
            get
            {
                return this.speakers;
            }

            set
            {
                this.Set(ref this.speakers, value);
            }
        }

        public string Room
        {
            get
            {
                return this.room;
            }

            set
            {
                this.Set(ref this.room, value);
            }
        }

        public string Track
        {
            get
            {
                return this.track;
            }

            set
            {
                this.Set(ref this.track, value);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.Set(ref this.description, value);
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }

            set
            {
                this.Set(ref this.level, value);
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.Set(ref this.title, value);
            }
        }

        public bool IsFavorite
        {
            get
            {
                return this.isFavorite;
            }

            set
            {
                this.Set(ref this.isFavorite, value);
                this.session.IsFavorite = value;
            } 
        }

        public string CalendarText => "Dodaj u kalendar";

        protected override void OnNavigatedForwardTo(SessionItemViewModel session)
        {
            base.OnNavigatedForwardTo(session);
            
            this.session = session;

            this.Time = $"{session.StartTime} - {session.EndTime}";
            this.Title = session.Title;
            this.Track = session.Track.TrackShort;
            this.Room = session.Room;
            this.Description = session.Description;
            this.Speakers = session.SpeakerList;
            this.Level = session.Level;
            this.IsFavorite = session.IsFavorite;

            try
            {
                this.eventTrackerService.TrackEvent($"[Session] {this.session.GetModel().Id}");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async void AddToCalendar()
        {
            var calendarEvent = new CalendarEvent
            {
                Description = this.Description,
                Name = this.Title,
                Location = $"Kumodraška 261, Beograd, Sala {this.Room}",
                Start = this.session.StartDateTime,
                AllDay = false,
                End = this.session.EndDateTime,
                ExternalId = this.session.GetModel().Id.ToString()
            };

            if (await this.reminderService.AddReminderAsync(this.session.GetModel().Id.ToString(), calendarEvent))
            {
                await this.contentDialogService.Alert(
                    "Kalendar",
                    "Uspešno ste dodali sesiju u kalendar.");

                try
                {
                    this.eventTrackerService.TrackEvent($"[Calendar] {this.session.GetModel().Id}");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                await this.contentDialogService.Alert(
                    "Greška",
                    "Sesija nije dodata u kalendar.");
            }
        }
    }
}