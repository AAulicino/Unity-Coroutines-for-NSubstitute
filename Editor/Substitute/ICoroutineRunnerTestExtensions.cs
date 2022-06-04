namespace CoroutineSubstitute
{
    public static class ICoroutineRunnerTestExtensions
    {
        public static bool MoveNext (this ICoroutineRunner runner)
            => CoroutineSubstitute.callRouter.MoveNext(runner);
    }
}
