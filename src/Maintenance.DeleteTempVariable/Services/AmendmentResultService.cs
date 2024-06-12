using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.Core.Models;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services
{
    public interface AmendmentResultService
    {
        Task<bool> UpdateStatus(AmendmentResultUpdateRequest request);

        Task<List<AmendmentResult>> GetResults(AmendmentResultRequest request);
    }

    public class AmendmentResultServiceImp : AmendmentResultService
    {
        private readonly IDynamicRepository repository;
        private readonly AppSettingsKey appSettingsKey;
        private readonly IMapper mapper;

        public AmendmentResultServiceImp(IDynamicRepository repository, IOptions<AppSettingsKey> options, IMapper mapper)
        {
            this.repository = repository;
            this.appSettingsKey = options.Value;
            this.mapper = mapper;
        }

        public async Task<List<AmendmentResult>> GetResults(AmendmentResultRequest request)
        {
            var results = new List<AmendmentResult>();
            foreach (var server in appSettingsKey.SiteDbServer.Where(s => s != "DBTXN"))
            {
                var criteria = new AmendmentResultSelCriteria
                {
                    DbServer = server,
                    StatusId = request.StatusId,
                    MatchId = request.MatchId,
                };

                var list = (await repository.FetchAsync<AmendmentResult>(criteria)).ToList();
                list.ForEach(i => i.DbServer = server);

                results.AddRange(list);
            }

            return results;
        }

        public async Task<bool> UpdateStatus(AmendmentResultUpdateRequest request)
        {
            if (request.Results.Count == 0) return true;

            request.Results.ForEach(item =>
            {
                var criteria = Create(item);

                repository.Execute(criteria);
            });

            return true;
        }

        private AmendmentResultUpdateCriteria Create(AmendmentResultItem item)
        {
            var criteria = mapper.Map<AmendmentResultUpdateCriteria>(item);

            criteria.StatusId = item.StatusId switch
            {
                (short)AmendmentStatus.FailSettle => (short)AmendmentStatus.Complete,
                (short)AmendmentStatus.TimeOut => (short)AmendmentStatus.Running,
                _ => 0
            };

            return criteria;
        }
    }
}