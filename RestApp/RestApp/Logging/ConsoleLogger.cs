using System;

namespace RestApp.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Error(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}