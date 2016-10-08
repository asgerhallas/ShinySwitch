namespace ShinySwitch
{
    public class SwitchResult<TReturn>
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