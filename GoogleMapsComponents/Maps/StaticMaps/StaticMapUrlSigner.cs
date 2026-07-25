using System;
using System.Security.Cryptography;
using System.Text;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Adds the <c>signature</c> parameter that the Maps Static API accepts as extra protection against
/// unauthorized use of an API key.
/// <para/>
/// <b>The URL signing secret must never reach the browser.</b> Signing a URL requires the secret, so this
/// class belongs in server side code: a Blazor Server app, an API endpoint, or a background job that
/// renders map images. In a WebAssembly app the secret would ship with the application and be readable by
/// anyone, which defeats the purpose of signing.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/digital-signature
/// </summary>
public static class StaticMapUrlSigner
{
    /// <summary>
    /// Appends a <c>signature</c> parameter to a Maps Static API URL.
    /// </summary>
    /// <param name="url">An absolute URL produced by <see cref="StaticMapUrlBuilder.Build"/>.</param>
    /// <param name="signingSecret">
    /// The URL signing secret from the Google Cloud console. This is not the API key.
    /// </param>
    /// <returns>The same URL with a valid signature appended.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="url"/> is not an absolute URL, or when either argument is empty.
    /// </exception>
    public static string Sign(string url, string signingSecret)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("The URL to sign cannot be null or whitespace.", nameof(url));

        if (string.IsNullOrWhiteSpace(signingSecret))
            throw new ArgumentException("The signing secret cannot be null or whitespace.", nameof(signingSecret));

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new ArgumentException("The URL to sign must be an absolute URL.", nameof(url));

        // Only the path and the query are signed, not the scheme or the host.
        var toSign = Encoding.ASCII.GetBytes(uri.AbsolutePath + uri.Query);
        var secret = Convert.FromBase64String(FromUrlSafeBase64(signingSecret));

        using var hmac = new HMACSHA1(secret);
        var signature = ToUrlSafeBase64(Convert.ToBase64String(hmac.ComputeHash(toSign)));

        var separator = string.IsNullOrEmpty(uri.Query) ? "?" : "&";
        return url + separator + "signature=" + signature;
    }

    private static string FromUrlSafeBase64(string value) => value.Replace('-', '+').Replace('_', '/');

    private static string ToUrlSafeBase64(string value) => value.Replace('+', '-').Replace('/', '_');
}
