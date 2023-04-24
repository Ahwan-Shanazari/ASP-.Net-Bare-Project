using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Framework;

public class ApiResult
{
    protected ApiResult Self;
    
    public ApiResult()
    {
        Self = this;
    }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public  static implicit operator ActionResult(ApiResult result)
    {
        return new ObjectResult(result.Self)
        {
            StatusCode = (int)result.StatusCode
        };
    }

    public IActionResult ToIActionResult(ApiResult result)
    {
        return new ObjectResult(result.Self)
        {
            StatusCode = (int)result.StatusCode
        };
    }
}

public class ApiResult<T>:ApiResult where T: class
{
    public T Data { get; set; }
}