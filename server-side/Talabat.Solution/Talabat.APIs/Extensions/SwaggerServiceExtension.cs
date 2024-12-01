using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Talabat.APIs.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection Services)
        {
            Services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
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

                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Talabat.APIs",
                        Contact = new OpenApiContact
                        {
                            Name = "Men3m",
                            Email = "abdelmonemanwr7777@gmail.com",
                        },
                        Description = "From cart to door, Talabat faster than before 😉",
                        //Extensions =
                        //{
                        //    {
                        //        "x-logo", new OpenApiObject
                        //        {
                        //            ["url"] = new OpenApiString("https://img.freepik.com/premium-vector/male-employees-sit-scooters-deliver-parcels-deliver-food-waving-hand-invite-use-service-there-were-parcel-boxes-floating-aroundall-floating-smartphone-delivery-concept_425581-19.jpg")
                        //        }
                        //    }
                        //}
                    });

                });
            return Services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}