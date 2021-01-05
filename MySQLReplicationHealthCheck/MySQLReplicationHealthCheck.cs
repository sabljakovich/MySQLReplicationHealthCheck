using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MySql.Data.MySqlClient;

namespace MySQLReplicationHealthCheck
{
    public class MySQLReplicationHealthCheck : IHealthCheck
    {
        private readonly string _mySQLConnectionString;

        private readonly int SLAVE_IO_RUNNING_INDEX = 10;
        private readonly int SLAVE_SQL_RUNNING_INDEX = 11;

        public MySQLReplicationHealthCheck(string mySQLConnectionString)
        {
            _mySQLConnectionString = mySQLConnectionString;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            using (var connection = new MySqlConnection(_mySQLConnectionString))
            {

                var slaves = connection.ExecuteReader("SHOW SLAVE STATUS;");

                if (slaves == null)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy("Replication not working"));
                }

                while (slaves.Read())
                {
                    if (slaves.GetString(SLAVE_IO_RUNNING_INDEX) != "YES" || slaves.GetString(SLAVE_SQL_RUNNING_INDEX) != "YES")
                    {
                        Task.FromResult(HealthCheckResult.Unhealthy("Replication not working"));
                    }
                }

            }

            return Task.FromResult(HealthCheckResult.Healthy("Replication OK"));
        }
    }

}
