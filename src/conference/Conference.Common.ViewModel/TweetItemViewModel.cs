namespace Conference.Common.ViewModel
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Conference.Common.Contracts.Model.Twitter;

    public class TweetItemViewModel
    {
        private readonly Tweet tweet;

        public TweetItemViewModel(Tweet tweet)
        {
            this.tweet = tweet;
            this.tweet.Text = System.Net.WebUtility.HtmlDecode(this.tweet.Text);

            foreach (Match match in Regex.Matches(this.tweet.Text, @"(?<!\w)[#|@]\w+"))
            {
                this.tweet.Text = this.tweet.Text.Replace(match.Value, $"<b>{match.Value}</b>");
            }
        }

        public string Link => this.tweet.Entities.Urls?.FirstOrDefault()?.UrlString ?? string.Empty;

        public string ImageUrl => this.tweet.User.ImageUrl;

        public string Text => this.tweet.Text;

        public string UserName => $"@{this.tweet.User.ScreenName}";

        public string Timestamp
        {
            get
            {
                var date = DateTime.ParseExact(this.tweet.CreatedAt, "ddd MMM dd HH:mm:ss %K yyyy", CultureInfo.InvariantCulture.DateTimeFormat);
                return LocalizedDateHelper.GetPrettyDateSince(date);
            }
        }

        public Tweet GetModel()
        {
            return this.tweet;
        }
    }
}
