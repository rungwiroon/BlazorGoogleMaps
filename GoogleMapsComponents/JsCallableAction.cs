using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents
{
    public class JsCallableAction
    {
        private readonly Action _action;

        public JsCallableAction(Action action)
        {
            _action = action;
        }

        [JSInvokable]
        public void Invoke()
        {
            _action();
        }
    }

    //public class JsCallableActionJObject
    //{
    //    private readonly Action<JObject> _action;

    //    public JsCallableActionJObject(Action<JObject> action)
    //    {
    //        _action = action;
    //    }

    //    [JSInvokable]
    //    public void Invoke(string arg)
    //    {
    //        if (string.IsNullOrEmpty(arg))
    //            _action(null);
    //        else
    //            _action(JObject.Parse(arg));
    //    }
    //}
}
