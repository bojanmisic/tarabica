namespace Conference.Common.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;
    using OpenMVVM.Web;

    public class ConferenceDataService : IConferenceDataService
    {
        private const string ServiceGetDataMethod = "GetData";

        private const string ServiceGetVersionMethod = "GetVersion";

        private const string ServiceUriString = "http://tarabica.msforge.net/Device/";

        private readonly IHttpClient httpClient;

        public ConferenceDataService(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResult<ConferenceData>> GetConferenceDataAsync(CancellationToken cancellationToken)
        {
            var data =
                await
                this.httpClient.GetJsonAsync<ConferenceData>(ServiceUriString + ServiceGetDataMethod, cancellationToken);

            if (data.Successful)
            {
                //foreach (var speaker in data.Value.Speakers)
                //{
                //    speaker.PictureUrl = $"{data.Value.PicturesLocation}/{speaker.PictureUrl}";
                //}
            }
            else
            {
                return ServiceResult<ConferenceData>.CreateError(
                    data.ErrorCode,
                    data.ErrorMessage,
                    data.ErrorDescription);
            }

            return data;
        }

        public async Task<ServiceResult<int>> GetVersionAsync(CancellationToken cancellationToken)
        {
            return await this.httpClient.GetJsonAsync<int>(ServiceUriString + ServiceGetVersionMethod, cancellationToken);
        }
    }
}