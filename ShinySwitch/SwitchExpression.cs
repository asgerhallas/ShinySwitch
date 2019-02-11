using System;

namespace ShinySwitch
{
    public abstract class SwitchExpression<TSubject, TExpression>
    {
        protected SwitchExpression(TSubject subject, SwitchResult<TExpression> result)
        {
            Subject = subject;
            Result = result;
        }

        public TSubject Subject { get; }
        public SwitchResult<TExpression> Result { get; }

        public TExpression Else(TExpression value) => Result.HasResult ? Result.Result : value;
        public TExpression Else(Func<TExpression> func) => Result.HasResult ? Result.Result : func();

        public TExpression OrDefault() => Result.HasResult ? Result.Result : default(TExpression);

        public TExpression OrThrow(Exception exception = null) => OrThrow(() => exception);

        public TExpression OrThrow(Func<Exception> exception)
        {
            if (Result.HasResult)
            {
                return Result.Result;
            }

            throw exception() ?? new ArgumentOutOfRangeException();
        }

        public static implicit operator TExpression(SwitchExpression<TSubject, TExpression> expression) => expression.OrThrow();
    }
}