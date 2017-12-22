using System;

namespace ShinySwitch
{
    public class TypeSwitchExpression2<TLeft, TRight, TExpression> : SwitchExpression<Tuple<TLeft, TRight>, TExpression>
    {
        internal TypeSwitchExpression2(TLeft left, TRight right, SwitchResult<TExpression> result)
            : base(Tuple.Create(left, right), result)
        {
        }

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, TExpression> func) =>
            MatchIf(subject.Item1 is TL && subject.Item2 is TR, () => func((TL)(object) subject.Item1, (TR)(object) subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TLeft left, TRight right, Func<TLeft, TRight, TExpression> func) =>
            MatchIf(Equals(subject.Item1, left) && Equals(subject.Item2, right), () => func(subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(Func<TL, TRight, TExpression> func) where TL : TLeft =>
            MatchIf(subject.Item1 is TL, () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(TRight right, Func<TL, TRight, TExpression> func) where TL : TLeft  =>
            MatchIf(subject.Item1 is TL && Equals(subject.Item2, right), () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(TLeft left, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Equals(subject.Item1, left) && subject.Item2 is TR, () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(subject.Item2 is TR, () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, bool> predicate, Func<TL, TR, TExpression> func) =>
            MatchIf(subject.Item1 is TL && subject.Item2 is TR && predicate((TL)(object)subject.Item1, (TR)(object)subject.Item2), () => func((TL)(object) subject.Item1, (TR)(object) subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TLeft left, TRight right, Func<bool> predicate, Func<TLeft, TRight, TExpression> func) =>
            MatchIf(Equals(subject.Item1, left) && Equals(subject.Item2, right) && predicate(), () => func(subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(Func<TL, bool> predicate, Func<TL, TRight, TExpression> func) where TL : TLeft =>
            MatchIf(subject.Item1 is TL && predicate((TL)subject.Item1), () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(TRight right, Func<TL, bool> predicate, Func<TL, TRight, TExpression> func) where TL : TLeft  =>
            MatchIf(subject.Item1 is TL && Equals(subject.Item2, right) && predicate((TL)subject.Item1), () => func((TL)subject.Item1, subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(TLeft left, Func<TR, bool> predicate, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Equals(subject.Item1, left) && subject.Item2 is TR && predicate((TR)subject.Item2), () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(Func<TR, bool> predicate, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(subject.Item2 is TR && predicate((TR)subject.Item2), () => func(subject.Item1, (TR)subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            result.HasResult
                ? new TypeSwitchExpression2<TLeft, TRight, TNewExpression>(subject.Item1, subject.Item2,
                    new SwitchResult<TNewExpression>(func(result.Result)))
                : new TypeSwitchExpression2<TLeft, TRight, TNewExpression>(subject.Item1, subject.Item2,
                    new SwitchResult<TNewExpression>());

        internal TypeSwitchExpression2<TLeft, TRight, TExpression> MatchIf(bool predicate, Func<TExpression> func) =>
            !result.HasResult && predicate
                ? new TypeSwitchExpression2<TLeft, TRight, TExpression>(
                    subject.Item1, subject.Item2,
                    new SwitchResult<TExpression>(func()))
                : this;
    }
}