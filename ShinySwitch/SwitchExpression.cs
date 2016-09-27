﻿using System;

namespace ShinySwitch
{
    public class SwitchExpression<TReturn>
    {
        readonly object subject;
        readonly SwitchResult<TReturn> result;

        internal SwitchExpression(object subject, SwitchResult<TReturn> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public SwitchExpression<TReturn> Match<T>(Func<T, TReturn> func) => Match(x => true, func);
        public SwitchExpression<TReturn> Match<T>(bool predicate, Func<T, TReturn> func) => Match(x => predicate, func);

        public SwitchExpression<TReturn> Match<T>(Func<T, bool> predicate, Func<T, TReturn> func)
        {
            return subject is T && predicate((T) subject)
                ? new SwitchExpression<TReturn>(subject, new SwitchResult<TReturn>(func((T) subject)))
                : this;
        }

        public SwitchExpression<TReturn> Then(Func<TReturn, object, TReturn> func)
        {
            return result.HasResult 
                ? new SwitchExpression<TReturn>(subject, new SwitchResult<TReturn>(func(result.Result, subject))) 
                : this;
        }

        public TReturn Else(TReturn value)
        {
            return result.HasResult
                ? result.Result
                : value;
        }

        public TReturn Else(Func<TReturn> func)
        {
            return result.HasResult
                ? result.Result
                : func();
        }

        public TReturn OrThrow(Exception exception = null)
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            throw exception ?? new ArgumentOutOfRangeException();
        }

        public TReturn OrDefault()
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            return default(TReturn);
        }
    }
}