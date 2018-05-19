namespace NlogDashboard.ExceptionDetails
{
    public class ExceptionDetail
    {
        public string ErrorCodeDetail { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FirstStack { get; set; }

        public string OtherStack { get; set; }
    }
}
