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
