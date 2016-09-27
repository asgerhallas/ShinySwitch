using System;

namespace ShinySwitch
{
    public class Switch
    {
        public static SwitchStatement On(object subject) => new SwitchStatement(subject, false);
        public static SystemTypeSwitchStatement On(Type subject) => new SystemTypeSwitchStatement(subject, false);
    }

    public class Switch<TReturn>
    {
        public static SwitchExpression<TReturn> On(object subject) => new SwitchExpression<TReturn>(subject, new SwitchResult<TReturn>());
        public static SystemTypeSwitchExpression<TReturn> On(Type subject) => new SystemTypeSwitchExpression<TReturn>(subject, new SwitchResult<TReturn>());
    }
}