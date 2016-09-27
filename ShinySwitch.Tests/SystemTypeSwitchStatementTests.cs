using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SystemTypeSwitchStatementTests
    {
        string result;

        public SystemTypeSwitchStatementTests()
        {
            result = "";
        }

        [Fact]
        public void MatchPassesSubject()
        {
            Switch.On(typeof(B))
                .Match<A>(x => Assert.Equal(typeof(B), x))
                .Match<B>(x => Assert.Equal(typeof(B), x))
                .Match<C>(x => { throw new Exception("Should not be called.");});
        }

        [Fact]
        public void MatchOnType()
        {
            Switch.On(typeof (B))
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
        public void MatchOnTypeAndConstantPredicate()
        {
            Switch.On(typeof(B))
                .Match<A>(false, x => result += "A")
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
        public void IfNoMatchThenElse()
        {
            Switch.On(typeof(object))
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("else", result);
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Switch.On(typeof(A))
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("A", result);
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch.On(typeof(object))
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch.On(typeof(A))
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }

        public class A { }
        public class B : A { }
        public class C : A { }
    }
}