using CoroutineSubstitute.Substitutes;
using NSubstitute;

namespace CoroutineSubstitute
{
    public static class CoroutineSubstitute
    {
        public static ICoroutineRunner Create ()
            => Substitute.ForPartsOf<CoroutineRunnerSubstitute>();
    }
}
