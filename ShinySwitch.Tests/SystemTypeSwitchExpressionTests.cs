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
            Assert.Equal("B",
                Switch<string>.On(typeof(B))
                    .Match<A>(x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
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
        public void MatchOnTypeAndConstantPredicate()
        {
            Assert.Equal("B",
                Switch<string>.On(typeof(B))
                    .Match<A>(false, x => "A")
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

        [Fact]
        public void IfNoMatchThenElse()
        {
            Assert.Equal("else",
                Switch<string>.On(typeof(object))
                    .Match<A>(x => "A")
                    .Else(() => "else"));
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Assert.Equal("A",
                Switch<string>.On(typeof(A))
                    .Match<A>(x => "A")
                    .Else(() => "else"));
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch<string>.On(typeof(object))
                    .Match<A>(x => "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch<string>.On(typeof(A))
                    .Match<A>(x => "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }


        public class A { }
        public class B : A { }
        public class C : A { }
    }
}