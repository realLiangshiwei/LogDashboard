namespace LogDashboard.Models
{
    public class GetLogChartsOutput
    {
        public GetLogChartsOutput(int length)
        {
            All = new int[length];
            Error = new int[length];
        }

        public int[] All { get; set; }

        public int[] Error { get; set; }
    }
}
