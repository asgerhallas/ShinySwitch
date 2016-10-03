using System;

namespace ShinySwitch
{
    public class SwitchExpression<TSubject, TReturn>
    {
        readonly TSubject subject;
        readonly SwitchResult<TReturn> result;

        internal SwitchExpression(TSubject subject, SwitchResult<TReturn> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public SwitchExpression<TSubject, TReturn> Match<T>(Func<T, TReturn> func) => Match(x => true, func);
        public SwitchExpression<TSubject, TReturn> Match<T>(bool predicate, Func<T, TReturn> func) => Match(x => predicate, func);
        public SwitchExpression<TSubject, TReturn> Match<T>(Func<T, bool> predicate, Func<T, TReturn> func)
        {
            return subject is T && predicate((T)(object)subject)
                ? new SwitchExpression<TSubject, TReturn>(subject, new SwitchResult<TReturn>(func((T)(object)subject)))
                : this;
        }

        public SwitchExpression<TSubject, TReturn> Match(TSubject value, Func<TSubject, TReturn> func) => Match(value, x => true, func);
        public SwitchExpression<TSubject, TReturn> Match(TSubject value, bool predicate, Func<TSubject, TReturn> func) => Match(value, x => predicate, func);
        public SwitchExpression<TSubject, TReturn> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TReturn> func)
        {
            return Equals(subject, value) && predicate(subject)
                ? new SwitchExpression<TSubject, TReturn>(subject, new SwitchResult<TReturn>(func(subject)))
                : this;
        }

        public SwitchExpression<TSubject, TReturn> Then(Func<TReturn, object, TReturn> func)
        {
            return result.HasResult 
                ? new SwitchExpression<TSubject, TReturn>(subject, new SwitchResult<TReturn>(func(result.Result, subject))) 
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