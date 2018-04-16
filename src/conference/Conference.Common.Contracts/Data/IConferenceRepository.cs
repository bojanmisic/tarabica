namespace Conference.Common.Contracts.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Model;
    using OpenMVVM.Web;

    public interface IConferenceRepository
    {
        Task<ServiceResult<ConferenceData>> GetConferenceData(CancellationToken cancellationToken = new CancellationToken());

        bool SaveFavorite(int sessionId, bool isFavorite);

        bool GetFavorite(int sessionId);
    }
}
