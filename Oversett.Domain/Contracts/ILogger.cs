using System;

namespace Oversett.Domain.Contracts
{
    public interface ILogger
    {
        void LogException(Exception ex);
    }
}
