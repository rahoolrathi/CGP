using CgbSteps.Api.Interfaces;
using CgbSteps.Api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CgbSteps.Api.Controllers;

[ApiController]
[Route("api/v1/system")]
public sealed class SystemController(ISystemService systemService) : ControllerBase
{
    [HttpGet("info")]
    [AllowAnonymous]
    public ActionResult<SystemInfoResponse> GetInfo() => systemService.GetInfo();
}
