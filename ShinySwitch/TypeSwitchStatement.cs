using System;

namespace ShinySwitch
{
    public class TypeSwitchStatement<TSubject> : SwitchStatement<TSubject>
    {
        public TypeSwitchStatement(TSubject subject, SwitchResult<bool> result) : base(subject, result) { }

        public TypeSwitchStatement<TSubject> Match<T>(Action<T> action) => MatchIf(Subject is T, () => action((T)(object)Subject));
        public TypeSwitchStatement<TSubject> Match(TSubject value, Action<TSubject> action) => Match(x => Equals(x, value), action);

        public TypeSwitchStatement<TSubject> Match<T>(Func<T, bool> predicate, Action<T> action) => MatchIf(Subject is T && predicate((T)(object)Subject), () => action((T)(object)Subject));
        public TypeSwitchStatement<TSubject> Match(TSubject value, Func<TSubject, bool> predicate, Action<TSubject> action) => Match(x => Equals(x, value) && predicate(x), action);

        public TypeSwitchStatement<TSubject> Then(Action<TSubject> action) => MatchIf(Result.HasResult, () => action(Subject));

        internal TypeSwitchStatement<TSubject> MatchIf(bool predicate, Action action)
        {
            if (predicate)
            {
                action();
                return new TypeSwitchStatement<TSubject>(Subject, new SwitchResult<bool>(true));
            }

            return this;
        }
    }
}