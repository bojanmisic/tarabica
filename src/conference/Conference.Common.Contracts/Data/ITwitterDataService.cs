namespace Conference.Common.Contracts.Data
{
    using System;
    using System.Threading.Tasks;

    using Conference.Common.Contracts.Model.Twitter;

    using OpenMVVM.Web;

    public interface ITwitterDataService
    {
        Task<ServiceResult<TwitterData>> GetNewTweets(long minId);

        Task<ServiceResult<TwitterData>> GetOlderTweets(long maxId);
    }
}
