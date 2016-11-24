using System;

namespace BL
{
    public class BlCallResult
    {
        public enum BlResult
        {
            Succeess, CoffeeError, RecipeError, UserError, RecipeBookError, DbError
        };

        public BlResult Result { get; set; }

        public Exception Exception { get; set; }
    }

    public class BlCallResult<T> : BlCallResult where T : class
    {
        public T ReturnValue { get; set; }

        public BlCallResult()
        {
            Result = BlResult.Succeess;
            Exception = null;
            ReturnValue = null;
        }

        public BlCallResult(T returnValue)
        {
            Result = BlResult.Succeess;
            Exception = null;
            ReturnValue = returnValue;
        }

        public BlCallResult(BlResult result, Exception exception)
        {
            Result = result;
            Exception = exception;
        }
    }
}
