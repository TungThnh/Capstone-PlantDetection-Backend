using Application.Settings;
using Data.Mappings;
using Domain.Entities;
using Hangfire;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var allowSpecificOrigins = "_allowSpecificOrigins";
var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<PlantDetectionContext>(options =>
        options.UseSqlServer(sqlConnectionString));

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(sqlConnectionString));

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
);
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDependenceInjection();
builder.Services.AddFirebase();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseCors(allowSpecificOrigins);

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI();

app.UseHangfireDashboard("/hangfire");

app.UseHttpsRedirection();

app.UseJwt();

app.UseAuthorization();

app.MapControllers();

app.Run();