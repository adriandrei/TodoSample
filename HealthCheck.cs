using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoSample;

public class HealthCheck : IHealthCheck
{
    private readonly ILogger<HealthCheck> logger;

    public HealthCheck(ILogger<HealthCheck> logger)
    {
        this.logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
