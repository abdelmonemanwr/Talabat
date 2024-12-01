using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Domain.Layer.IRepositories;
using Talabat.Repository.Layer;
using Talabat.APIs.DTOs.ErrorsDTOs;
using Talabat.Domain.Layer.IServices;
using Talabat.Service.Layer.Tokens;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services) 
        {
            //builder.Services.AddAutoMapper(Profile => Profile.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles)); // Simple Way

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddScoped<IBasketRepository, BasketRepository>();

            Services.AddScoped<ITokenService, TokenService>();

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    return new BadRequestObjectResult(new ErrorValidationRespose { Errors = errors });
                };
            });

            return Services;
        }
    }
}
