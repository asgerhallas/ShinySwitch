using System;

namespace ShinySwitch
{
    public class TypeSwitchExpression2<TLeft, TRight, TExpression> : SwitchExpression<Tuple<TLeft, TRight>, TExpression>
    {
        public TypeSwitchExpression2(TLeft left, TRight right, SwitchResult<TExpression> result) 
            : base(Tuple.Create(left, right), result) { }

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, TExpression> func) =>
            MatchIf(Subject.Item1 is TL && Subject.Item2 is TR, () => func((TL)(object) Subject.Item1, (TR)(object) Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TLeft left, TRight right, Func<TLeft, TRight, TExpression> func) =>
            MatchIf(Equals(Subject.Item1, left) && Equals(Subject.Item2, right), () => func(Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(Func<TL, TRight, TExpression> func) where TL : TLeft =>
            MatchIf(Subject.Item1 is TL, () => func((TL)Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(TRight right, Func<TL, TRight, TExpression> func) where TL : TLeft  =>
            MatchIf(Subject.Item1 is TL && Equals(Subject.Item2, right), () => func((TL)Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(TLeft left, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Equals(Subject.Item1, left) && Subject.Item2 is TR, () => func(Subject.Item1, (TR)Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Subject.Item2 is TR, () => func(Subject.Item1, (TR)Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, bool> predicate, Func<TL, TR, TExpression> func) =>
            MatchIf(Subject.Item1 is TL && Subject.Item2 is TR && predicate((TL)(object)Subject.Item1, (TR)(object)Subject.Item2), () => func((TL)(object) Subject.Item1, (TR)(object) Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TLeft left, TRight right, Func<bool> predicate, Func<TLeft, TRight, TExpression> func) =>
            MatchIf(Equals(Subject.Item1, left) && Equals(Subject.Item2, right) && predicate(), () => func(Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(Func<TL, bool> predicate, Func<TL, TRight, TExpression> func) where TL : TLeft =>
            MatchIf(Subject.Item1 is TL && predicate((TL)Subject.Item1), () => func((TL)Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchLeft<TL>(TRight right, Func<TL, bool> predicate, Func<TL, TRight, TExpression> func) where TL : TLeft  =>
            MatchIf(Subject.Item1 is TL && Equals(Subject.Item2, right) && predicate((TL)Subject.Item1), () => func((TL)Subject.Item1, Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(TLeft left, Func<TR, bool> predicate, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Equals(Subject.Item1, left) && Subject.Item2 is TR && predicate((TR)Subject.Item2), () => func(Subject.Item1, (TR)Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchRight<TR>(Func<TR, bool> predicate, Func<TLeft, TR, TExpression> func) where TR : TRight =>
            MatchIf(Subject.Item2 is TR && predicate((TR)Subject.Item2), () => func(Subject.Item1, (TR)Subject.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            Result.HasResult
                ? new TypeSwitchExpression2<TLeft, TRight, TNewExpression>(Subject.Item1, Subject.Item2,
                    new SwitchResult<TNewExpression>(func(Result.Result)))
                : new TypeSwitchExpression2<TLeft, TRight, TNewExpression>(Subject.Item1, Subject.Item2,
                    new SwitchResult<TNewExpression>());

        internal TypeSwitchExpression2<TLeft, TRight, TExpression> MatchIf(bool predicate, Func<TExpression> func) =>
            !Result.HasResult && predicate
                ? new TypeSwitchExpression2<TLeft, TRight, TExpression>(
                    Subject.Item1, Subject.Item2,
                    new SwitchResult<TExpression>(func()))
                : this;
    }
}