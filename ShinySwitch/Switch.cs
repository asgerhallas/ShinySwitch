using System;

namespace ShinySwitch
{
    public class Switch
    {
        /// <summary>
        /// Switch on the type of the given instance and invoke an action based on the match.
        /// If you need to return a value, use <see cref="Switch{TExpression}" />.
        /// </summary>
        /// <param name="subject">The value to switch on.</param>
        /// <param name="matchMany">Allow multiple matches and actions to be run.</param>
        public static TypeSwitchStatement<TSubject> On<TSubject>(TSubject subject, bool matchMany = false) => new(subject, new MatchResult<bool>(), matchMany);

        /// <summary>
        /// Switch on a System.Type and invoke an action based on the match.
        /// If you need to return a value, use <see cref="Switch{TExpression}" />.
        /// </summary>
        /// <param name="subject">The System.Type to switch on.</param>
        /// <param name="matchMany">Allow multiple matches and actions to be run.</param>
        public static SystemTypeSwitchStatement On(Type subject, bool matchMany = false) => new(subject, new MatchResult<bool>(), matchMany);
    }

    public class Switch<TExpression>
    {
        /// <summary>
        /// Switch on the type of the given instance and return a value based on the match.
        /// </summary>
        /// <param name="subject">The value to switch on.</param>
        /// <returns>The value specified by the match.</returns>
        public static TypeSwitchExpression<TSubject, TExpression> On<TSubject>(TSubject subject) => new(subject, new MatchResult<TExpression>());

        /// <summary>
        /// Switch on a System.Type of the given instance and return a value based on the match.
        /// </summary>
        /// <param name="subject">The System.Type to switch on.</param>
        /// <returns>The value specified by the match.</returns>
        public static SystemTypeSwitchExpression<TExpression> On(Type subject) => new(subject, new MatchResult<TExpression>());
    }
}