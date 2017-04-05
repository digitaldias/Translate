using System;

namespace Translate.Domain.Contracts
{
    public interface IExceptionHandler
    {
        TResult Run<TResult>(Func<TResult> unsafeFunction);
    }
}
