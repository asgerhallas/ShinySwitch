using System;

namespace ShinySwitch
{
    public class Switch
    {
        public static SwitchStatement OnTypeOf(object subject) => new SwitchStatement(subject);
        public static ReflectionSwitchStatement On(Type subject) => new ReflectionSwitchStatement(subject);
        public static SwitchExpression<TReturn> OnTypeOf<TReturn>(object subject) => new SwitchExpression<TReturn>(subject);
    }

    public class SwitchStatement
    {
        readonly object subject;

        bool match;

        public SwitchStatement(object subject)
        {
            this.subject = subject;
        }

        public SwitchStatement Match<T>(Action<T> action)
        {
            return Match(x => true, action);
        }

        public SwitchStatement Match<T>(bool predicate, Action<T> action)
        {
            return Match(x => predicate, action);
        }

        public SwitchStatement Match<T>(Func<T, bool> predicate, Action<T> action)
        {
            if (subject is T && predicate((T)subject))
            {
                match = true;
                action((T)subject);
            }

            return this;
        }

        public SwitchStatement Then(Action<object> action)
        {
            if (match)
            {
                action(subject);
            }

            return this;
        }

        public void Else(Action<object> action)
        {
            if (match) return;

            match = true;
            action(subject);
        }

        public void OrThrow(Exception exception)
        {
            if (match) return;

            throw exception;
        }
    }

    public class ReflectionSwitchStatement
    {
        readonly Type subject;

        bool match;

        public ReflectionSwitchStatement(Type subject)
        {
            this.subject = subject;
        }

        public ReflectionSwitchStatement Match<T>(Action<Type> action)
        {
            return Match<T>(x => true, action);
        }

        public ReflectionSwitchStatement Match<T>(bool predicate, Action<Type> action)
        {
            return Match<T>(x => predicate, action);
        }

        public ReflectionSwitchStatement Match<T>(Func<Type, bool> predicate, Action<Type> action)
        {
            if (typeof(T).IsAssignableFrom(subject) && predicate(subject))
            {
                match = true;
                action(subject);
            }

            return this;
        }

        public ReflectionSwitchStatement Then(Action<Type> action)
        {
            if (match)
            {
                action(subject);
            }

            return this;
        }

        public void Else(Action<Type> action)
        {
            if (match) return;

            match = true;
            action(subject);
        }

        public void OrThrow(Exception exception)
        {
            if (match) return;

            throw exception;
        }
    }

    public class SwitchExpression<TReturn>
    {
        readonly object subject;

        SwitchResult result = new SwitchResult();

        public SwitchExpression(object subject)
        {
            this.subject = subject;
        }

        public SwitchExpression<TReturn> Match<T>(Func<TReturn> func)
        {
            var asType = subject as Type;
            if (asType != null && asType == typeof(T))
            {
                result = new SwitchResult(func());
            }

            return Match<T>(x => func());
        }

        public SwitchExpression<TReturn> Match<T>(Func<T, TReturn> func)
        {
            if (subject is T)
            {
                result = new SwitchResult(func((T)subject));
            }

            return this;
        }

        public SwitchExpression<TReturn> Then(Action<TReturn> action)
        {
            if (result.HasResult)
            {
                action(result.Result);
            }

            return this;
        }

        public TReturn Else(TReturn value)
        {
            return result.HasResult
                ? result.Result
                : value;
        }

        public TReturn Else(Func<TReturn> func)
        {
            return result.HasResult
                ? result.Result
                : func();
        }

        public TReturn OrThrow(Exception exception = null)
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            throw exception ?? new ArgumentOutOfRangeException();
        }

        public TReturn OrDefault()
        {
            if (result.HasResult)
            {
                return result.Result;
            }

            return default(TReturn);
        }

        class SwitchResult
        {
            public SwitchResult()
            {
                HasResult = false;
            }

            public SwitchResult(TReturn result)
            {
                Result = result;
                HasResult = true;
            }

            public bool HasResult { get; }
            public TReturn Result { get; }
        }
    }

}