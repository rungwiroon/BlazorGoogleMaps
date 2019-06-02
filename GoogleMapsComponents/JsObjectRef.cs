using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class JsObjectRef : IDisposable
    {
        protected readonly Guid _guid;

        internal Guid Guid
        {
            get { return _guid; }
        }

        protected readonly IJSRuntime _jsRuntime;

        public JsObjectRef(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public JsObjectRef(
            IJSRuntime jsRuntime,
            Guid guid)
        {
            _jsRuntime = jsRuntime;
            _guid = guid;
        }

        public JsObjectRef(
            IJSRuntime jsRuntime,
            string functionName,
            params object[] args)
            : this(jsRuntime, Guid.NewGuid(), functionName, args)
        {
            
        }

        public JsObjectRef(
            IJSRuntime jsRuntime, 
            Guid guid, 
            string functionName,
            params object[] args)
        {
            _jsRuntime = jsRuntime;
            _guid = guid;

            _jsRuntime.MyInvokeAsync<object>(
                "googleMapsObjectManager.createObject",
                new object[] { guid, functionName }
                    .Concat(args).ToArray()
            );
        }

        public virtual void Dispose()
        {
            _jsRuntime.InvokeAsync<object>(
                "googleMapsObjectManager.dispose",
                _guid.ToString()
            );
        }

        public Task<T> InvokeAsync<T>(string functionName, params object[] args)
        {
            return _jsRuntime.MyInvokeAsync<T>(
                "googleMapsObjectManager.invoke",
                new object[] { _guid.ToString(), functionName }
                    .Concat(args).ToArray()
            );
        }

        public override bool Equals(object obj)
        {
            var other = obj as JsObjectRef;

            if (other == null)
            {
                return false;
            }
            else
            {
                return other.Guid == this.Guid;
            }
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
    }
}
