using CoroutineSubstitute.Call;

namespace CoroutineSubstitute
{
    public static class CoroutineSubstitute
    {
        static readonly IStartCoroutineCallFactory callFactory = new StartCoroutineCallFactory();

        public static ICoroutineRunner Create ()
        {
            return new CoroutineRunnerSubstitute(callFactory);
        }
    }
}
