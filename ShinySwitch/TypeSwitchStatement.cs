using System;

namespace ShinySwitch
{
    public class TypeSwitchStatement<TSubject>
    {
        readonly TSubject subject;
        readonly bool match;

        public TypeSwitchStatement(TSubject subject, bool match)
        {
            this.subject = subject;
            this.match = match;
        }

        public TypeSwitchStatement<TSubject> Match<T>(Action<T> action) => Match(x => true, action);
        public TypeSwitchStatement<TSubject> Match<T>(bool predicate, Action<T> action) => Match(x => predicate, action);
        public TypeSwitchStatement<TSubject> Match<T>(Func<T, bool> predicate, Action<T> action)
        {
            if (subject is T)
            {
                var t = (T) (object) subject;

                if (predicate(t))
                {
                    action(t);
                    return new TypeSwitchStatement<TSubject>(subject, true);
                }
            }

            return this;
        }

        public TypeSwitchStatement<TSubject> Match(TSubject value, Action<TSubject> action) => Match(value, x => true, action);
        public TypeSwitchStatement<TSubject> Match(TSubject value, bool predicate, Action<TSubject> action) => Match(value, x => predicate, action);
        public TypeSwitchStatement<TSubject> Match(TSubject value, Func<TSubject, bool> predicate, Action<TSubject> action)
        {
            if (Equals(subject, value))
            {
                if (predicate(subject))
                {
                    action(subject);
                    return new TypeSwitchStatement<TSubject>(subject, true);
                }
            }

            return this;
        }

        public TypeSwitchStatement<TSubject> Then(Action<object> action)
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