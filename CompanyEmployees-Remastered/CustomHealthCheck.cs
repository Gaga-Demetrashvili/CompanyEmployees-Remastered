using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CompanyEmployees_Remastered
{
    public class CustomHealthCheck : IHealthCheck
    {
        private Random _random = new Random();

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var responseTime = _random.Next(1, 300);

            return await Task.FromResult(responseTime switch
            {
                < 100 => HealthCheckResult.Healthy("Healthy result from CustomHealthCheck"),
                (>= 100) and (< 200) => HealthCheckResult.Degraded("Degraded result from CustomHealthCheck"),
                _ => HealthCheckResult.Unhealthy("Unhealthy result from CustomHealthCheck")
            });
        }
    }
}