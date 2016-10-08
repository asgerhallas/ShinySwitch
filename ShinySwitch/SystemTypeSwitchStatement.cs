using System;

namespace ShinySwitch
{
    public class SystemTypeSwitchStatement : SwitchStatement<Type>
    {
        internal SystemTypeSwitchStatement(Type subject, SwitchResult<bool> result) : base(subject, result) {}

        public SystemTypeSwitchStatement Match<T>(Action<Type> action) => MatchInternal(x => typeof(T).IsAssignableFrom(subject), action);
        public SystemTypeSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action) => MatchInternal(x => typeof(T).IsAssignableFrom(subject) && predicate(x), action);

        public SystemTypeSwitchStatement Then(Action<Type> action) => MatchInternal(x => result.HasResult, action);

        public SystemTypeSwitchStatement MatchInternal(Func<Type, bool> predicate, Action<Type> action)
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