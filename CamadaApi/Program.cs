using CamadaApi.Configuration;
using CamadaData.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MeuDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ResolveDependencies();

builder.Services.ValidacaoModelState();

var app = builder.Build();

app.UseAuthentication();
app.Run();
