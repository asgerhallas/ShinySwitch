using System;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TExpression> : SwitchExpression<Type, TExpression>
    {
        internal SystemTypeSwitchExpression(Type subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, TExpression> func) => MatchIf(typeof(T).IsAssignableFrom(subject), () => func(subject));
        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, bool> predicate, Func<Type, TExpression> func) => MatchIf(typeof (T).IsAssignableFrom(subject) && predicate(subject), () => func(subject));

        public SystemTypeSwitchExpression<TExpression> Then(Func<TExpression, Type, TExpression> func) =>
            result.HasResult
                ? new SystemTypeSwitchExpression<TExpression>(subject,
                    new SwitchResult<TExpression>(func(result.Result, subject)))
                : this;

        internal SystemTypeSwitchExpression<TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !result.HasResult && predicate
                ? new SystemTypeSwitchExpression<TExpression>(subject, 
                    new SwitchResult<TExpression>(func()))
                : this;
    }
}