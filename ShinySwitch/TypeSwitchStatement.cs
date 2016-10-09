using System;

namespace ShinySwitch
{
    public class TypeSwitchStatement<TSubject> : SwitchStatement<TSubject>
    {
        internal TypeSwitchStatement(TSubject subject, SwitchResult<bool> result) : base(subject, result) { }

        public TypeSwitchStatement<TSubject> Match<T>(Action<T> action) where T : TSubject => MatchIf(x => subject is T, action);
        public TypeSwitchStatement<TSubject> Match(TSubject value, Action<TSubject> action) => Match(x => Equals(x, value), action);

        public TypeSwitchStatement<TSubject> Match<T>(Func<T, bool> predicate, Action<T> action) where T : TSubject => MatchIf(x => subject is T && predicate((T)x), action);
        public TypeSwitchStatement<TSubject> Match(TSubject value, Func<TSubject, bool> predicate, Action<TSubject> action) => Match(x => Equals(x, value) && predicate(x), action);

        public TypeSwitchStatement<TSubject> Then(Action<TSubject> action) => MatchIf(x => result.HasResult, action);

        internal TypeSwitchStatement<TSubject> MatchIf<T>(Func<TSubject, bool> predicate, Action<T> action) where T:TSubject
        {
            if (predicate(subject))
            {
                action((T)subject);
                return new TypeSwitchStatement<TSubject>(subject, new SwitchResult<bool>(true));
            }

            return this;
        }
    }
}