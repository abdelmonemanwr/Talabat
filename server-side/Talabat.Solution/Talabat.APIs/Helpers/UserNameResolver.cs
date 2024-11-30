using AutoMapper;
using Talabat.APIs.DTOs.AuthDTOs;
using Talabat.Domain.Layer.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class UserNameResolver : IValueResolver<RegisterDTO, ApplicationUser, string>
    {
        public string Resolve(RegisterDTO source, ApplicationUser destination, string destMember, ResolutionContext context)
        {
            return source.Email.Split('@')[0]!;
        }
    }
}
