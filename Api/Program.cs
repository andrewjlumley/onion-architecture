using Api;
using Api.Endpoints;
using Api.Extensions;
using Services;
using Services.Article;
using Common.Exceptions;
using Domain.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.Repositories;
using static System.Net.Mime.MediaTypeNames;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddAutoMapper(typeof(ArticleMappingProfile));

builder.Services.AddDbContextPool<ApplicationDbContext>(o =>
{
	var connectionString = builder.Configuration.GetConnectionString("Database");

	string assemblyName = "Persistence"; // typeof(ApplicationDbContext).Namespace!;
	o.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(assemblyName));
	// To add migration:
	// dotnet ef migrations add InitialCreate --project Persistence --startup-project Api
	// dotnet ef database update --project Persistence --startup-project Api
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseExceptionHandler(o => { });
app.MapArticleEndpoints();
app.UseHttpsRedirection();

app.Run();
