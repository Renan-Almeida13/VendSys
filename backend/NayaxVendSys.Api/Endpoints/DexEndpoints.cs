using NayaxVendSys.Api.Services;
using NayaxVendSys.Api.Models;

namespace NayaxVendSys.Api.Endpoints;

public static class DexEndpoints
{
    public static void MapDexEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/vdi-dex", async (DexRequest request, DexParser dexParser, DatabaseService dbService) =>
        {
            try
            {
                var dexMeter = dexParser.ParseDexMeter(request.DexContent, request.Machine);
                var dexMeterId = await dbService.SaveDexMeterAsync(dexMeter);
                
                var laneMeters = dexParser.ParseDexLaneMeters(request.DexContent, dexMeterId);
                await dbService.SaveDexLaneMetersAsync(laneMeters);

                return Results.Ok(new { message = "DEX data saved successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("SaveDex")
        .WithOpenApi()
        .RequireAuthorization();
    }
} 