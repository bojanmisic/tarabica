namespace Conference.Common.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Data;
    using Conference.Common.Contracts.Model;

    using Newtonsoft.Json;

    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Web;

    public class ConferenceRepository : IConferenceRepository
    {
        private const string ConferenceDataKey = "TarabicaData";

        private const string FavoritesDataKey = "FavoriteSession";

        private readonly ICacheService cacheService;

        private readonly IConferenceDataService conferenceDataService;

        public ConferenceRepository(IConferenceDataService conferenceDataService, ICacheService cacheService)
        {
            this.conferenceDataService = conferenceDataService;
            this.cacheService = cacheService;
        }

        public async Task<ServiceResult<ConferenceData>> GetConferenceData(CancellationToken cancellationToken = new CancellationToken())
        {
            ConferenceData conferenceData = null;

            string json = null;
            try
            {
                json = this.cacheService.GetValueOrDefault<string>(ConferenceDataKey);
            }
            catch (Exception)
            {

            }

            ConferenceData item = null;

            if (json != null)
            {
                try
                {
                    item = JsonConvert.DeserializeObject<ConferenceData>(json);
                }
                catch (Exception)
                {

                }
            }

            if (item != null)
            {
                conferenceData = item;

                if (cancellationToken.IsCancellationRequested)
                {
                    return this.FillInFavorites(conferenceData);
                }

                var versionId = conferenceData.Version;

                var result = await this.conferenceDataService.GetVersionAsync(cancellationToken);//.ConfigureAwait(false);

                int latestVersionId = 0;
                if (result.Successful)
                {
                    latestVersionId = result.Value;
                }

                if (versionId >= latestVersionId)
                {
                    return this.FillInFavorites(conferenceData);
                }
            }
            else
            {
                var assembly = typeof(ConferenceRepository).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("Conference.Common.Data.Resources.DefaultConferenceData.json");
                string text = string.Empty;

                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(text))
                {
                    try
                    {
                        conferenceData = JsonConvert.DeserializeObject<ConferenceData>(text);

                        var result =
                            await this.conferenceDataService.GetVersionAsync(cancellationToken);//.ConfigureAwait(false);

                        int latestVersionId = 0;

                        if (result.Successful)
                        {
                            latestVersionId = result.Value;
                        }

                        if (conferenceData.Version >= latestVersionId)
                        {
                            return this.FillInFavorites(conferenceData);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ServiceResult<ConferenceData>.CreateError(ex);
                    }
                }
            }

            var newData = await this.conferenceDataService.GetConferenceDataAsync(cancellationToken);//.ConfigureAwait(false);

            if (newData.Successful)
            {
                if (conferenceData != null)
                {
                    foreach (var session in conferenceData.Sessions)
                    {
                        var targetSession = newData.Value.Sessions.FirstOrDefault(s => s.Id == session.Id);

                        if (targetSession != null)
                        {
                            targetSession.IsFavorite = this.GetFavorite(session.Id);
                        }                
                    }
                }

                this.cacheService.AddOrUpdateValue(ConferenceDataKey, JsonConvert.SerializeObject(newData.Value));
            }
            else
            {
                newData = conferenceData;
                this.cacheService.AddOrUpdateValue(ConferenceDataKey, JsonConvert.SerializeObject(newData.Value));
            }

            return this.FillInFavorites(newData);
        }

        public bool SaveFavorite(int sessionId, bool isFavorite)
        {
            return this.cacheService.AddOrUpdateValue($"{FavoritesDataKey}_{sessionId}", isFavorite);
        }

        public bool GetFavorite(int sessionId)
        {
            return this.cacheService.GetValueOrDefault<bool>($"{FavoritesDataKey}_{sessionId}");
        }

        private ConferenceData FillInFavorites(ConferenceData conferenceData)
        {
            if (conferenceData == null)
            {
                return null;
            }

            foreach (var session in conferenceData.Sessions)
            {
                session.IsFavorite = this.GetFavorite(session.Id);
            }

            return conferenceData;
        }
    }
}