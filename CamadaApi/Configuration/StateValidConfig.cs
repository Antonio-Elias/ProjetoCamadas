using Microsoft.AspNetCore.Mvc;

namespace CamadaApi.Configuration;

public static class StateValidConfig 
{
    public static IServiceCollection ValidacaoModelState(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }
}
