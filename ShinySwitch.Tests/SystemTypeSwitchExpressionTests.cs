using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SystemTypeSwitchExpressionTests
    {
        [Fact]
        public void MatchPassesSubject()
        {
            Switch<string>.On(typeof(B))
                .Match<A>(x =>
                {
                    Assert.Equal(typeof(B), x);
                    return "";
                })
                .Match<B>(x =>
                {
                    Assert.Equal(typeof(B), x);
                    return "";
                })
                .Match<C>(x => { throw new Exception("Should not be called."); })
                .OrThrow();
        }

        [Fact]
        public void MatchOnType()
        {
            Assert.Equal("C",
                Switch<string>.On(typeof(C))
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void FirstMatchWins()
        {
            Assert.Equal("A",
                Switch<string>.On(typeof(B))
                    .Match<A>(x => "A")
                    .Match<B>(x => "B")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Assert.Equal("B",
                Switch<string>.On(typeof(B))
                    .Match<A>(x => false, x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void IfMatchThen()
        {
            Assert.Equal("Bthen", 
                Switch<string>.On(typeof(B))
                    .Match<B>(x => "B")
                    .Then((result, x) => result+"then")
                    .OrThrow());
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Assert.Equal("",
                Switch<string>.On(typeof(B))
                    .Match<C>(x => "C")
                    .Then((result, x) => result + "then")
                    .Else(""));
        }

        public class A { }
        public class B : A { }
        public class C : A { }
    }
}