namespace ShinySwitch
{
    public class MatchResult<TReturn>
    {
        public MatchResult() => HasMatch = false;

        public MatchResult(TReturn value)
        {
            Value = value;
            HasMatch = true;
        }

        public bool HasMatch { get; }
        public TReturn Value { get; }
    }
}