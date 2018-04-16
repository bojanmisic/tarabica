namespace Conference.Common.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;

    public static class ConferenceHelpers
    {
        public static ObservableCollection<SessionGroupViewModel> GroupSessions(IConferenceRepository conferenceRepository, ConferenceData conferenceData, bool favoritesOnly = false)
        {
            if (conferenceData == null)
            {
                return null;
            }

            var sessionGroupTileInfoList = new List<SessionGroupViewModel>();

            var groupHeaders = conferenceData.Slots
                .OrderBy(s => (s.StartHour * 100) + s.StartMinute)
                .Select(s => $"{s.StartHour:00}:{s.StartMinute:00} - {s.EndHour:00}:{s.EndMinute:00}")
                .Distinct()
                .ToArray();

            var sessionInfos = conferenceData.Sessions
                .Where(s => !favoritesOnly || s.IsFavorite)
                .Select(s => new SessionItemViewModel(s, conferenceData, conferenceRepository))
                .OrderBy(s => s.StartTime);

            var groups = new Dictionary<string, SessionGroupViewModel>();

            foreach (var header in groupHeaders)
            {
                var group = new SessionGroupViewModel(header);
                sessionGroupTileInfoList.Add(group);
                groups[header] = group;
            }

            foreach (var sessionInfo in sessionInfos)
            {
                var groupName = $"{sessionInfo.StartTime} - {sessionInfo.EndTime}";
                groups[groupName].Sessions.Add(sessionInfo);
            }

            var sessionGroupTileInfos = new ObservableCollection<SessionGroupViewModel>();
            foreach (var sessionGroup in sessionGroupTileInfoList)
            {
                if (sessionGroup.Sessions.Count > 0)
                {
                    sessionGroupTileInfos.Add(sessionGroup);
                }
            }

            return sessionGroupTileInfos;
        }
    }
}
