using Oversett.Domain.Contracts;
using System;

namespace Oversett
{
    public class Logger : ILogger
    {
        public void LogError(string errormessage)
        {
            Console.WriteLine($"[ERROR!] '{errormessage}");
        }


        public void LogException(Exception ex)
        {
            Console.WriteLine($">>EXCEPTION>> '{ex.Message}'\n\n{ex.StackTrace.ToString()}");
        }


        public void LogInfo(string information)
        {
            Console.WriteLine($"[Information]: '{information}'");
        }
    }
}
