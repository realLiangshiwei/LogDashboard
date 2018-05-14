using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RazorLight;

namespace NlogDashboard.Handler
{
    public class DashboardHandle : NlogNlogDashboardHandleBase
    {
        public DashboardHandle(NlogDashboardContext context) : base(context)
        {
        }

        public async Task<string> Home()
        {
            return await View();
        }
    }
}
