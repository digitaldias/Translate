using System;

namespace Oversett.Domain.Contracts
{
    public interface IExceptionHandler
    {
        TResult Run<TResult>(Func<TResult> unsafeFunction);
    }
}
