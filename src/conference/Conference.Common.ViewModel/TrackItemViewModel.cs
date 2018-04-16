namespace Conference.Common.ViewModel
{
    using Conference.Common.Contracts.Model;

    public class TrackItemViewModel
    {
        public TrackItemViewModel(string track)
        {
            this.TrackShort = track;
            this.TrackLong = TrackHelper.GetTitleForTrack(this.TrackShort);
            this.TrackColor = TrackHelper.GetColorForTrack(this.TrackShort);
        }

        public string TrackShort { get; }

        public string TrackLong { get; }

        public string TrackColor { get; }
    }
}
