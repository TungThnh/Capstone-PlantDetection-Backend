﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Data;
using Application.Services.Interfaces;
using Application.Services.Implementations;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure.Configurations
{
    public static class AppConfiguration
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPlantCategoryService, PlantCategoryService>();
            services.AddScoped<IPredictionService, PredictionService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IPlantService, PlantService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExamService, ExamService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddFirebase(this IServiceCollection services)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(Path.Combine(currentDirectory, "firebase-adminsdk.json")),
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plant Detection Service - ASP.Net 6.0 Repository Pattern", Description = "APIs Service", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                 });
            });
        }

        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
        }
    }
}