using AutoMapper;

namespace Talabat.APIs.Helpers
{
    public class ImageUrlResolver<T, TDTO> : IValueResolver<T, TDTO, string>
    {
        private readonly IConfiguration configuration;

        public ImageUrlResolver(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        public string Resolve(T source, TDTO destination, string destMember, ResolutionContext context)
        {
            string? imageUri = (string?)source?.GetType().GetProperty("ImageUrl")?.GetValue(source);
            if (!string.IsNullOrEmpty(imageUri))
            {
                return $"{configuration["BaseApiUrl"]}/{imageUri}";
            }
            return "no image found";
        }
    }
}
