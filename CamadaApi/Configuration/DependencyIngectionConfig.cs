using CamadaApi.Extensions;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Notifications;
using CamadaBusiness.Services;
using CamadaData.Context;
using CamadaData.Repository;

namespace CamadaApi.Configuration;

public static class DependencyIngectionConfig 
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<MeuDbContext>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();

        services.AddScoped<INotificador, Notificador>();
        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();

        return services;
    }
}
