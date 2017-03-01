using System;

namespace ShinySwitch
{
    //todo: should there be different switches if the subject is a value type (so we should not cast to object)
    //or should that be avoided by a deliberte .MatchInterface<T> method? Or should it be avoided at all?
    //the match method it self could also check - but that would only be ok if the switch is cached eg lazy... is it a good idea, or should it be kept simpler?

    //todo: make it possible to seperate build of matches and execution, so it could be intercepted to build eg a visitor where steps can be intercepted
    //it could also be intercepted when eager ... hmm..

    //if lazy then either build _and_ execution is done with extension methods, for same syntax as now (to be able to give value upfront and carry it along until execution)
    //or a build that you pass to the executor like: Switch<string>.On(value, Switch.Match<>().Else(x)) and behind the scenes: Switch.Match<>().Else(x).Execute(value)

    public class TypeSwitchExpression<TSubject, TExpression> : SwitchExpression<TSubject, TExpression>
    {
        internal TypeSwitchExpression(TSubject subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, TExpression> func) => MatchIf(subject is T, () => func((T)(object)subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, TExpression> func) => MatchIf(Equals(subject, value), () => func(subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, TExpression returnValue) => MatchIf(Equals(subject, value), () => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, bool> predicate, Func<T, TExpression> func) => MatchIf(subject is T && predicate((T)(object)subject), () => func((T)(object)subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TExpression> func) => MatchIf(Equals(subject, value) && predicate(subject), () => func(subject));
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, TExpression returnValue) => MatchIf(Equals(subject, value) && predicate(subject), () => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Match(bool predicate, Func<TSubject, TExpression> func) => MatchIf(predicate, () => func(subject));

        public TypeSwitchExpression<TSubject, TNewExpression> Then<TNewExpression>(Func<TExpression, TNewExpression> func) =>
            result.HasResult
                ? new TypeSwitchExpression<TSubject, TNewExpression>(subject, new SwitchResult<TNewExpression>(func(result.Result)))
                : new TypeSwitchExpression<TSubject, TNewExpression>(subject, new SwitchResult<TNewExpression>());

        internal TypeSwitchExpression<TSubject, TExpression> MatchIf(bool predicate, Func<TExpression> func) => 
            !result.HasResult && predicate
            ? new TypeSwitchExpression<TSubject, TExpression>(subject, new SwitchResult<TExpression>(func()))
            : this;
    }

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