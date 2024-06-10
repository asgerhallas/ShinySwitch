using System;

namespace ShinySwitch
{
    public abstract class SwitchExpression<TSubject, TExpression>(TSubject subject, MatchResult<TExpression> result)
    {
        public TSubject Subject { get; } = subject;
        public MatchResult<TExpression> Result { get; } = result;

        public TExpression Else(TExpression value) => Result.HasMatch ? Result.Value : value;
        public TExpression Else(Func<TExpression> func) => Result.HasMatch ? Result.Value : func();

        public TExpression OrDefault() => Result.HasMatch ? Result.Value : default;

        public TExpression OrThrow(Exception exception = null) =>
            Result.HasMatch
                ? Result.Value
                : throw (exception ?? new ArgumentOutOfRangeException());

        public TExpression OrThrow(Func<Exception> exception) => 
            Result.HasMatch 
                ? Result.Value 
                : throw (exception() ?? new ArgumentOutOfRangeException());

        public static implicit operator TExpression(SwitchExpression<TSubject, TExpression> expression) => expression.OrThrow();
    }
}