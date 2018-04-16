namespace Conference.Common.Contracts.Data
{
    using System.Threading;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Model;
    using OpenMVVM.Web;

    public interface IConferenceDataService
    {
        Task<ServiceResult<ConferenceData>> GetConferenceDataAsync(CancellationToken cancellationToken = new CancellationToken());

        Task<ServiceResult<int>> GetVersionAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
