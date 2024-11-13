using CassetteBeastsAPI.Controllers;
using CassetteBeastsAPI.Services;
using CassetteBeastsAPI.Utilities;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
var policyName = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFeatureManagement();
builder.Services.AddSingleton<LiteDbContext, LiteDbContext>();
builder.Services.AddTransient<StatusService, StatusService>();
builder.Services.AddTransient<ElementalTypesService, ElementalTypesService>();
builder.Services.AddTransient<SpeciesService, SpeciesService>();
builder.Services.AddTransient<MovesService, MovesService>();
//builder.Services.AddTransient<DataManagementService, DataManagementService>();
builder.Services.AddTransient<FusionService, FusionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:5173") // specifying the allowed origin
                            .WithMethods("GET") // defining the allowed HTTP method
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    System.Environment.SetEnvironmentVariable("DevOnly","Allow");
}
else
{
    System.Environment.SetEnvironmentVariable("DevOnly", "Deny");
}

app.UseCors(policyName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
