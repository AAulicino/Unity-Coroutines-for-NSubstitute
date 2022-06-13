using CoroutineSubstitute.Call;
using NSubstitute;

namespace CoroutineSubstitute
{
    public static class CoroutineSubstitute
    {
        static readonly IStartCoroutineCallFactory callFactory = new StartCoroutineCallFactory();

        public static ICoroutineRunner Create ()
            => Substitute.ForPartsOf<CoroutineRunnerSubstitute>(callFactory);
    }
}
