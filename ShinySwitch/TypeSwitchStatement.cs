using System;

namespace ShinySwitch
{
    public class TypeSwitchStatement<TSubject>(TSubject subject, MatchResult<bool> result, bool matchMany)
        : SwitchStatement<TSubject>(subject, result, matchMany)
    {
        public TypeSwitchStatement<TSubject> Match<T>(Action<T> action) => MatchIf(action);
        public TypeSwitchStatement<TSubject> Match<T>(T value, Action<T> action) => Match(x => Equals(x, value), action);

        public TypeSwitchStatement<TSubject> Match<T>(Func<T, bool> predicate, Action<T> action) => MatchIf(predicate, action);
        public TypeSwitchStatement<TSubject> Match<T>(T value, Func<T, bool> predicate, Action<T> action) => Match(x => Equals(x, value) && predicate(x), action);

        public TypeSwitchStatement<TSubject> Then(Action<TSubject> action)
        {
            if (Result.HasMatch)
            {
                action(Subject);
            }

            return this;
        }

        internal TypeSwitchStatement<TSubject> MatchIf<T>(Func<T, bool> predicate, Action<T> action)
        {
            if ((!Result.HasMatch || MatchMany) && Subject is T t && predicate(t))
            {
                action(t);
                return new TypeSwitchStatement<TSubject>(Subject, new MatchResult<bool>(true), MatchMany);
            }

            return this;
        }

        internal TypeSwitchStatement<TSubject> MatchIf<T>(Action<T> action)
        {
            if ((!Result.HasMatch || MatchMany) && Subject is T t)
            {
                action(t);
                return new TypeSwitchStatement<TSubject>(Subject, new MatchResult<bool>(true), MatchMany);
            }

            return this;
        }
    }
}