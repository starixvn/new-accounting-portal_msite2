using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;
using SharpRaven.Data;
using static StackExchange.Redis.Role;

namespace Maintenance.DeleteTempVariable.Services
{
    public interface AmendmentResultService
    {
        Task<bool> UpdateStatus(AmendmentResultUpdateRequest request);

        Task<List<AmendmentResult>> GetResults();
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

        public async Task<List<AmendmentResult>> GetResults()
        {
            var results = new List<AmendmentResult>();
            foreach (var server in appSettingsKey.SiteDbServer.Where(s => s != "DBTXN"))
            {
                var criteria = new AmendmentResultSelCriteria
                {
                    DbServer = server,
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

            foreach (var item in request.Results)
            {
                var criteria = mapper.Map<AmendmentResultUpdateCriteria>(item);
                repository.Execute(criteria);
            }

            return true;
        }
    }
}