using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class JSInvokableAction
    {
        private readonly Action _delegate;

        public JSInvokableAction(Action @delegate)
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

    public class JSInvokableAsyncAction<TParam>
    {
        private readonly Func<TParam, Task> _delegate;

        public JSInvokableAsyncAction(Func<TParam, Task> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public Task Invoke1(TParam arg1)
        {
            return _delegate(arg1);
        }

        [JSInvokable]
        public void Invoke2(TParam arg1)
        {
            _delegate(arg1);
        }
    }

    public class JSInvokableAction<TParam1, TParam2>
    {
        private readonly Action<TParam1, TParam2> _delegate;

        public JSInvokableAction(Action<TParam1, TParam2> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public void Invoke2(TParam1 arg1, TParam2 arg2)
        {
            _delegate(arg1, arg2);
        }
    }

    public class JSInvokableAsyncAction
    {
        private readonly Func<Task> _delegate;

        public JSInvokableAsyncAction(Func<Task> @delegate)
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

    public class JSInvokableFunc<TParam, TRet>
    {
        private readonly Func<TParam, TRet> _delegate;

        public JSInvokableFunc(Func<TParam, TRet> @delegate)
        {
            _delegate = @delegate;
        }

        [JSInvokable]
        public TRet Invoke1(TParam arg1)
        {
            return _delegate(arg1);
        }
    }

    public class JSInvokableAsyncFunc<TParam, TRet>
    {
        private readonly Func<TParam, Task<TRet>> _delegate;

        public JSInvokableAsyncFunc(Func<TParam, Task<TRet>> @delegate)
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
}
