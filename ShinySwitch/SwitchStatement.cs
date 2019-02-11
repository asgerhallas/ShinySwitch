using System;

namespace ShinySwitch
{
    public abstract class SwitchStatement<TSubject>
    {
        protected SwitchStatement(TSubject subject, SwitchResult<bool> result)
        {
            Subject = subject;
            Result = result;
        }

        public TSubject Subject { get; }
        public SwitchResult<bool> Result { get; }

        public void Else(Action<object> action)
        {
            if (Result.HasResult) return;

            action(Subject);
        }

        public void OrThrow(Exception exception = null)
        {
            if (Result.HasResult) return;

            throw exception ?? new ArgumentOutOfRangeException();
        }
    }
}