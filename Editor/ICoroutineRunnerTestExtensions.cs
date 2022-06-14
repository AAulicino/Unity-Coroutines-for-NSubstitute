using System;
using CoroutineSubstitute.Substitutes;
using CoroutineSubstitute.Substitutes.Call;
using UnityEngine;

namespace CoroutineSubstitute
{
    public static class ICoroutineRunnerTestExtensions
    {
        public static bool MoveNext (this ICoroutineRunner runner)
            => CastToSubstitute(runner).MoveNext();

        public static bool MoveNext (this ICoroutineRunner runner, int callId)
            => FindCall(runner, callId).MoveNext();

        public static bool MoveNext (this ICoroutineRunner runner, Coroutine coroutine)
            => FindCall(runner, coroutine).MoveNext();

        public static bool MoveNext (this ICoroutineRunner runner, string callerName)
            => FindCall(runner, callerName).MoveNext();

        public static IStartCoroutineCall FindCall (this ICoroutineRunner runner, int callId)
            => CastToSubstitute(runner).FindCall(callId);

        public static IStartCoroutineCall FindCall (this ICoroutineRunner runner, Coroutine routine)
            => CastToSubstitute(runner).FindCall(routine);

        public static IStartCoroutineCall FindCall (this ICoroutineRunner runner, string callerMethod)
            => CastToSubstitute(runner).FindCall(callerMethod);

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
