namespace LogDashboard.Extensions
{
    public static class StringArrayExtensions
    {
        public static string TryGetValue(this string[] array, int index)
        {
            try
            {
                return array[index].Trim().Replace(',', '.');
            }
            catch
            {
                return null;
            }
        }
    }
}
