﻿using Microsoft.OpenApi.Models;
namespace Talabat.APIs.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Talabat.APIs", Version = "v1" }));
            return builder.Services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
