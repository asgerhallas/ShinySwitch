using System;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TExpression> : SwitchExpression<Type, TExpression>
    {
        internal SystemTypeSwitchExpression(Type subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, TExpression> func) => MatchInternal(x => typeof(T).IsAssignableFrom(subject), func);
        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, bool> predicate, Func<Type, TExpression> func) => MatchInternal(x => typeof (T).IsAssignableFrom(subject) && predicate(x), func);
        public SystemTypeSwitchExpression<TExpression> Then(Func<TExpression, Type, TExpression> func) => MatchInternal(x => result.HasResult, x => func(result.Result, x));

        public SystemTypeSwitchExpression<TExpression> MatchInternal(Func<Type, bool> predicate, Func<Type, TExpression> func)
        {
            return predicate(subject)
                ? new SystemTypeSwitchExpression<TExpression>(subject, 
                    new SwitchResult<TExpression>(func(subject)))
                : this;
        }
    }
}