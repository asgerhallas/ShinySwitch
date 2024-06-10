using System;
using System.Linq.Expressions;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchStatementTests
    {
        string result = "";

        [Fact]
        public void IfNoMatchThenElse()
        {
            new TestSwitchStatement(new object(), new MatchResult<bool>())
                .Else(x => result += "else");

            Assert.Equal("else", result);
        }

        [Fact]
        public void IfMatchThenNoElse()
        {
            new TestSwitchStatement(new object(), new MatchResult<bool>(true))
                .Else(x => result += "else");

            Assert.Equal("", result);
        }

        [Fact]
        public void IfNoMatchThenThrow() =>
            Assert.Throws<Exception>(() =>
                new TestSwitchStatement(new object(), new MatchResult<bool>())
                    .OrThrow(new Exception("ohno")));

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                new TestSwitchStatement(new object(), new MatchResult<bool>(true))
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }

        public class TestSwitchStatement(object subject, MatchResult<bool> result) : SwitchStatement<object>(subject, result, matchMany: false);
    }
}