using System;

namespace Oversett.Domain.Contracts
{
    public interface ILogger
    {
        void LogException(Exception ex);


        void LogInfo(string information);


        void LogError(string errormessage);
    }
}
