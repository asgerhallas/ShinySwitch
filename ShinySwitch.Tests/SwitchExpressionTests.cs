using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchExpressionTests
    {
        [Fact]
        public void IfNoMatchThenThrow() =>
            Assert.Throws<Exception>(() =>
                new TestSwitchExpression(new object(), new MatchResult<string>())
                    .OrThrow(new Exception("ohno")));

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                new TestSwitchExpression(new object(), new MatchResult<string>("A"))
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }

        [Fact]
        public void IfNoMatchThenThrowOnImplicitReturn() =>
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var something = (string)new TestSwitchExpression(new object(), new MatchResult<string>());
            });

        [Fact]
        public void IfMatchThenNoThrowOnImplicitReturn() =>
            Assert.Equal("A", 
                new TestSwitchExpression(new object(), new MatchResult<string>("A")));

        [Fact]
        public void StringConcatOnImplicitReturn() =>
            Assert.Equal("Value: A",
                "Value: " + new TestSwitchExpression(new object(), new MatchResult<string>("A")));

        [Fact]
        public void IfNoMatchThenElse() =>
            Assert.Equal("else",
                new TestSwitchExpression(new object(), new MatchResult<string>())
                    .Else(() => "else"));

        [Fact]
        public void NullValues()
        {
            
        }

        SystemTypeSwitchExpression<T> Match<T>(Func<object, T> func) => null;

        public class TestSwitchExpression(object subject, MatchResult<string> result) : SwitchExpression<object, string>(subject, result);
    }
}