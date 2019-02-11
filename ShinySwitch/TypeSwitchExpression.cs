using System;

namespace ShinySwitch
{
    public class TypeSwitchExpression<TSubject, TExpression> : SwitchExpression<TSubject, TExpression>
    {
        public TypeSwitchExpression(TSubject subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, TExpression> func) => MatchIf(func);
        public TypeSwitchExpression<TSubject, TExpression> Match<T>(T value, Func<T, TExpression> func) => MatchIf(x => Equals(x, value), func);
        public TypeSwitchExpression<TSubject, TExpression> Match<T>(T value, TExpression returnValue) => MatchIf<TSubject>(x => Equals(x, value), _ => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, bool> predicate, Func<T, TExpression> func) => MatchIf(predicate, func);
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TExpression> func) => MatchIf(x => Equals(x, value) && predicate(x), func);
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, TExpression returnValue) => MatchIf<TSubject>(x => Equals(x, value) && predicate(x), _ => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> MatchNull(TExpression returnValue) => MatchIfNull(() => true, () => returnValue);
        public TypeSwitchExpression<TSubject, TExpression> MatchNull(Func<TExpression> func) => MatchIfNull(() => true, func);

        public TypeSwitchExpression<TSubject, TExpression> Match(Func<bool> predicate, Func<TSubject, TExpression> func) => MatchIf(_ => predicate(), func);

        public TypeSwitchExpression<TSubject, TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            Result.HasResult
                ? new TypeSwitchExpression<TSubject, TNewExpression>(Subject, new SwitchResult<TNewExpression>(func(Result.Result)))
                : new TypeSwitchExpression<TSubject, TNewExpression>(Subject, new SwitchResult<TNewExpression>());

        internal TypeSwitchExpression<TSubject, TExpression> MatchIf<T>(Func<T, bool> predicate, Func<T, TExpression> func) => 
            !Result.HasResult && Subject is T t && predicate(t)
                ? new TypeSwitchExpression<TSubject, TExpression>(Subject, new SwitchResult<TExpression>(func(t)))
                : this;

        internal TypeSwitchExpression<TSubject, TExpression> MatchIfNull(Func<bool> predicate, Func<TExpression> func) =>
            !Result.HasResult && ReferenceEquals(Subject, null) && predicate()
                ? new TypeSwitchExpression<TSubject, TExpression>(Subject, new SwitchResult<TExpression>(func()))
                : this;

        internal TypeSwitchExpression<TSubject, TExpression> MatchIf<T>(Func<T, TExpression> func) => 
            !Result.HasResult && Subject is T t
                ? new TypeSwitchExpression<TSubject, TExpression>(Subject, new SwitchResult<TExpression>(func(t)))
                : this;
    }
}