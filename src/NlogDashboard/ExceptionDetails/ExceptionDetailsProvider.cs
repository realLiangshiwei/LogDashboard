using System;
using System.Collections.Generic;
using NlogDashboard.ExceptionDetails;

namespace NlogDashboard.ExceptionDetails
{
    public class ExceptionDetailsProvider
    {
        public List<ExceptionDetail> GetDetails(string exception)
        {
            if (string.IsNullOrWhiteSpace(exception))
            {
                return new List<ExceptionDetail>();
            }
            var exceptionArray = exception.Split(new[] { "--- End of stack trace from previous location where exception was thrown ---" }, StringSplitOptions.None);


            throw new System.Exception("未完成");
        }
    }
}
