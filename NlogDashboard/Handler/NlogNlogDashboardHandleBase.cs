using System;
using System.Threading.Tasks;

namespace NlogDashboard.Handler
{
    public abstract class NlogNlogDashboardHandleBase : INlogDashboardHandle
    {

        protected NlogNlogDashboardHandleBase(NlogDashboardContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public NlogDashboardContext Context { get; }

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
