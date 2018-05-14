using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NlogDashboard.Handle
{
    public abstract class NlogNlogDashboardHandleBase : INlogDashboardHandle
    {

        protected NlogNlogDashboardHandleBase(NlogDashboardContext context,SqlConnection conn)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Conn = conn ?? throw new ArgumentNullException(nameof(conn));
        }

        public NlogDashboardContext Context { get; }
        public SqlConnection Conn { get; }

        public virtual async Task<string> View<T>(T model) where T : class, new()
        {
            return await Context.Engine.CompileRenderAsync(Context.Route.View, model);
        }

        public virtual async Task<string> View(object model = null)
        {
            return await Context.Engine.CompileRenderAsync(Context.Route.View, model);
        }
    }
}
