using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchExpressionTests
    {
        [Fact]
        public void MatchPassesSubject()
        {
            var subject = new B();

            Switch.On<string>(subject)
                .Match<A>(x =>
                {
                    Assert.Equal(subject, x);
                    return "";
                })
                .Match<B>(x =>
                {
                    Assert.Equal(subject, x);
                    return "";
                })
                .Match<C>(x => { throw new Exception("Should not be called."); })
                .OrThrow();
        }

        [Fact]
        public void MatchOnType()
        {
            Assert.Equal("B",
                Switch.On<string>(new B())
                    .Match<A>(x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Assert.Equal("B",
                Switch.On<string>(new B())
                    .Match<A>(x => false, x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnTypeAndConstantPredicate()
        {
            Assert.Equal("B",
                Switch.On<string>(new B())
                    .Match<A>(false, x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void IfMatchThen()
        {
            Assert.Equal("Bthen", 
                Switch.On<string>(new B())
                    .Match<B>(x => "B")
                    .Then((result, x) => result+"then")
                    .OrThrow());
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Assert.Equal("",
                Switch.On<string>(new B())
                    .Match<C>(x => "C")
                    .Then((result, x) => result + "then")
                    .Else(""));
        }

        [Fact]
        public void IfNoMatchThenElse()
        {
            Assert.Equal("else",
                Switch.On<string>(new object())
                    .Match<A>(x => "A")
                    .Else(() => "else"));
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Assert.Equal("A",
                Switch.On<string>(new A())
                    .Match<A>(x => "A")
                    .Else(() => "else"));
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch.On<string>(new object())
                    .Match<A>(x => "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch.On<string>(new A())
                    .Match<A>(x => "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }


        public class A { }
        public class B : A { }
        public class C : A { }
    }
}