using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents;

public static class ExceptionExtensions
{
    public static bool HasInnerExceptionsOfType<T>(this Exception ex) where T : Exception => ex.GetInnerExceptionsOfType<T>().Any();

    public static IEnumerable<T> GetInnerExceptionsOfType<T>(this Exception ex) where T : Exception
    {
        var candidates = new[] { ex, ex.InnerException };

        if (ex is AggregateException aggEx)
        {
            var innerEceptions = aggEx.InnerExceptions.ToArray();
            candidates = candidates.Concat(innerEceptions).Where(x => x is not null).ToArray();

        }

        var exceptions = candidates.Select(x => x as T).Where(x => x is not null).Cast<T>().Distinct();
        return exceptions;
    }
}