using System;

namespace ShinySwitch
{
    public class TypeSwitchExpression<TSubject, TExpression> : SwitchExpression<TSubject, TExpression>
    {
        internal TypeSwitchExpression(TSubject subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, TExpression> func) where T : TSubject => MatchIf(subject is T, () => func((T)subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, TExpression> func) => MatchIf(Equals(subject, value), () => func(subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, TExpression returnValue) => MatchIf(Equals(subject, value), () => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, bool> predicate, Func<T, TExpression> func) where T : TSubject => MatchIf(subject is T && predicate((T) subject), () => func((T)subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TExpression> func) => MatchIf(Equals(subject, value) && predicate(subject), () => func(subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, TExpression returnValue) => MatchIf(Equals(subject, value) && predicate(subject), () => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Then(Func<TExpression, TSubject, TExpression> func) =>
            result.HasResult
                ? new TypeSwitchExpression<TSubject, TExpression>(subject,
                    new SwitchResult<TExpression>(func(result.Result, subject)))
                : this;

        internal TypeSwitchExpression<TSubject, TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !result.HasResult && predicate
            ? new TypeSwitchExpression<TSubject, TExpression>(subject,
                new SwitchResult<TExpression>(func()))
            : this;
    }

    public class TypeSwitchExpression2<TLeft, TRight, TExpression> : SwitchExpression<Tuple<TLeft, TRight>, TExpression>
    {
        internal TypeSwitchExpression2(TLeft left, TRight right, SwitchResult<TExpression> result)
            : base(Tuple.Create(left, right), result)
        {
        }

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, TExpression> func) where TL : TLeft where TR : TRight =>
            MatchIf(() => subject.Item1 is TL && subject.Item2 is TR, () => func((TL) subject.Item1, (TR) subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TLeft left, TRight right, Func<TLeft, TRight, TExpression> func) =>
            MatchIf(() => Equals(subject.Item1, left) && Equals(subject.Item2, right), () => func(subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(Func<TL, TRight, TExpression> func) where TL : TLeft =>
            MatchIf(() => subject.Item1 is TL, () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(TRight right, Func<TL, TRight, TExpression> func) where TL : TLeft  =>
            MatchIf(() => subject.Item1 is TL && Equals(subject.Item2, right), () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(TLeft left, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(() => Equals(subject.Item1, left) && subject.Item2 is TR, () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(() => subject.Item2 is TR, () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Then(Func<TExpression, TLeft, TRight, TExpression> func) =>
            result.HasResult
                ? new TypeSwitchExpression2<TLeft, TRight, TExpression>(
                    subject.Item1, subject.Item2,
                    new SwitchResult<TExpression>(func(result.Result, subject.Item1, subject.Item2)))
                : this;

        internal TypeSwitchExpression2<TLeft, TRight, TExpression> MatchIf(Func<bool> predicate, Func<TExpression> func) =>
            !result.HasResult && predicate()
                ? new TypeSwitchExpression2<TLeft, TRight, TExpression>(
                    subject.Item1, subject.Item2,
                    new SwitchResult<TExpression>(func()))
                : this;
    }
}