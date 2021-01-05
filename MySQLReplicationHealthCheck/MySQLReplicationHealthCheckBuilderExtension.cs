using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MySQLReplicationHealthCheck
{
    public static class MySQLReplicationHealthCheckBuilderExtension
    {
        public static IHealthChecksBuilder AddMySqlReplication(this IHealthChecksBuilder builder, string connectionString, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
        {
            return builder.Add(new HealthCheckRegistration(
                name ?? "MySQL Replication",
                serviceProvider => new MySQLReplicationHealthCheck(connectionString),
                failureStatus,
                tags,
                timeout
            ));
        }
    }
}
