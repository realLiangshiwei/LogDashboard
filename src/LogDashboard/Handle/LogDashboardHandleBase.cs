using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LogDashboard.Views;
using Microsoft.Extensions.DependencyInjection;
using LogDashboard.Extensions;

namespace LogDashboard.Handle
{
    public abstract class LogDashboardHandleBase : ILogDashboardHandle
    {
        protected LogDashboardHandleBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ViewData = new Dictionary<string, object>();
        }

        public LogDashboardContext Context { get; set; }

        public IServiceProvider ServiceProvider { get; }

        public Dictionary<string, object> ViewData { get; set; }



        public virtual async Task<string> View(object model = null, Type viewType = null)
        {
            Context.HttpContext.Response.ContentType = "text/html";
            ViewData["DashboardMapPath"] = Context.Options.PathMatch;
            ViewData["Brand"] = Context.Options.Brand;
            ViewData["View"] = Context.Route.View;

            //Activate View
            var view =
                ServiceProvider.GetRequiredService(viewType ?? Context.Route.View) as
                    RazorPage;

            if (view == null)
            {
                throw new ArgumentException("view not found");
            }

            if (model != null)
            {
                ViewData["Model"] = model.ToJsonString();
            }

            view.Context = Context;
            view.ViewData = ViewData;
            return await Task.FromResult(view.ToString());
        }


        public virtual string Json(object model)
        {
            Context.HttpContext.Response.ContentType = "text/json";
            return JsonConvert.SerializeObject(model);
        }
    }
}
