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
    [ServiceFilter(typeof(PermissionAuthorizeAttribute))]
    public IActionResult HelloWorld()
    {
        return Ok("Hello World");
    }
}