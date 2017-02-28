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

        public ValueSwitchExpression<TValue, TReturn> Match(TValue prototype, Func<TValue, bool> predicate, Func<TValue, TReturn> func) => 
            Equals(subject, prototype) && predicate(subject)
                ? new ValueSwitchExpression<TValue, TReturn>(subject, new SwitchResult<TReturn>(func(subject)))
                : this;

        public ValueSwitchExpression<TValue, TReturn> Match(bool predicate, Func<TValue, TReturn> func) => 
            predicate
                ? new ValueSwitchExpression<TValue, TReturn>(subject, new SwitchResult<TReturn>(func(subject)))
                : this;


        public ValueSwitchExpression<TValue, TNewReturn> Then<TNewReturn>(Func<TReturn, object, TNewReturn> func)
        {
            return result.HasResult
                ? new ValueSwitchExpression<TValue, TNewReturn>(subject, new SwitchResult<TNewReturn>(func(result.Result, subject)))
                : new ValueSwitchExpression<TValue, TNewReturn>(subject, new SwitchResult<TNewReturn>());
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