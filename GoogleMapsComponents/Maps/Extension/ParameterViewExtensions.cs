using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// Contains extension methods for <see cref="ParameterView" />.
/// </summary>
internal static class ParameterViewExtensions
{
    /// <summary>
    /// Checks if a parameter changed.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="parameterValue">The parameter value (SHOULD NOT BE ENTERED MANUALLY).</param>
    /// <returns><c>true</c> if the parameter value has changed, <c>false</c> otherwise.</returns>
    internal static bool DidParameterChange<T>(this ParameterView parameters, T parameterValue, [CallerArgumentExpression("parameterValue")] string parameterName = "")
    {
        if (parameters.TryGetValue(parameterName, out T? value) && value != null)
        {
            return !EqualityComparer<T>.Default.Equals(value, parameterValue);
        }

        return false;
    }
}