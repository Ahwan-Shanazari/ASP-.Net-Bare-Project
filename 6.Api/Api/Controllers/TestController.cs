using AutoMapper;
using Framework;
using Framework.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TestController:BaseController
{
    public TestController(IMapper mapper) : base(mapper)
    {
    }

    [HttpGet]
    [PermissionAuthorize]
    public IActionResult HelloWorld()
    {
        return Ok("Hello World");
    }
}