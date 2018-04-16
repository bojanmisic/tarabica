namespace Conference.Common.Contracts.Model.Twitter
{
    using System;

    using Newtonsoft.Json;

    public class Url
    {
        [JsonProperty(PropertyName = "Url")]
        public string UrlString { get; set; }

        public override string ToString()
        {
            return $"{this.UrlString}";
        }
    }
}
