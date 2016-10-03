using System;

namespace ShinySwitch
{
    public class ValueSwitchExpression<TValue, TReturn>
    {
        readonly TValue subject;
        readonly SwitchResult<TReturn> result;

        internal ValueSwitchExpression(TValue subject, SwitchResult<TReturn> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public ValueSwitchExpression<TValue, TReturn> Match(TValue prototype, Func<TValue, TReturn> func) => Match(prototype, _ => true, func);
        public ValueSwitchExpression<TValue, TReturn> Match(TValue prototype, bool predicate, Func<TValue, TReturn> func) => Match(prototype, _ => predicate, func);

        public ValueSwitchExpression<TValue, TReturn> Match(TValue prototype, Func<TValue, bool> predicate, Func<TValue, TReturn> func)
        {
            return Equals(subject, prototype) && predicate(subject)
                ? new ValueSwitchExpression<TValue, TReturn>(subject, new SwitchResult<TReturn>(func(subject)))
                : this;
        }

        public ValueSwitchExpression<TValue, TReturn> Then(Func<TReturn, object, TReturn> func)
        {
            return result.HasResult
                ? new ValueSwitchExpression<TValue, TReturn>(subject, new SwitchResult<TReturn>(func(result.Result, subject)))
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