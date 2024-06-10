using System;
using System.Reflection;

namespace ShinySwitch
{
    public class SystemTypeSwitchStatement(Type subject, MatchResult<bool> result, bool matchMany)
        : SwitchStatement<Type>(subject, result, matchMany)
    {
        public TypeInfo SubjectTypeInfo { get; } = subject.GetTypeInfo();

        public SystemTypeSwitchStatement Match<T>(Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo), action);
        public SystemTypeSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action) => MatchIf(x => typeof(T).GetTypeInfo().IsAssignableFrom(SubjectTypeInfo) && predicate(x), action);

        public SystemTypeSwitchStatement Then(Action<Type> action)
        {
            if (Result.HasMatch)
            {
                action(Subject);

                return new SystemTypeSwitchStatement(Subject, new MatchResult<bool>(true), MatchMany);
            }

            return this;
        }

        internal SystemTypeSwitchStatement MatchIf(Func<Type, bool> predicate, Action<Type> action)
        {
            if ((!Result.HasMatch || MatchMany) && predicate(Subject))
            {
                action(Subject);

                return new SystemTypeSwitchStatement(Subject, new MatchResult<bool>(true), MatchMany);
            }

            return this;

        }
    }
}