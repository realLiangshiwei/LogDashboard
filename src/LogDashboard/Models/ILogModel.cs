using System;
using System.Text;

namespace LogDashboard.Models
{
    public interface ILogModel {

        string _message { get; set; }

        int Id { get; set; }

        DateTime LongDate { get; set; }

        string Level { get; set; }

        string Logger { get; set; }

        string Message {
            get {
                return System.Uri.EscapeDataString(_message);
            }
            set {
                _message = value;
            }
        }

        string Exception { get; set; }
    }
}
