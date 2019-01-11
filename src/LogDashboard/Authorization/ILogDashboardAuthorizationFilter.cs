using System.Threading.Tasks;

namespace LogDashboard.Authorization
{
    public interface ILogDashboardAuthorizationFilter
    {
        bool Authorization(LogDashboardContext context);
    }
}
