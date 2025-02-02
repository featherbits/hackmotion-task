namespace AnalyticsService.Services.Analytics;

public class DeviceInfo
{
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public required string UserAgent { get; set; }
}

public class User
{
    public required DeviceInfo DeviceInfo { get; set; }
    public required string IpAddress { get; set; }
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
}

public class EventPayload
{
    public required string Name { get; set; }
    public required string PageUrl { get; set; }
}

public class EventEntry
{
    public required EventPayload Payload { get; set; }
    public required string IpAddress { get; set; }
    public DateTime Created { get; set; }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public interface IAnalyticsService
{
    Task<string> CreateUser(string ipAddress, DeviceInfo deviceInfo);
    Task AddEvent(string userTokenPayload, string ipAddress, EventPayload eventPayload);
}