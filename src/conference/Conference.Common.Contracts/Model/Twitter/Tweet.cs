namespace Conference.Common.Contracts.Model.Twitter
{
    using System;

    using Newtonsoft.Json;

    public class Tweet
    {
        public Int64 Id { get; set; }

        [JsonProperty(PropertyName = "Created_at")]
        public string CreatedAt { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Entities Entities { get; set; }

        public override string ToString()
        {
            return $"[{this.CreatedAt}] {this.User.ScreenName}: {this.Text}";
        }
    }
}
