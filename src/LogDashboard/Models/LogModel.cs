using System;
using System.Text.Json.Serialization;
using DapperExtensions.Mapper;

namespace LogDashboard.Models
{

    public class LogModel : ILogModel
    {

        public int Id { get; set; }

        public DateTime LongDate { get; set; }

        public string Level { get; set; }

        public string Message {
            get {
                return _message;//.Replace("\"","^").Replace("'","~").Replace("\r", "\\r").Replace("\n", "\\n");
            }
            set {
                _message = value;
            }
        }

        public string Logger { get; set; }

        public string Exception {
            get {
                return _exception;//.Replace("\"", "^").Replace("'", "~").Replace("\r", "\\r").Replace("\n", "\\n");
            }
            set {
                _exception = value;
            }
        }
                                                                                                             
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public string _exception { get; set; }

        [JsonIgnore,Newtonsoft.Json.JsonIgnore]
        public string _message { get; set; }
    }

}
