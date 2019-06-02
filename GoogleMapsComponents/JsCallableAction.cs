using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents
{
    public class JsCallableAction : JsObjectRef
    {
        private Action<JObject> _action;

        public JsCallableAction(IJSRuntime jsRuntime, Action<JObject> action)
            : base(jsRuntime)
        {
            _action = action;
        }

        public override void Dispose()
        {

        }

        [JSInvokable]
        public void Invoke(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                _action(null);
            else
                _action(JObject.Parse(arg));
        }
    }
}
