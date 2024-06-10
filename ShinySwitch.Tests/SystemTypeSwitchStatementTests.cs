using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SystemTypeSwitchStatementTests
    {
        string result = "";

        [Fact]
        public void MatchPassesSubject() =>
            Switch.On(typeof(B))
                .Match<A>(x => Assert.Equal(typeof(B), x))
                .Match<B>(x => Assert.Equal(typeof(B), x))
                .Match<C>(x => throw new Exception("Should not be called."));

        [Fact]
        public void MatchOnType()
        {
            Switch.On(typeof (B))
                .Match<A>(x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("A", result);
        }

        [Fact]
        public void MatchOnType_FallThrough()
        {
            Switch.On(typeof (B), matchMany: true)
                .Match<A>(x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("AB", result);
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Switch.On(typeof(B))
                .Match<A>(x => false, x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("B", result);
        }

        [Fact]
        public void IfMatchThen()
        {
            Switch.On(typeof(B))
                .Match<B>(x => result += "B")
                .Then(x => result += "then");

            Assert.Equal("Bthen", result);
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Switch.On(typeof(B))
                .Match<C>(x => result += "C")
                .Then(x => result += "then");

            Assert.Equal("", result);
        }

        [Fact]
        public void IfNoMatchNoThenButElse()
        {
            Switch.On(typeof(B))
                .Match<C>(x => result += "C")
                .Then(x => result += "then")
                .Else(x => result += "else");

            Assert.Equal("else", result);
        }

        [Fact]
        public void FallThrough_Type()
        {
            Switch.On(typeof(B), matchMany: true)
                .Match<A>(x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("AB", result);
        }
    }

    public class A { }
    public class B : A { }
    public class C : A, X { }
    public interface X { }
}