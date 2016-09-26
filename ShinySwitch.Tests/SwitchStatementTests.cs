using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchStatementTests
    {
        string result;

        public SwitchStatementTests()
        {
            result = "";
        }

        [Fact]
        public void MatchOnType()
        {
            Switch.OnTypeOf(new B())
                .Match<A>(x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("AB", result);
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Switch.OnTypeOf(new B())
                .Match<A>(x => false, x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("B", result);
        }

        [Fact]
        public void MatchOnTypeAndClosedOverPredicate()
        {
            Switch.OnTypeOf(new B())
                .Match<A>(false, x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("B", result);
        }

        [Fact]
        public void IfMatchThen()
        {
            Switch.OnTypeOf(new B())
                .Match<B>(x => result += "B")
                .Then(x => result += "then");

            Assert.Equal("Bthen", result);
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Switch.OnTypeOf(new B())
                .Match<C>(x => result += "C")
                .Then(x => result += "then");

            Assert.Equal("", result);
        }

        [Fact]
        public void IfNoMatchThenElse()
        {
            Switch.OnTypeOf(new object())
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("else", result);
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Switch.OnTypeOf(new A())
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("A", result);
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch.OnTypeOf(new object())
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch.OnTypeOf(new A())
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }


        public class A { }
        public class B : A { }
        public class C : A { }
    }
}
