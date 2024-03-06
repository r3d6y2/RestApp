using System;

namespace RestApp.Logging
{
    public interface ILogger
    {
        public void Error(Exception exception);
    }
}