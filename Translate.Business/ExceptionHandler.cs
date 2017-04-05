using Translate.Domain.Contracts;
using System;

namespace Translate.Business
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;


        public ExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }


        public TResult Run<TResult>(Func<TResult> unsafeFunction)
        {
            if (unsafeFunction != null)
            {
                try
                {
                    return unsafeFunction.Invoke();
                }
                catch(Exception ex)
                {
                    _logger.LogException(ex);
                }
            }                
            return default(TResult);
        }
    }
}
