namespace LogDashboard.Models
{
    public class GetLogChartsOutput
    {
        public GetLogChartsOutput(int length)
        {
            All = new int[length];
            Error = new int[length];
            Warn = new int[length];
            Debug = new int[length];
            Fatal = new int[length];
            Info = new int[length];
            Trace = new int[length];
        }

        public int[] All { get; set; }

        public int[] Error { get; set; }

        public int[] Warn { get; set; }

        public int[] Debug { get; set; }

        public int[] Fatal { get; set; }

        public int[] Info { get; set; }

        public int[] Trace { get; set; }
    }
}
