using System.Net;
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
    
    protected ActionResult SendResult(HttpStatusCode statusCode= HttpStatusCode.OK, string message = "",bool isSuccess = true)
    {
        return new ApiResult() { StatusCode = statusCode, Message = message , IsSuccess = isSuccess};
    }
    
    protected ActionResult SendResult<TData>(TData data,HttpStatusCode statusCode= HttpStatusCode.OK, string message = "",bool isSuccess = true)
    {
        return new ApiResult<TData>() { StatusCode = statusCode, Message = message , IsSuccess = isSuccess,Data = data};
    }

    protected string CurrentUserId => HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
    protected string CurrentUserName => HttpContext.User?.FindFirstValue(ClaimTypes.Name) ?? "";
}