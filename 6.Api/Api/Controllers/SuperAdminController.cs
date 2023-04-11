using System.Collections;
using AutoMapper;
using Framework;
using Framework.CustomAttributes;
using Framework.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[PermissionAuthorize]
public class SuperAdminController : BaseController
{
    private readonly IRouteDetector _routeDetector;

    public SuperAdminController(IRouteDetector routeDetector, IMapper mapper, HttpContext context) 
        : base(mapper, context)
    {
        _routeDetector = routeDetector;
    }

    [HttpGet]
    public IActionResult GetAllPermissions()
    {
        return Ok(_routeDetector.GetAllRoutes(typeof(SuperAdminController).Assembly));
    }
}