using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Framework;

public class ApiResult
{
    private ApiResult Self;
    
    public ApiResult()
    {
        Self = this;
    }

    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public  static implicit operator ActionResult(ApiResult result)
    {
        return new ObjectResult(result.Self)
        {
            StatusCode = (int)result.StatusCode
        };
    }

    public IActionResult ToIActionResult()
    {
        return new ObjectResult(Self)
        {
            StatusCode = (int)StatusCode
        };
    }
}

public class ApiResult<T>:ApiResult
{
    public T? Data { get; set; }
}