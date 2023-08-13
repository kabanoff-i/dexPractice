namespace ExtensionsPractice
{
    public static class IntExtensions
    {
        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }
    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine(24.Seconds());
        }
    }
}
