using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NLogDashboard.Handle
{
    public abstract class NlogNLogDashboardHandleBase : INLogDashboardHandle
    {

        protected NlogNLogDashboardHandleBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ViewBag = new ExpandoObject();
        }

        public NLogDashboardContext Context { get; set; }

        public IServiceProvider ServiceProvider { get; }

        public dynamic ViewBag { get; set; }


        public virtual async Task<string> View(object model = null, string viewName = null)
        {
            Context.HttpContext.Response.ContentType = "text/html";
            ViewBag.DashboardMapPath = Context.Options.PathMatch;
            ViewBag.View = Context.Route.View;
            return await Context.Engine.CompileRenderAsync(viewName ?? Context.Route.View, model, ViewBag);
        }


        public virtual string Json(object model)
        {
            Context.HttpContext.Response.ContentType = "text/json";
            return JsonConvert.SerializeObject(model);
        }
    }
}
