using System;
using CoroutineSubstitute.Substitutes;

namespace CoroutineSubstitute
{
    public static class ICoroutineRunnerTestExtensions
    {
        public static bool MoveNext (this ICoroutineRunner runner)
            => CastToSubstitute(runner).MoveNext();

        public static void MoveNextAndExpect<T> (this ICoroutineRunner runner)
            => CastToSubstitute(runner).MoveNextAndExpect<T>();

        static CoroutineRunnerSubstitute CastToSubstitute (ICoroutineRunner runner)
        {
            if (runner is CoroutineRunnerSubstitute substitute)
                return substitute;

            throw new ArgumentException(
                "ICoroutineRunner must be created using CoroutineSubstitute.Create()"
            );
        }
    }
}
