namespace Conference.Common.Data
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Conference.Common.Contracts.Data;
    using Contracts.Model.Twitter;

    using Newtonsoft.Json;

    using OpenMVVM.Web;

    public class TwitterDataService : ITwitterDataService
    {

        private const string ConsumerKey = "Gdj5GxIGSDWYg8kgj9ca9fy7X";
        private const string ConsumerSecret = "TP0ryHOkvzI72HddDyoAyhp30oTwOECxf6428ZdldX8T2gFipL";
        private const string RequestTokenUrl = "https://api.twitter.com/oauth2/token";
        private const string SearchTweetsUrl = "https://api.twitter.com/1.1/search/tweets.json";

        private string bearerToken = null; 

        public async Task<ServiceResult<TwitterData>> GetNewTweets(long minId)
        {
            await this.Authorize();

            if (bearerToken != null)
            {
                string q = "q=%40tarabicaconf OR %23tarabica OR %23tarabica17 OR %23tarabicamvp";
                string count = "count=20";
                string result_type = "result_type=recent";
                string include_entities = "include_entities=true";
                string since_id = String.Format("since_id={0}", minId);

                string query = "?" + q + "&" + count + "&" + include_entities + "&" + result_type + "&" + since_id;

                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using (var client = new HttpClient(handler))
                {
                    try
                    {
                        var request = new HttpRequestMessage { Method = new HttpMethod("GET") };
                        request.Headers.Add("Accept-Encoding", "gzip");
                        request.Headers.Add("Authorization", $"Bearer {this.bearerToken}");

                        client.BaseAddress = new Uri(SearchTweetsUrl + query);

                        var result = await client.SendAsync(request);

                        string resultData = new StreamReader(await result.Content.ReadAsStreamAsync()).ReadToEnd();

                        var twitterData = JsonConvert.DeserializeObject<TwitterData>(resultData);

                        return twitterData;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("HTTP ERROR: " + e.Message);
                        return ServiceResult<TwitterData>.CreateError(e);
                    }
                }              
            }

            return ServiceResult<TwitterData>.CreateError(0, "Could not authorize Twitter service. Please check your Internet connection.", "");
        }

        public async Task<ServiceResult<TwitterData>> GetOlderTweets(long maxId)
        {
            await this.Authorize();

            if (bearerToken != null)
            {
                string q = "q=%40tarabicaconf OR %23tarabica OR %23tarabica17 OR %23tarabicamvp";
                string count = "count=20";
                string result_type = "result_type=recent";
                string include_entities = "include_entities=true";
                string since_id = String.Format("max_id={0}", maxId);

                string query = "?" + q + "&" + count + "&" + include_entities + "&" + result_type + "&" + since_id;

                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using (var client = new HttpClient(handler))
                {
                    try
                    {
                        var request = new HttpRequestMessage { Method = new HttpMethod("GET") };
                        request.Headers.Add("Accept-Encoding", "gzip");
                        request.Headers.Add("Authorization", $"Bearer {this.bearerToken}");

                        client.BaseAddress = new Uri(SearchTweetsUrl + query);

                        var result = await client.SendAsync(request);

                        string resultData = new StreamReader(await result.Content.ReadAsStreamAsync()).ReadToEnd();

                        var twitterData = JsonConvert.DeserializeObject<TwitterData>(resultData);

                        return twitterData;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("HTTP ERROR: " + e.Message);
                        return ServiceResult<TwitterData>.CreateError(e);
                    }
                }
            }

            return ServiceResult<TwitterData>.CreateError(0, "Could not authorize Twitter service. Please check your Internet connection.", "");
        }

        private async Task<AuthorizationResponse> Authorize()
        {
            var bearerTokenCredentialsEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Uri.EscapeUriString(ConsumerKey)}:{Uri.EscapeUriString(ConsumerSecret)}"));

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                try
                {
                    var request = new HttpRequestMessage { Method = new HttpMethod("POST") };
                    request.Headers.Add("Accept-Encoding", "gzip");
                    request.Headers.Add("Authorization", $"Basic {bearerTokenCredentialsEncoded}");

                    request.Content = new StringContent(
                        "grant_type=client_credentials",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded");

                    client.BaseAddress = new Uri(RequestTokenUrl);

                    var result = await client.SendAsync(request);

                    string resultData = new StreamReader(await result.Content.ReadAsStreamAsync()).ReadToEnd();

                    var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponse>(resultData);

                    this.bearerToken = authorizationResponse.AccessToken;
                    return authorizationResponse;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("HTTP ERROR: " + e.Message);
                    return ServiceResult<AuthorizationResponse>.CreateError(e);
                }
            }
        }
    }
}
