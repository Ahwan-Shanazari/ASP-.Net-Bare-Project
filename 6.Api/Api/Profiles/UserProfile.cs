using Api.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Api.Profiles;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<IdentityUser<long>, UserSignupDto>().ReverseMap();
        CreateMap<IdentityUser<long>, LoggedInUserDto>().ReverseMap();
    }
}