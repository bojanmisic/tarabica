namespace Conference.Common.Contracts.Model.Twitter
{
    using Newtonsoft.Json;

    public class User
    {
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty(PropertyName = "Profile_image_url_https")]
        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return $"{this.Name} ({this.ScreenName})";
        }
    }
}
