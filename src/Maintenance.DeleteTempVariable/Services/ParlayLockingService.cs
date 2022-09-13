using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services
{
    public interface ParlayLockingService
    {
        Task<List<MatchParlayLocking>> GetMatches(ParlayLockingRequest request);

        Task<int> DeleteMatch(DeleteParlayLockingRequest request);
    }

    public class ParlayLockingServiceImp : ParlayLockingService
    {
        private readonly IDynamicRepository repository;

        public ParlayLockingServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<MatchParlayLocking>> GetMatches(ParlayLockingRequest request)
        {
            var criteria = new GetParlayLockingCriteria
            {
                MatchId = request.MatchId ?? 0,
                TimeRange = request.TimeRange
            };

            return (await repository.FetchAsync<MatchParlayLocking>(criteria)).ToList();
        }

        public async Task<int> DeleteMatch(DeleteParlayLockingRequest request)
        {
            var criteria = new DeleteParlayLockingCriteria
            {
                Session = request.Session
            };

            return await repository.ExecuteAsync(criteria);
        }
    }
}