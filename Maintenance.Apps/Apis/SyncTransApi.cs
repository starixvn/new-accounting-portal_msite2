using Maintenance.Core.Models;
using Maintenance.Sync.Models;
using Maintenance.Sync.Requests;
using Maintenance.Sync.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Maintenance.Apps.Apis;

public static class SyncTransApi
{
    public static IEndpointRouteBuilder MapSyncTransApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/delete-temp-variable");
        api.MapGet("/ok", () => "OK");

        api.MapGet("/get-dbinfo", GetDbInfo);
        api.MapPost("/sync-transid-update", UpdateTransId);
        api.MapPost("/get-sync-match", GetBatchTransId);
        api.MapPost("/update-batch-transId", UpdateBatchTransId);

        return app;
    }

    private static Ok<DbInfo> GetDbInfo(SyncTransDirectlyService service) => TypedResults.Ok(service.GetDbInfo());

    private static async Task<Ok<Result>> UpdateTransId(UpdateTransIdsRequest request, SyncTransDirectlyService service)
        => TypedResults.Ok(await service.UpdateTransId(request));

    private static async Task<Ok<List<SynMatchIdInfo>>> GetBatchTransId(GetBatchTransIdRequest request, SyncMatchTransService service)
        => TypedResults.Ok(await service.GetBatchTransId(request));

    private static async Task<Ok<Result>> UpdateBatchTransId(UpdateBatchTransIdRequest request, SyncMatchTransService service)
        => TypedResults.Ok(await service.UpdateBatchTransId(request));
}