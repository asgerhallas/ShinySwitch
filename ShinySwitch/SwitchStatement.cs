using System;

namespace ShinySwitch
{
    public abstract class SwitchStatement<TSubject>(TSubject subject, MatchResult<bool> result, bool matchMany)
    {
        public TSubject Subject { get; } = subject;
        public MatchResult<bool> Result { get; } = result;
        public bool MatchMany { get; } = matchMany;

        public void Else(Action<object> action)
        {
            if (Result.HasMatch) return;

            action(Subject);
        }

        public void OrThrow(Exception exception = null)
        {
            if (Result.HasMatch) return;

            throw exception ?? new ArgumentOutOfRangeException();
        }

        public void OrThrow(Func<Exception> exception)
        {
            if (Result.HasMatch) return;

            throw exception() ?? new ArgumentOutOfRangeException();
        }
    }
}