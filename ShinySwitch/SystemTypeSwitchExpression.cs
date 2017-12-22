using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TExpression> : SwitchExpression<Type, TExpression>
    {
        readonly TypeInfo subjectTypeInfo;

        internal SystemTypeSwitchExpression(Type subject, SwitchResult<TExpression> result) : base(subject, result) => subjectTypeInfo = subject.GetTypeInfo();

        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, TExpression> func) => MatchIf(typeof(T).GetTypeInfo().IsAssignableFrom(subjectTypeInfo), () => func(subject));
        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, bool> predicate, Func<Type, TExpression> func) => MatchIf(typeof (T).GetTypeInfo().IsAssignableFrom(subjectTypeInfo) && predicate(subject), () => func(subject));

        public SystemTypeSwitchExpression<TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            result.HasResult
                ? new SystemTypeSwitchExpression<TNewExpression>(subject, new SwitchResult<TNewExpression>(func(result.Result)))
                : new SystemTypeSwitchExpression<TNewExpression>(subject, new SwitchResult<TNewExpression>());

        internal SystemTypeSwitchExpression<TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !result.HasResult && predicate
                ? new SystemTypeSwitchExpression<TExpression>(subject, new SwitchResult<TExpression>(func()))
                : this;
    }
}