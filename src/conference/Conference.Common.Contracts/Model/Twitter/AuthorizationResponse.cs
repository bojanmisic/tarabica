namespace Conference.Common.Contracts.Model.Twitter
{
    using System;

    using Newtonsoft.Json;

    public class AuthorizationResponse
    {
        [JsonProperty(PropertyName = "Token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "Access_token")]
        public string AccessToken { get; set; }

        public override string ToString()
        {
            return $"{this.TokenType}: {this.AccessToken}";
        }
    }
}
