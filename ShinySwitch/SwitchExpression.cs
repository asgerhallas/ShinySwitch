using System;

namespace ShinySwitch
{
    public abstract class SwitchExpression<TSubject, TExpression>
    {
        protected readonly TSubject subject;
        protected readonly SwitchResult<TExpression> result;

        protected SwitchExpression(TSubject subject, SwitchResult<TExpression> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public TExpression Else(TExpression value) => result.HasResult ? result.Result : value;
        public TExpression Else(Func<TExpression> func) => result.HasResult ? result.Result : func();

        public TExpression OrDefault() => result.HasResult ? result.Result : default(TExpression);

        public TExpression OrThrow(Exception exception = null) => OrThrow(() => exception);

        public TExpression OrThrow(Func<Exception> exception)
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            throw exception() ?? new ArgumentOutOfRangeException();
        }

        public static implicit operator TExpression(SwitchExpression<TSubject, TExpression> expression)
        {
            return expression.OrThrow();
        }
    }
}