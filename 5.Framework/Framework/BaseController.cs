using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;

    public BaseController(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected string CurrentUserId => HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
    protected string CurrentUserName => HttpContext.User?.FindFirstValue(ClaimTypes.Name) ?? "";
}