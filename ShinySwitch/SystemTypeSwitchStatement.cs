using System;

namespace ShinySwitch
{
    public class SystemTypeSwitchStatement
    {
        readonly Type subject;
        readonly bool match;

        public SystemTypeSwitchStatement(Type subject, bool match)
        {
            this.subject = subject;
            this.match = match;
        }

        public SystemTypeSwitchStatement Match<T>(Action<Type> action)
        {
            return Match<T>(x => true, action);
        }

        public SystemTypeSwitchStatement Match<T>(bool predicate, Action<Type> action)
        {
            return Match<T>(x => predicate, action);
        }

        public SystemTypeSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action)
        {
            if (typeof(T).IsAssignableFrom(subject) && predicate(subject))
            {
                action(subject);
                return new SystemTypeSwitchStatement(subject, true);
            }

            return this;
        }

        public SystemTypeSwitchStatement Then(Action<Type> action)
        {
            if (match)
            {
                action(subject);
            }

            return this;
        }

        public void Else(Action<Type> action)
        {
            if (match) return;

            action(subject);
        }

        public void OrThrow(Exception exception)
        {
            if (match) return;

            throw exception;
        }
    }
}