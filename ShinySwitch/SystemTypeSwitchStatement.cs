using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchStatement : SwitchStatement<Type>
    {
        readonly TypeInfo subjectTypeInfo;

        internal SystemTypeSwitchStatement(Type subject, SwitchResult<bool> result) : base(subject, result)
        {
            subjectTypeInfo = subject.GetTypeInfo();
        }

        public SystemTypeSwitchStatement Match<T>(Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(subjectTypeInfo), action);
        public SystemTypeSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(subjectTypeInfo) && predicate(x), action);

        public SystemTypeSwitchStatement Then(Action<Type> action) => MatchIf(x => result.HasResult, action);

        internal SystemTypeSwitchStatement MatchIf(Func<Type, bool> predicate, Action<Type> action)
        {
            if (predicate(subject))
            {
                action(subject);
                return new SystemTypeSwitchStatement(subject, new SwitchResult<bool>(true));
            }

            return this;
        }
    }
}