
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

namespace AnalyticsService.Services.Analytics;

public interface IAnalyticsDataAccess
{
    Task StoreUser(User user);
    Task StoreEvent(EventEntry entry);
}

public class LoggingAnalyticsDataAccess(
    ILogger<LoggingAnalyticsDataAccess> logger
) : IAnalyticsDataAccess
{
    public Task StoreEvent(EventEntry entry)
    {
        logger.LogInformation("Analytics event: {entry}", JsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }

    public Task StoreUser(User user)
    {
        logger.LogInformation("Analytics user created: {user}", JsonSerializer.Serialize(user));
        return Task.CompletedTask;
    }
}

public class UserToken
{
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
}

public class AnalyticsService(
    IAnalyticsDataAccess data,
    IDataProtectionProvider provider
) : IAnalyticsService
{
    private readonly IDataProtector protector = provider.CreateProtector("AnalyticsService");

    public async Task<string> CreateUser(string ipAddress, DeviceInfo deviceInfo)
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            IpAddress = ipAddress,
            DeviceInfo = deviceInfo,
            Created = DateTime.UtcNow
        };

        await data.StoreUser(user);
        return protector.Protect(JsonSerializer.Serialize(new UserToken
        {
            UserId = user.Id,
            Created = user.Created
        }));
    }

    public async Task AddEvent(string userTokenPayload, string ipAddress, EventPayload eventPayload)
    {
        var token = JsonSerializer.Deserialize<UserToken>(protector.Unprotect(userTokenPayload))
            ?? throw new Exception("Failed to obtain user token from token payload");

        var entry = new EventEntry
        {
            Payload = eventPayload,
            IpAddress = ipAddress,
            Created = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            UserId = token.UserId
        };

        await data.StoreEvent(entry);
    }
}