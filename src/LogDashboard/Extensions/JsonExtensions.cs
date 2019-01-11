using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LogDashboard.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
