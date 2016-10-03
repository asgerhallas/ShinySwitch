using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchExpressionMatchValueTests
    {
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
        public void MatchOnValueAndConstantPredicate()
        {
            Assert.Equal("B",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, false, x => "A")
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
        public void IfNoMatchThenElse()
        {
            Assert.Equal("else",
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, x => "A")
                    .Else(() => "else"));
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            Assert.Equal("A",
                Switch<string>.On(TheEnum.A)
                    .Match(TheEnum.A, x => "A")
                    .Else(() => "else"));
        }


        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                Switch<string>.On(TheEnum.B)
                    .Match(TheEnum.A, x => "A")
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                Switch<string>.On(TheEnum.A)
                    .Match(TheEnum.A, x => "A")
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }

        public enum TheEnum
        {
            A,
            B,
            C
        }
    }
}