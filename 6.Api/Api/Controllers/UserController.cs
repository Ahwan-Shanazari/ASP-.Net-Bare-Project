using Api.Dtos;
using AutoMapper;
using Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;

public class UserController : BaseController
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices, IMapper mapper) 
        : base(mapper)
    {
        _userServices = userServices;
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        var result = await _userServices.Login(user.UserName, user.Password);
        if (result)
            return Ok();
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(UserDto dto)
    {
        var user = _mapper.Map<IdentityUser<long>>(dto);
        var result = await _userServices.CreateAccount(user, dto.Password);
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
        var userDto = _mapper.Map<LoggedInUserDto>(userEntity);
        return Ok(userDto);
    }
}