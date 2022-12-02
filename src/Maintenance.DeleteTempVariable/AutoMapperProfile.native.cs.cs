using AutoMapper;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Requests;

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
    }
}