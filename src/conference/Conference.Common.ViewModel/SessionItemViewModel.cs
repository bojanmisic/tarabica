namespace Conference.Common.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;

    using OpenMVVM.Core;

    public class SessionItemViewModel : ObservableObject
    {
        private ConferenceData conferenceData;

        private readonly Session session;

        private readonly IConferenceRepository conferenceRepository;

        public SessionItemViewModel(Session session, ConferenceData conferenceData, IConferenceRepository conferenceRepository, bool getSpeakers = true)
        {
            this.session = session;
            this.conferenceRepository = conferenceRepository;
            this.conferenceData = conferenceData;

            this.Track = new TrackItemViewModel(this.session.Track);

            if (getSpeakers)
            {
                this.SpeakerList = this.conferenceData.SessionSpeakerRelations
                   .Where(r => r.SessionId == this.session.Id)
                   .Select(relation => this.conferenceData.Speakers
                   .First(s => s.Id == relation.SpeakerId))
                   .Select(speaker => new SpeakerItemViewModel(speaker, this.conferenceData, conferenceRepository, true)).ToList();

                this.Speakers = string.Join(", ", this.SpeakerList.Select(s => s.Name));
            }

            var slot = this.conferenceData.Slots.First(s => s.TimeLine == session.TimeLine);
            this.StartTime = $"{slot.StartHour:00}:{slot.StartMinute:00}";
            this.EndTime = $"{slot.EndHour:00}:{slot.EndMinute:00}";
            var room = this.conferenceData.Rooms.FirstOrDefault(r => r.Id == session.RoomId);

            if (room != null)
            {
                this.Room = $"{room.Code:000}";
            }

            if (this.conferenceData.Days.Count > 0)
            {
                var dateTimeStart = this.conferenceData.Days[0].Date;
                dateTimeStart = dateTimeStart.AddHours(slot.StartHour);
                dateTimeStart = dateTimeStart.AddMinutes(slot.StartMinute);
                this.StartDateTime = dateTimeStart;

                var dateTimeEnd = this.conferenceData.Days[0].Date;
                dateTimeEnd = dateTimeEnd.AddHours(slot.EndHour);
                dateTimeEnd = dateTimeEnd.AddMinutes(slot.EndMinute);
                this.EndDateTime = dateTimeEnd;
            }
        }

        public bool IsFavorite
        {
            get
            {
                return this.session.IsFavorite;
            }

            set
            {
                this.session.IsFavorite = value;
                this.RaisePropertyChanged(() => this.IsFavorite);
                this.conferenceRepository.SaveFavorite(this.session.Id, value);
            }
        }

        public TrackItemViewModel Track { get; }

        public string StartTime { get; }
        
        public DateTime StartDateTime { get; }

        public string EndTime { get; }

        public DateTime EndDateTime { get; }

        public string Speakers { get; }

        public List<SpeakerItemViewModel> SpeakerList { get; }

        public string Room { get; }

        public string Description => this.session.Description;

        public string Title => this.session.Title;

        public string Language => this.session.Lang;

        public int Level => this.session.Level;

        public string SpeakersAndLevelDescription => $"{this.Speakers} ({this.Level})";

        public Session GetModel()
        {
            return this.session;
        }

        public ConferenceData GetConferenceData()
        {
            return this.conferenceData;
        }
    }
}
