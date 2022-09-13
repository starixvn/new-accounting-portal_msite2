using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services
{
    public interface TempVariableService
    {
        Task<int> TicketCount(MatchTicketRequest request);

        Task<List<Match>> GetMatches(int? matchId);

        Task<Match> GetMatchDetail(int matchId);

        Task<int> DeleteMatch(DeleteTempRequest request);
    }

    public class TempVariableServiceImp : TempVariableService
    {
        private readonly IDynamicRepository repository;

        public TempVariableServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> TicketCount(MatchTicketRequest request)
        {
            var criteria = new GetTicketCriteria
            {
                MatchId = request.MatchId,
                KeyId = request.KeyId,
                Action = request.ActionName,
                MarketId = request.MarketId ?? string.Empty
            };

            var model = await repository.GetAsync<MatchTicket>(criteria);
            return model.TicketCount;
        }

        public async Task<List<Match>> GetMatches(int? matchId)
        {
            var criteria = new GetMatchesCriteria
            {
                MatchId = matchId ?? 0
            };

            return (await repository.FetchAsync<Match>(criteria)).ToList();
        }

        public async Task<Match> GetMatchDetail(int matchId)
        {
            var criteria = new GetMatchesCriteria
            {
                MatchId = matchId
            };

            return await repository.GetAsync<Match>(criteria);
        }

        public async Task<int> DeleteMatch(DeleteTempRequest request)
        {
            var criteria = new DeleteMatchCriteria
            {
                MatchId = request.MatchId,
                KeyId = request.KeyId,
                MarketId = request.MarketId
            };

            return await repository.ExecuteAsync(criteria);
        }
    }
}