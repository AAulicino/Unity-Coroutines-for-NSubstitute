namespace CoroutineSubstitute
{
    public static class ICoroutineRunnerTestExtensions
    {
        public static object Current (this ICoroutineRunner runner)
            => CoroutineSubstitute.callRouter.Current(runner);

        public static bool MoveNext (this ICoroutineRunner runner)
            => CoroutineSubstitute.callRouter.MoveNext(runner);
    }
}
