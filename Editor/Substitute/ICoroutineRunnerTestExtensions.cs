using System;
using System.Collections.Generic;
using CoroutineSubstitute.Call;

namespace CoroutineSubstitute
{
    public static class ICoroutineRunnerTestExtensions
    {
        public static IReadOnlyList<IStartCoroutineCall> GetReceivedStartCoroutineCalls (
            this ICoroutineRunner runner
        )
            => CastToSubstitute(runner).ReceivedStartCoroutineCalls;

        public static bool MoveNext (this ICoroutineRunner runner)
            => CastToSubstitute(runner).MoveNext();

        static CoroutineRunnerSubstitute CastToSubstitute (ICoroutineRunner runner)
        {
            if (runner is CoroutineRunnerSubstitute substitute)
                return substitute;

            throw new InvalidOperationException(
                "ICoroutineRunner must be created using CoroutineSubstitute.Create()"
            );
        }
    }
}
