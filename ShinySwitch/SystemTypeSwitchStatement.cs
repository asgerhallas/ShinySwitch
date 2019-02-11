using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchStatement : SwitchStatement<Type>
    {
        public SystemTypeSwitchStatement(Type subject, SwitchResult<bool> result) : base(subject, result) => SubjectTypeInfo = subject.GetTypeInfo();

        public TypeInfo SubjectTypeInfo { get; }

        public SystemTypeSwitchStatement Match<T>(Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo), action);
        public SystemTypeSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo) && predicate(x), action);

        public SystemTypeSwitchStatement Then(Action<Type> action) => MatchIf(x => Result.HasResult, action);

        internal SystemTypeSwitchStatement MatchIf(Func<Type, bool> predicate, Action<Type> action)
        {
            if (predicate(Subject))
            {
                action(Subject);
                return new SystemTypeSwitchStatement(Subject, new SwitchResult<bool>(true));
            }

            return this;
        }
    }
}