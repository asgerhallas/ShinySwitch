using System;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TReturn>
    {
        readonly Type subject;
        readonly SwitchResult<TReturn> result;

        internal SystemTypeSwitchExpression(Type subject, SwitchResult<TReturn> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public SystemTypeSwitchExpression<TReturn> Match<T>(Func<Type, TReturn> func) => Match<T>(x => true, func);
        public SystemTypeSwitchExpression<TReturn> Match<T>(bool predicate, Func<Type, TReturn> func) => Match<T>(x => predicate, func);

        public SystemTypeSwitchExpression<TReturn> Match<T>(Func<Type, bool> predicate, Func<Type, TReturn> func)
        {
            return typeof(T).IsAssignableFrom(subject) && predicate(subject)
                ? new SystemTypeSwitchExpression<TReturn>(subject, new SwitchResult<TReturn>(func(subject)))
                : this;
        }

        public SystemTypeSwitchExpression<TReturn> Then(Func<TReturn, object, TReturn> func)
        {
            return result.HasResult 
                ? new SystemTypeSwitchExpression<TReturn>(subject, new SwitchResult<TReturn>(func(result.Result, subject))) 
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