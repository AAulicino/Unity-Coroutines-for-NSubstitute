using CoroutineSubstitute.Call;

namespace CoroutineSubstitute
{
    public static class CoroutineSubstitute
    {
        internal static readonly CoroutineRunnerRouter callRouter = new CoroutineRunnerRouter();
        static readonly IStartCoroutineCallFactory callFactory = new StartCoroutineCallFactory();

        public static ICoroutineRunner Create ()
        {
            CoroutineRunnerSubstitute coroutineRunnerSubstitute = new CoroutineRunnerSubstitute(
                callFactory
            );
            callRouter.Register(coroutineRunnerSubstitute);
            return coroutineRunnerSubstitute;
        }
    }
}
