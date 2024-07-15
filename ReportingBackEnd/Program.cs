using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Reporting.Application.Services;
using Reporting.Core.IService;
using Reporting.Infrastracture.Configuration;
using System.Web.Services.Description;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200",
                                              "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});


builder.Services.Configure<ReportingDatabaseSettings>(
    builder.Configuration.GetSection("ReportingDatabase"));
//
var databaseSettings = builder.Configuration.GetSection("ReportingDatabase").Get<ReportingDatabaseSettings>();
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(databaseSettings.ConnectionString));


builder.Services.AddScoped(s =>
    s.GetRequiredService<IMongoClient>().GetDatabase(databaseSettings.DatabaseName));

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IExecutionService, ExecutionService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IDiagramService, DiagramService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();