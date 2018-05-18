using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NlogDashboard.Handle
{
    public abstract class NlogNlogDashboardHandleBase : INlogDashboardHandle
    {

        protected NlogNlogDashboardHandleBase(NlogDashboardContext context, SqlConnection conn)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Conn = conn ?? throw new ArgumentNullException(nameof(conn));
            ViewBag = new ExpandoObject();
        }

        public NlogDashboardContext Context { get; }
        public SqlConnection Conn { get; }

        public dynamic ViewBag { get; set; }



        public virtual async Task<string> View(object model = null, string viewName = null)
        {
            ViewBag.DashboardMapPath = Context.Options.PathMatch;
            ViewBag.View = Context.Route.View;
            return await Context.Engine.CompileRenderAsync(viewName ?? Context.Route.View, model, ViewBag);
        }


        public virtual string Json(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
