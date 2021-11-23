using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal interface IJSObjRefWrapper : IAsyncDisposable
    {
        IJSObjectReference Value { get; }

        ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args);
        ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args);
    }

    internal struct JSObjRefWrapper : IJSObjRefWrapper
    {
        private readonly IJSObjectReference jsObjectRef;

        public IJSObjectReference Value => jsObjectRef;

        public JSObjRefWrapper(IJSObjectReference jsObjectRef)
        {
            this.jsObjectRef = jsObjectRef;
        }

        public async ValueTask DisposeAsync()
        {
            await jsObjectRef.DisposeAsync();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args)
        {
            return jsObjectRef.InvokeAsync<TValue>(identifier, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(
            string identifier, CancellationToken cancellationToken, params object?[]? args)
        {
            return jsObjectRef.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }
    }

    internal struct JSObjPropRefWrapper : IJSObjRefWrapper
    {
        private readonly IJSObjRefWrapper jsObjectRef;
        private readonly string propertyName;

        public IJSObjectReference Value => jsObjectRef.Value;

        public JSObjPropRefWrapper(string propertyName, IJSObjRefWrapper jsObjectRef)
        {
            this.jsObjectRef = jsObjectRef;
            this.propertyName = propertyName;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args)
        {
            return jsObjectRef.InvokeAsync<TValue>(
                $"{propertyName}.{identifier}", args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(
            string identifier, CancellationToken cancellationToken, params object?[]? args)
        {
            return jsObjectRef.InvokeAsync<TValue>(
                $"{propertyName}.{identifier}", cancellationToken, args);
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
