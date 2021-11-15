using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class JsInvokableAction
    {
        private readonly Action _delegate;

        public JsInvokableAction(Action @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public void Invoke0()
        {
            _delegate();
        }

        [JSInvokable]
        public void Invoke1()
        {
            _delegate();
        }

        [JSInvokable]
        public void Invoke2()
        {
            _delegate();
        }
    }

    public class JsInvokableAsyncAction
    {
        private readonly Func<Task> _delegate;

        public JsInvokableAsyncAction(Func<Task> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public Task Invoke0()
        {
            return _delegate();
        }

        [JSInvokable]
        public Task Invoke1()
        {
            return _delegate();
        }

        [JSInvokable]
        public Task Invoke2()
        {
            return _delegate();
        }
    }

    public class JsInvokableAsyncAction<T>
    {
        private readonly Func<T, Task> _delegate;

        public JsInvokableAsyncAction(Func<T, Task> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public Task Invoke1(T arg1)
        {
            return _delegate(arg1);
        }

        [JSInvokable]
        public void Invoke2(T arg1)
        {
            _delegate(arg1);
        }
    }

    public class JsInvokableAsyncFunc<TParam, TRet>
    {
        private readonly Func<TParam, Task<TRet>> _delegate;

        public JsInvokableAsyncFunc(Func<TParam, Task<TRet>> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public Task<TRet> Invoke1(TParam arg1)
        {
            return _delegate(arg1);
        }

        //[JSInvokable]
        //public Task<TRet> Invoke2(TParam arg1)
        //{
        //    return _delegate(arg1);
        //}
    }

    public class JsInvokableFunc<TParam, TRet>
    {
        private readonly Func<TParam, TRet> _delegate;

        public JsInvokableFunc(Func<TParam, TRet> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public TRet Invoke1(TParam arg1)
        {
            return _delegate(arg1);
        }

        //[JSInvokable]
        //public Task<TRet> Invoke2(TParam arg1)
        //{
        //    return _delegate(arg1);
        //}
    }

    public class JsInvokableAction<T, U>
    {
        private readonly Action<T, U> _delegate;

        public JsInvokableAction(Action<T, U> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public void Invoke2(T arg1, U arg2)
        {
            _delegate(arg1, arg2);
        }
    }
}
