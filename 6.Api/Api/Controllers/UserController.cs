using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Dtos;
using AutoMapper;
using Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Interfaces;

namespace Api.Controllers;

public class UserController : BaseController
{
    private readonly IUserServices _userServices;
    private readonly IConfiguration _configuration;

    public UserController(IUserServices userServices, IMapper mapper,IConfiguration configuration) 
        : base(mapper)
    {
        _userServices = userServices;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        // var result = await _userServices.Login(user.UserName, user.Password);
        // if (result)
        //     return Ok();
        // return BadRequest();
        
        //ToDo: replace this with custom appsettings class
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtOptions"]));
        var signInCridentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //var tokenOptions = new JwtSecurityToken()
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(UserSignupDto signupDto)
    {
        var user = Mapper.Map<IdentityUser<long>>(signupDto);
        var result = await _userServices.CreateAccount(user, signupDto.Password);
        if (result)
            return Ok();
        return BadRequest();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userServices.Logout();
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCurrentLoggedInUserInfo()
    {
        var userEntity = await _userServices.FindUser(long.Parse(CurrentUserId));
        var userDto = Mapper.Map<LoggedInUserDto>(userEntity);
        return Ok(userDto);
    }
}