using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public abstract class JsObjectRef : IDisposable
    {
        protected readonly Guid _guid;

        public abstract void Dispose();

        internal Guid Guid
        {
            get { return _guid; }
        }

        protected readonly IJSRuntime _jsRuntime;

        public JsObjectRef(IJSRuntime jsRuntime)
            : this(jsRuntime, Guid.NewGuid())
        {
            
        }

        public JsObjectRef(IJSRuntime jsRuntime, Guid guid)
        {
            _jsRuntime = jsRuntime;
            _guid = guid;
        }
    }
}
