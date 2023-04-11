using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMapper _mapper;
    private readonly HttpContext _context;

    public BaseController(IMapper mapper, HttpContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    protected string CurrentUserId => _context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
    protected string CurrentUserName => _context.User?.FindFirstValue(ClaimTypes.Name) ?? "";
}