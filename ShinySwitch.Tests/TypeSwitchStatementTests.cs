using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class TypeSwitchStatementTests
    {
        string result;

        public TypeSwitchStatementTests()
        {
            result = "";
        }

        [Fact]
        public void MatchPassesSubject()
        {
            var subject = new B();

            Switch.On((object)subject)
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
            Switch.On((object)new B())
                .Match<A>(x => false, x => result += "A")
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
            Switch.On((object)new B())
                .Match<C>(x => result += "C")
                .Then(x => result += "then");

            Assert.Equal("", result);
        }

        public class A { }
        public class B : A { }
        public class C : A { }
    }
}
