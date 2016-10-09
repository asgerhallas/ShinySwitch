using Xunit;

namespace ShinySwitch.Tests
{
    public class TypeSwitchExpression2Tests
    {
        [Fact]
        public void MatchBothSides()
        {
            var b = new B();

            Assert.Equal("Yes", Switch<string>.On((A)b, TheEnum.C)
                .Match<B, TheEnum>((r, l) => "Yes")
                .Match<C, TheEnum>((r, l) => "No"));

            Assert.Equal("Yes", Switch<string>.On((A)b, TheEnum.C)
                .Match(b, TheEnum.C, (r, l) => "Yes")
                .Match(b, TheEnum.A, (r, l) => "No"));
        }

        [Fact]
        public void MatchLeft()
        {
            var b = new B();

            Assert.Equal("Yes", Switch<string>.On((A)b, TheEnum.C)
                .MatchLeft<B>((r, l) => "Yes")
                .MatchLeft<C>((r, l) => "No"));

            Assert.Equal("Yes", Switch<string>.On((A)b, TheEnum.C)
                .MatchLeft<B>(TheEnum.C, (r, l) => "Yes")
                .MatchLeft<C>(TheEnum.A, (r, l) => "No"));
        }

        [Fact]
        public void MatchRight()
        {
            var b = new B();

            Assert.Equal("Yes", Switch<string>.On(TheEnum.C, (A)b)
                .MatchRight<B>((r, l) => "Yes")
                .MatchRight<C>((r, l) => "No"));

            Assert.Equal("Yes", Switch<string>.On(TheEnum.C, (A)b)
                .MatchRight<B>(TheEnum.C, (r, l) => "Yes")
                .MatchRight<C>(TheEnum.A, (r, l) => "No"));
        }
    }
}