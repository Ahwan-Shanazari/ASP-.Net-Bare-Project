using AutoMapper;
using Data.Repositories.Interfaces;
using Framework;
using Framework.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TestController : BaseController
{
    private readonly IUserRepository _userRepository;

    public TestController(IUserRepository userRepository, IMapper mapper) : base(mapper)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [PermissionAuthorize]
    public IActionResult HelloWorld()
    {
        return Ok("Hello World");
    }

    //ToDo:this action is just for testing and must be deleted before the release and also we should not use repos directly in controllers 
    [HttpGet]
    public async Task<IActionResult> TestTheCache()
    {
        return Ok(await _userRepository.ReadAllFromCacheOrDb());
    }
}