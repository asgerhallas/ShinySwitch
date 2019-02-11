using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchExpression<TExpression> : SwitchExpression<Type, TExpression>
    {
        public SystemTypeSwitchExpression(Type subject, SwitchResult<TExpression> result) : base(subject, result) => SubjectTypeInfo = subject.GetTypeInfo();

        public TypeInfo SubjectTypeInfo { get; }

        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, TExpression> func) => MatchIf(typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo), () => func(Subject));
        public SystemTypeSwitchExpression<TExpression> Match<T>(Func<Type, bool> predicate, Func<Type, TExpression> func) => MatchIf(typeof (T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo) && predicate(Subject), () => func(Subject));

        public SystemTypeSwitchExpression<TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            Result.HasResult
                ? new SystemTypeSwitchExpression<TNewExpression>(Subject, new SwitchResult<TNewExpression>(func(Result.Result)))
                : new SystemTypeSwitchExpression<TNewExpression>(Subject, new SwitchResult<TNewExpression>());

        internal SystemTypeSwitchExpression<TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !Result.HasResult && predicate
                ? new SystemTypeSwitchExpression<TExpression>(Subject, new SwitchResult<TExpression>(func()))
                : this;
    }
}