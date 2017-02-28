using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class TypeSwitchExpressionTests
    {
        [Fact]
        public void MatchPassesSubject()
        {
            A subject = new B();

            Switch<string>.On(subject)
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
            Assert.Equal("C",
                Switch<string>.On((object)new C())
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnInterface()
        {
            Assert.Equal("C",
                Switch<string>.On((A)new C())
                    .Match<X>(x => "C"));
        }

        //[Fact]
        //public void MatchOnType2()
        //{
        //    Assert.Equal("C",
        //        Switch<string, object>
        //            .Match<B>(x => "B")
        //            .Match<C>(x => "C"))
        //        .OrThrow());
        //}

        [Fact]
        public void MatchOnTypeFirstMatchWins()
        {
            Assert.Equal("A",
                Switch<string>.On((object)new B())
                    .Match<A>(x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnTypeAndPredicate()
        {
            Assert.Equal("B",
                Switch<string>.On((object)new B())
                    .Match<A>(x => false, x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnValue()
        {
            Assert.Equal("B",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, _ => "A")
                    .Match(TheEnum.B, _ => "B")
                    .Match(TheEnum.C, _ => "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnValueReturnConstant()
        {
            Assert.Equal("B",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, "A")
                    .Match(TheEnum.B, "B")
                    .Match(TheEnum.C, "C")
                    .OrThrow());
        }

        [Fact]
        public void MatchOnValueAndPredicate()
        {
            Assert.Equal("B",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, x => false, x => "A")
                    .Match(TheEnum.B, x => "B")
                    .Match(TheEnum.C, x => "C")
                    .OrThrow());
        }

        [Fact]
        public void IfMatchThen()
        {
            Assert.Equal("Bthen",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.B, x => "B")
                    .Then((result, x) => result + "then")
                    .OrThrow());
        }

        [Fact]
        public void IfNoMatchNoThen()
        {
            Assert.Equal("",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.C, x => "C")
                    .Then((result, x) => result + "then")
                    .Else(""));
        }

        [Fact]
        public void IfMatchThenChangeType()
        {
            Assert.Equal("1then",
                Switch<int>.On(TheEnum.B)
                    .Match(TheEnum.B, x => 1)
                    .Then((result, x) => result + "then")
                    .OrThrow());
        }
    }
}