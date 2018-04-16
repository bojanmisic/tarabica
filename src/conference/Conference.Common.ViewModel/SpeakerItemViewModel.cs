namespace Conference.Common.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;

    using OpenMVVM.Core;

    public class SpeakerItemViewModel
    {
        private readonly Speaker speaker;

        public SpeakerItemViewModel(Speaker speaker, ConferenceData conferenceData, IConferenceRepository conferenceRepository, bool getSessions = true)
        {
            this.speaker = speaker;

            if (getSessions)
            {
                this.Sessions = conferenceData.SessionSpeakerRelations
                    .Where(r => r.SpeakerId == this.speaker.Id)
                    .Select(relation => conferenceData.Sessions.First(s => s.Id == relation.SessionId))
                    .Select(s => new SessionItemViewModel(s, conferenceData, conferenceRepository, false))
                    .ToList();
            }     
        }

        public string ImageUrl => $"http://tarabica.msforge.net/Content/images/speakers/{this.speaker.Id}.jpg";

        public string Bio => this.speaker.Bio;

        public string Company => this.speaker.Company;

        public string Name => $"{this.speaker.FirstName} {this.speaker.LastName}";

        public List<SessionItemViewModel> Sessions { get; }

        public Speaker GetModel()
        {
            return this.speaker;
        }
    }
}
