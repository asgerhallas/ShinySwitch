using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchStatementMatchTypeTests
    {
        string result;

        public SwitchStatementMatchTypeTests()
        {
            result = "";
        }

        [Fact]
        public void MatchPassesSubject()
        {
            var subject = new B();

            Switch.On(subject)
                .Match<A>(x => Assert.Equal(subject, x))
                .Match<B>(x => Assert.Equal(subject, x))
                .Match<C>(x => { throw new Exception("Should not be called."); });
        }

        [Fact]
        public void MatchOnType()
        {
            Switch.On(new B())
                .Match<A>(x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("AB", result);
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Switch.On(new B())
                .Match<A>(x => false, x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("B", result);
        }

        [Fact]
        public void MatchOnTypeAndConstantPredicate()
        {
            Switch.On(new B())
                .Match<A>(false, x => result += "A")
                .Match<B>(x => result += "B")
                .Match<C>(x => result += "C");

            Assert.Equal("B", result);
        }

        [Fact]
        public void IfMatchThen()
        {
            Switch.On(new B())
                .Match<B>(x => result += "B")
                .Then(x => result += "then");

            Assert.Equal("Bthen", result);
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Switch.On(new B())
                .Match<C>(x => result += "C")
                .Then(x => result += "then");

            Assert.Equal("", result);
        }

        [Fact]
        public void IfNoMatchThenElse()
        {
            Switch.On(new object())
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("else", result);
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Switch.On(new A())
                .Match<A>(x => result += "A")
                .Else(x => result += "else");

            Assert.Equal("A", result);
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch.On(new object())
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch.On(new A())
                    .Match<A>(x => result += "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }


        public class A { }
        public class B : A { }
        public class C : A { }
    }
}
