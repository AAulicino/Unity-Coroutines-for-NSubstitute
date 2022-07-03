using CoroutineSubstitute.Assertions;
using CoroutineSubstitute.Substitutes;
using NSubstitute;

namespace CoroutineSubstitute
{
    public delegate void TypeAssert (object expected, object actual);

    public static class CoroutineSubstitute
    {
        public static ICoroutineRunner Create ()
            => Substitute.ForPartsOf<CoroutineRunnerSubstitute>();

        public static void RegisterCustomType<T> (TypeAssert typeAssert)
            => CoroutineRunnerAssertions.RegisterCustomType<T>(typeAssert);
    }
}
