using System;

namespace ShinySwitch
{
    public class Switch
    {
        public static TypeSwitchStatement<TSubject> On<TSubject>(TSubject subject) => new TypeSwitchStatement<TSubject>(subject, new SwitchResult<bool>());
        public static SystemTypeSwitchStatement On(Type subject) => new SystemTypeSwitchStatement(subject, new SwitchResult<bool>());
    }

    public class Switch<TExpression>
    {
        public static TypeSwitchExpression<TSubject, TExpression> On<TSubject>(TSubject subject) => new TypeSwitchExpression<TSubject, TExpression>(subject, new SwitchResult<TExpression>());
        public static TypeSwitchExpression2<TLeft, TRight, TExpression> On<TLeft, TRight>(TLeft left, TRight right) => new TypeSwitchExpression2<TLeft, TRight, TExpression>(left, right, new SwitchResult<TExpression>());
        public static SystemTypeSwitchExpression<TExpression> On(Type subject) => new SystemTypeSwitchExpression<TExpression>(subject, new SwitchResult<TExpression>());


    }
}