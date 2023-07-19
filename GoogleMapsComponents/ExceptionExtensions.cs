using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMapsComponents
{
    public static class ExceptionExtensions
    {


        public static bool HasInnerExceptionsOfType<T>(this Exception ex) where T : Exception => ex.GetInnerExceptionsOfType<T>().Any();

        public static IEnumerable<T> GetInnerExceptionsOfType<T>(this Exception ex) where T : Exception
        {
            var candidates = new[] { ex, ex.InnerException };

            var aggEx = ex as AggregateException;
            if (aggEx is not null)
            {
                var innerEceptions = aggEx?.InnerExceptions?.ToArray() ?? Array.Empty<Exception>();
                candidates = candidates.Concat(innerEceptions).Where(ex => ex is not null).ToArray();

            }
            IEnumerable<T> exceptions = candidates.Select(ex => ex as T).Where(ex => ex is not null).Cast<T>().Distinct();
            return exceptions;
        }
    }
}
