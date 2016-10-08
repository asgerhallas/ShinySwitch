using System;

namespace ShinySwitch
{
    public abstract class SwitchStatement<TSubject>
    {
        protected readonly TSubject subject;
        protected readonly SwitchResult<bool> result;

        protected SwitchStatement(TSubject subject, SwitchResult<bool> result)
        {
            this.subject = subject;
            this.result = result;
        }

        public void Else(Action<object> action)
        {
            if (result.HasResult) return;

            action(subject);
        }

        public void OrThrow(Exception exception = null)
        {
            if (result.HasResult) return;

            throw exception ?? new ArgumentOutOfRangeException();
        }
    }
}