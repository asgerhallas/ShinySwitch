using System;

namespace ShinySwitch
{
    public class TypeSwitchStatement<TSubject> : SwitchStatement<TSubject>
    {
        public TypeSwitchStatement(TSubject subject, SwitchResult<bool> result) : base(subject, result) { }

        public TypeSwitchStatement<TSubject> Match<T>(Action<T> action) => MatchIf(action);
        public TypeSwitchStatement<TSubject> Match<T>(T value, Action<T> action) => Match(x => Equals(x, value), action);

        public TypeSwitchStatement<TSubject> Match<T>(Func<T, bool> predicate, Action<T> action) => MatchIf(predicate, action);
        public TypeSwitchStatement<TSubject> Match<T>(T value, Func<T, bool> predicate, Action<T> action) => Match(x => Equals(x, value) && predicate(x), action);

        public TypeSwitchStatement<TSubject> Then(Action<TSubject> action)
        {
            if (Result.HasResult)
            {
                action(Subject);
            }

            return this;
        }

        internal TypeSwitchStatement<TSubject> MatchIf<T>(Func<T, bool> predicate, Action<T> action)
        {
            if (!Result.HasResult && Subject is T t && predicate(t))
            {
                action(t);
                return new TypeSwitchStatement<TSubject>(Subject, new SwitchResult<bool>(true));
            }

            return this;
        }

        internal TypeSwitchStatement<TSubject> MatchIf<T>(Action<T> action)
        {
            if (!Result.HasResult && Subject is T t)
            {
                action(t);
                return new TypeSwitchStatement<TSubject>(Subject, new SwitchResult<bool>(true));
            }

            return this;
        }
    }
}