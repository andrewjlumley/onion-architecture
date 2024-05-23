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
using Microsoft.AspNetCore.Server.IISIntegration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

builder.Services.AddEndpointsApiExplorer();	
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddTransient<GlobalExceptionHandler>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.ApplyMigrations();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandler>();
app.MapArticleEndpoints();
app.UseHttpsRedirection();
app.UseAuthentication();

app.Run();
