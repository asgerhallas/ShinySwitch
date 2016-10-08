using System;

namespace ShinySwitch
{
    public abstract class SwitchExpression<TSubject, TExpression>
    {
        protected readonly TSubject subject;
        protected readonly SwitchResult<TExpression> result;

        protected SwitchExpression(TSubject subject, SwitchResult<TExpression> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public TExpression Else(TExpression value) => result.HasResult ? result.Result : value;
        public TExpression Else(Func<TExpression> func) => result.HasResult ? result.Result : func();

        public TExpression OrDefault() => result.HasResult ? result.Result : default(TExpression);

        public TExpression OrThrow(Exception exception = null) => OrThrow(() => exception);

        public TExpression OrThrow(Func<Exception> exception)
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            throw exception() ?? new ArgumentOutOfRangeException();
        }

        public static implicit operator TExpression(SwitchExpression<TSubject, TExpression> expression)
        {
            return expression.OrThrow();
        }
    }

    //public class TypeSwitchExpression2<TSubject, TExpression> : SwitchExpression<TSubject, TExpression>
    //{
    //    readonly TSubject left;

    //    internal TypeSwitchExpression2(TSubject left, TSubject right, SwitchResult<TExpression> result) : base(subject, result)
    //    {
    //        this.left = left;
    //    }

    //    public TypeSwitchExpression2<TSubject, TExpression> MatchLeft<TLeft>(Func<TLeft, TExpression> func) => 
    //        new TypeSwitchExpression2<TSubject, TExpression>(subject, Match(
    //            new SubjectIsType<TLeft, TExpression>(() => func((TLeft)left))));


    //    public TypeSwitchExpression<TSubject, TReturn> Match<T>(Func<T, bool> predicate, Func<T, TReturn> func)
    //    {
    //        return subject is T && predicate((T)(object)subject)
    //            ? new TypeSwitchExpression<TSubject, TReturn>(subject, new SwitchResult<TReturn>(func((T)(object)subject)))
    //            : this;
    //    }
    //}



    public class TypeSwitchExpression<TSubject, TExpression> : SwitchExpression<TSubject, TExpression>
    {
        internal TypeSwitchExpression(TSubject subject, SwitchResult<TExpression> result) : base(subject, result) { }

        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, TExpression> func) where T : TSubject => MatchInternal(x => subject is T, func);
        public TypeSwitchExpression<TSubject, TExpression> Match<T>(Func<T, bool> predicate, Func<T, TExpression> func) where T : TSubject => MatchInternal(x => subject is T && predicate((T) x), func);

        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, TExpression> func) => MatchInternal(x => Equals(x, value), func);
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TExpression> func) => MatchInternal(x => Equals(x, value) && predicate(x), func);
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, TExpression returnValue) => MatchInternal<TSubject>(x => Equals(x, value), x => returnValue);
        public TypeSwitchExpression<TSubject, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, TExpression returnValue) => MatchInternal<TSubject>(x => Equals(x, value) && predicate(x), x => returnValue);

        public TypeSwitchExpression<TSubject, TExpression> Then(Func<TExpression, TSubject, TExpression> func) => MatchInternal<TSubject>(_ => result.HasResult, x => func(result.Result, x));

        public TypeSwitchExpression<TSubject, TExpression> MatchInternal<T>(Func<TSubject, bool> predicate, Func<T, TExpression> func) where T : TSubject
        {
            return predicate(subject)
                ? new TypeSwitchExpression<TSubject, TExpression>(subject,
                    new SwitchResult<TExpression>(func((T)subject)))
                : this;
        }
    }

    public class TypeSwitchExpression2<TLeft, TRight, TExpression> : SwitchExpression<Tuple<TLeft, TRight>, TExpression>
    {
        internal TypeSwitchExpression2(TLeft left, TRight right, SwitchResult<TExpression> result)
            : base(Tuple.Create(left, right), result)
        {
        }

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, TExpression> func) where TL : TLeft where TR : TRight =>
            MatchInternal(
                (l, r) => subject.Item1 is TL && subject.Item2 is TR,
                x => func((TL) x.Item1, (TR) x.Item2));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> Match<TL, TR>(Func<TL, TR, bool> predicate, Func<TL, TR, TExpression> func) where TL : TLeft where TR : TRight =>
            MatchInternal(
                (l, r) =>
                    subject.Item1 is TL && subject.Item2 is TR 
                    && predicate((TL) subject.Item1, (TR) subject.Item2),
                x => func((TL) x.Item1, (TR) x.Item2));

        //public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TSubject value, Func<TSubject, TExpression> func) => MatchInternal(x => Equals(x, value), func);
        //public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, Func<TSubject, TExpression> func) => MatchInternal(x => Equals(x, value) && predicate(x), func);
        //public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TSubject value, TExpression returnValue) => MatchInternal<TSubject>(x => Equals(x, value), x => returnValue);
        //public TypeSwitchExpression2<TLeft, TRight, TExpression> Match(TSubject value, Func<TSubject, bool> predicate, TExpression returnValue) => MatchInternal<TSubject>(x => Equals(x, value) && predicate(x), x => returnValue);

        //public TypeSwitchExpression2<TLeft, TRight, TExpression> Then(Func<TExpression, TLeft, TRight, TExpression> func)
        //    => MatchInternal<Tuple<TLeft, TRight>>(_ => result.HasResult, x => func(result.Result, x));

        public TypeSwitchExpression2<TLeft, TRight, TExpression> MatchInternal(Func<TLeft, TRight, bool> predicate, Func<Tuple<TLeft, TRight>, TExpression> func)
        {
            return predicate(subject.Item1, subject.Item2)
                ? new TypeSwitchExpression2<TLeft, TRight, TExpression>(subject.Item1, subject.Item2,
                    new SwitchResult<TExpression>(func(subject)))
                : this;
        }
    }
}