using Api.Dtos;
using AutoMapper;
using Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        var token = await _userServices.Login(user.UserName, user.Password);
        return Ok(token);
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