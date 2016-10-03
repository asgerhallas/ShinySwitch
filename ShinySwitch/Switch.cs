using System;

namespace ShinySwitch
{
    public class Switch
    {
        public static TypeSwitchStatement<T> On<T>(T subject) => new TypeSwitchStatement<T>(subject, false);
        public static SystemTypeSwitchStatement On(Type subject) => new SystemTypeSwitchStatement(subject, false);
    }

    public class Switch<TReturn>
    {
        public static SwitchExpression<T, TReturn> On<T>(T subject) => new SwitchExpression<T, TReturn>(subject, new SwitchResult<TReturn>());
        public static SystemTypeSwitchExpression<TReturn> On(Type subject) => new SystemTypeSwitchExpression<TReturn>(subject, new SwitchResult<TReturn>());
    }
}