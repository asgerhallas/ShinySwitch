using System;
using Xunit;

namespace ShinySwitch.Tests
{
    public class SwitchExpressionTests
    {
        [Fact]
        public void IfNoMatchThenThrow()
        {
            Assert.Throws<Exception>(() =>
                new TestSwitchExpression(new object(), new SwitchResult<string>())
                    .OrThrow(new Exception("ohno")));
        }

        [Fact]
        public void IfMatchThenNoThrow()
        {
            var exception = Record.Exception(() =>
                new TestSwitchExpression(new object(), new SwitchResult<string>("A"))
                    .OrThrow(new Exception("ohno")));

            Assert.Equal(null, exception);
        }

        [Fact]
        public void IfNoMatchThenThrowOnImplicitReturn()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var something = (string)new TestSwitchExpression(new object(), new SwitchResult<string>());
            });
        }

        [Fact]
        public void IfMatchThenNoThrowOnImplicitReturn()
        {
            Assert.Equal("A", 
                new TestSwitchExpression(new object(), new SwitchResult<string>("A")));
        }

        [Fact]
        public void StringConcatOnImplicitReturn()
        {
            Assert.Equal("Value: A",
                "Value: " + new TestSwitchExpression(new object(), new SwitchResult<string>("A")));
        }


        [Fact]
        public void IfNoMatchThenElse()
        {
            Assert.Equal("else",
                new TestSwitchExpression(new object(), new SwitchResult<string>())
                    .Else(() => "else"));
        }

        [Fact]
        public void SwitchAnonExpression()
        {
            Assert.Equal(new {a = "a"},
                Switch.On("").Return(new {a = ""})
                    .Match<string>(y => new {a = "a"})
                    .Else(new { a = "b" }));
        }

        [Fact]
        public void NullValues()
        {
            
        }

        SystemTypeSwitchExpression<T> Match<T>(Func<object, T> func)
        {
            return null;
        }

        public class TestSwitchExpression : SwitchExpression<object, string>
        {
            public TestSwitchExpression(object subject, SwitchResult<string> result) : base(subject, result)
            {
                
            }
        }
    }
}