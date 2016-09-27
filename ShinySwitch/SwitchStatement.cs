using System;

namespace ShinySwitch
{
    public class SwitchStatement
    {
        readonly object subject;
        readonly bool match;

        public SwitchStatement(object subject, bool match)
        {
            this.subject = subject;
            this.match = match;
        }

        public SwitchStatement Match<T>(Action<T> action)
        {
            return Match(x => true, action);
        }

        public SwitchStatement Match<T>(bool predicate, Action<T> action)
        {
            return Match(x => predicate, action);
        }

        public SwitchStatement Match<T>(Func<T, bool> predicate, Action<T> action)
        {
            if (subject is T && predicate((T)subject))
            {
                action((T)subject);
                return new SwitchStatement(subject, true);
            }

            return this;
        }

        public SwitchStatement Then(Action<object> action)
        {
            if (match)
            {
                action(subject);
            }

            return this;
        }

        public void Else(Action<object> action)
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