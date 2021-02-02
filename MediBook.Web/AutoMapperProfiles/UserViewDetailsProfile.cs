namespace MediBook.Web.AutoMapperProfiles
{
    using AutoMapper;
    using MediBook.Core.DTOs;
    using MediBook.Web.Models;

    /// <summary>
    /// The UserDetails Profile to enable AutoMapper
    /// </summary>
    public class UserViewDetailsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewDetailsProfile"/> class
        /// </summary>
        public UserViewDetailsProfile()
        {
            CreateMap<UserDetailsViewModel, UserAccountDetailsDto>();
        }
    }
}