using AutoMapper;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Requests.SyncErrorLog;

namespace Maintenance.DeleteTempVariable;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MatchTicketRequest, GetTicketCriteria>();
        CreateMap<MatchTicketRequest, GetMatchesCriteria>();
        CreateMap<DeleteTempRequest, DeleteMatchCriteria>();

        CreateMap<ParlayLockingRequest, GetParlayLockingCriteria>();
        CreateMap<DeleteParlayLockingRequest, DeleteParlayLockingCriteria>();

        CreateMap<DeleteSettlementProcessingRequest, DeleteSettlementLockCriteria>();
        CreateMap<UpdateSettlementLogRequest, UpdateSettlementLogCriteria>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.CreatedTime));

        CreateMap<UpdateLogStatusRequest, UpdateLogStatusCriteria>();
        CreateMap<CheckLogIdsRequest, CheckLogIdsCriteria>();

        CreateMap<SettlementErrorLogDetailRequest, GetSettlementErrorLogDetailsCriteria>();
        CreateMap<DeleteSettlementProcessingErrorLogRequest, DeleteSettlementProcessingErrorLogCriteria>();
        CreateMap<UpdateSiteDbSSBErrorLogRequest, UpdateSiteDbSSBErrorLogCriteria>();
        CreateMap<DeleteSettlementLockErrorLogRequest, DeleteSettlementLockErrorLogCriteria>();

        CreateMap<UpdateTransIdsRequest, UpdateTransIdsCriteria>();

        CreateMap<AmendmentResultItem, AmendmentResultUpdateCriteria>()
           .ForMember(dest => dest.SessionIds, opt => opt.MapFrom(src => src.SessionId));
    }
}