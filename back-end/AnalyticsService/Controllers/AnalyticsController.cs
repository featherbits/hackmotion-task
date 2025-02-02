using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnalyticsService.Services.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers;

public class DeviceInfoRequestPayload
{
    [Required]
    public int? ScreenWidth { get; set; }
    [Required]
    public int? ScreenHeight { get; set; }
    [Required]
    public string? UserAgent { get; set; }
}

public class EventRequestPayload
{
    public string? Name { get; set; }
    public string? PageUrl { get; set; }
    public string? Token { get; set; }
}

[ApiController]
[Route("api/analytics")]
public class AnalyticsController(
    IAnalyticsService analytics
) : ControllerBase
{
    [HttpPost("user")]
    public async Task<ActionResult> CreateUser([FromBody] DeviceInfoRequestPayload payload)
    {
        var token = await analytics.CreateUser(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "undefined", new DeviceInfo
        {
            ScreenWidth = payload.ScreenWidth!.Value,
            ScreenHeight = payload.ScreenHeight!.Value,
            UserAgent = payload.UserAgent!,
        });
        return Ok(new
        {
            token
        });
    }

    [HttpPost("event")]
    public async Task AddEvent([FromBody] EventRequestPayload payload)
    {
        await analytics.AddEvent(
            payload.Token!,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "undefined",
            new EventPayload
            {
                Name = payload.Name!,
                PageUrl = payload.PageUrl!
            }
        );
    }
}