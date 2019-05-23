using System;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LogDashboard.Models;
using LogDashboard.StackTrace;

namespace LogDashboard.Handle
{
    public abstract class LogDashboardHandleBase : ILogDashboardHandle
    {
        protected LogDashboardHandleBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ViewBag = new ExpandoObject();
        }

        public LogDashboardContext Context { get; set; }

        public IServiceProvider ServiceProvider { get; }

        public dynamic ViewBag { get; set; }

 

        public virtual async Task<string> View(object model = null, string viewName = null)
        {
            ViewBag.CustomPropertyInfos = Context.Options.CustomPropertyInfos;
            ViewBag.LogModelType = Context.Options.LogModelType;
            Context.HttpContext.Response.ContentType = "text/html";
            ViewBag.DashboardMapPath = Context.Options.PathMatch;
            ViewBag.Brand = Context.Options.Brand;
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
