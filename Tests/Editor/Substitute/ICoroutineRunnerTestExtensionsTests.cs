using System;
using NSubstitute;
using NUnit.Framework;

namespace CoroutineSubstitute.Tests
{
    public class ICoroutineRunnerTestExtensionsTests
    {
        class BaseICoroutineRunnerTestExtensionsTests
        {
            public ICoroutineRunner CoroutineRunner { get; private set; }

            [SetUp]
            public void Setup ()
            {
                CoroutineRunner = CoroutineSubstitute.Create();
            }
        }

        class MoveNext : BaseICoroutineRunnerTestExtensionsTests
        {
            [Test]
            public void Calls_MoveNext_On_CoroutineRunner ()
            {
                ICoroutineRunnerTestExtensions.MoveNext(CoroutineRunner);

                CoroutineRunner.Received().MoveNext();
            }

            [Test]
            public void Returns_CoroutineRunner_MoveNext_Result ()
            {
                CoroutineRunner.MoveNext().Returns(true);

                Assert.IsTrue(ICoroutineRunnerTestExtensions.MoveNext(CoroutineRunner));
            }

            [Test]
            public void Throws_When_Runner_Is_Not_CoroutineRunnerSubstitute ()
            {
                Assert.Throws<ArgumentException>(() =>
                    ICoroutineRunnerTestExtensions.MoveNext(Substitute.For<ICoroutineRunner>())
                );
            }
        }
    }
}
