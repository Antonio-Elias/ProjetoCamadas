using CamadaApi.Configuration;
using CamadaBusiness.Interfaces;
using CamadaBusiness.Notifications;
using CamadaBusiness.Services;
using CamadaData.Context;
using CamadaData.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MeuDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ResolveDependencies();

var app = builder.Build();

app.Run();
