using Oversett.Domain.Contracts;
using System;

namespace Oversett
{
    public class Logger : ILogger
    {
        public void LogException(Exception ex)
        {
            Console.WriteLine($">>EXCEPTION>> '{ex.Message}'\n\n{ex.StackTrace.ToString()}");
        }
    }
}
