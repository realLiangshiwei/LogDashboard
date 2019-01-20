namespace LogDashboard.Models
{
    public class GetChartDataInput
    {
        public ChartDataType ChartDataType { get; set; }
    }

    public enum ChartDataType
    {
        Hour = 1,
        Day = 2,
        Week = 3,
        Month = 4
    }
}
