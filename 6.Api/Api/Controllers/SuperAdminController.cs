using System.Collections;
using Framework;
using Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SuperAdminController : BaseController
{
    private readonly IRouteDetector _routeDetector;

    public SuperAdminController(IRouteDetector routeDetector)
    {
        _routeDetector = routeDetector;
    }

    [HttpGet]
    public IActionResult GetAllPermissions()
    {
        return Ok(_routeDetector.GetAllRoutes(typeof(SuperAdminController).Assembly));
    }
}