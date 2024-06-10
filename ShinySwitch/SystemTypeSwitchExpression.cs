using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TExpression>(Type subject, MatchResult<TExpression> result)
        : SwitchExpression<Type, TExpression>(subject, result)
    {
        public TypeInfo SubjectTypeInfo { get; } = subject.GetTypeInfo();

        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, TExpression> func) => MatchIf(typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo), () => func(Subject));
        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, bool> predicate, Func<Type, TExpression> func) => MatchIf(typeof (T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo) && predicate(Subject), () => func(Subject));

        public SystemTypeSwitchExpression<TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            Result.HasMatch
                ? new SystemTypeSwitchExpression<TNewExpression>(Subject, new MatchResult<TNewExpression>(func(Result.Value)))
                : new SystemTypeSwitchExpression<TNewExpression>(Subject, new MatchResult<TNewExpression>());

        internal SystemTypeSwitchExpression<TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !Result.HasMatch && predicate
                ? new SystemTypeSwitchExpression<TExpression>(Subject, new MatchResult<TExpression>(func()))
                : this;
    }
}