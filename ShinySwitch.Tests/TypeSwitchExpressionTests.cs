using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace ShinySwitch.Tests
{
    public class TypeSwitchExpressionTests(ITestOutputHelper output)
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
                .Match<C>(x => throw new Exception("Should not be called."))
                .OrThrow();
        }

        [Fact]
        public void MatchOnType() =>
            Assert.Equal("C",
                Switch<string>.On((object)new C())
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());

        [Fact]
        public void MatchOnInterface() =>
            Assert.Equal("C",
                Switch<string>.On((A)new C())
                    .Match<X>(x => "C"));

        [Fact]
        public void MatchOnTypeFirstMatchWins() =>
            Assert.Equal("A",
                Switch<string>.On((object)new B())
                    .Match<A>(x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());

        [Fact]
        public void MatchOnTypeAndPredicate() =>
            Assert.Equal("B",
                Switch<string>.On((object)new B())
                    .Match<A>(x => false, x => "A")
                    .Match<B>(x => "B")
                    .Match<C>(x => "C")
                    .OrThrow());

        [Fact]
        public void MatchOnValue()
        {
            //Assert.Equal("B",
            //    Switch<string>.On(TheEnum.B)
            //        .Match(1, _ => "1")
            //        .Match(2, _ => "2")
            //        .Match(TheEnum.A, _ => "A")
            //        .Match(TheEnum.B, _ => "B")
            //        .Match(TheEnum.C, _ => "C")
            //        .OrThrow());
        }

        [Fact]
        public void MatchOnValueReturnConstant() =>
            Assert.Equal("B",
                Switch<string>.On(TheEnum.B)
                    .Match(1, _ => "1")
                    .Match(2, _ => "2")
                    .Match(TheEnum.A, "A")
                    .Match(TheEnum.B, "B")
                    .Match(TheEnum.C, "C")
                    .OrThrow());

        //[Fact]
        //public void MatchOnValueAndPredicate()
        //{
        //    Assert.Equal("B",
        //        Switch<string>.On(TheEnum.B)
        //            .Match(1, _ => "1")
        //            .Match(2, _ => "2")
        //            .Match(TheEnum.B, x => false, x => "A")
        //            .Match(TheEnum.B, x => "B")
        //            .Match(TheEnum.C, x => "C")
        //            .OrThrow());
        //}

        //[Fact]
        //public void IfMatchThen()
        //{
        //    Assert.Equal("Bthen",
        //        Switch<string>.On(TheEnum.B)
        //            .Match(TheEnum.B, x => "B")
        //            .Then(result => result + "then")
        //            .OrThrow());
        //}

        //[Fact]
        //public void IfNoMatchNoThen()
        //{
        //    Assert.Equal("",
        //        Switch<string>.On(TheEnum.B)
        //            .Match(TheEnum.C, x => "C")
        //            .Then(result => result + "then")
        //            .Else(""));
        //}

        //[Fact]
        //public void IfMatchThenChangeType()
        //{
        //    Assert.Equal("1then",
        //        Switch<int>.On(TheEnum.B)
        //            .Match(TheEnum.B, x => 1)
        //            .Then(result => result + "then")
        //            .OrThrow());
        //}

        [Fact]
        public void IfMatchOnPredicate() =>
            Assert.Equal("B",
                Switch<string>.On("anything")
                    .Match(() => false, x => "A")
                    .Match(() => true, x => "B")
                    .Match(() => false, x => "C")
                    .OrThrow());

        [Fact]
        public void MatchOnNullValue() =>
            Assert.Equal("Null",
                Switch<string>.On((object)null)
                    .Match(1, _ => "1")
                    .Match(TheEnum.A, "A")
                    .MatchNull("Null")
                    .OrThrow());

        [Fact]
        public void MatchOnNullValueReturnLazy() =>
            Assert.Equal("Null",
                Switch<string>.On((object)null)
                    .Match(1, _ => "1")
                    .Match(TheEnum.A, "A")
                    .MatchNull(() => "Null")
                    .OrThrow());

        [Fact]
        public void CrudePerformanceMeasure()
        {
            // 15-12-2017: 120ms on my machine :)

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000000; i++)
            {
                Switch<string>.On(1)
                    .Match(2, x => "A")
                    .Match(3, x => "B")
                    .Match(1, x => "C");
            }

            output.WriteLine($"{sw.Elapsed.TotalMilliseconds}");
        }
    }
}