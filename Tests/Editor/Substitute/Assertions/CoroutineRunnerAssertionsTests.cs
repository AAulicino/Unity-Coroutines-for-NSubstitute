using System;
using System.Collections;
using CoroutineSubstitute.Assertions;
using CoroutineSubstitute.Substitutes;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.UnitTests.Assertions
{
    public class CoroutineAssertionsTests
    {
        class BaseCoroutineAssertionsTests
        {
            public ICoroutineRunnerSubstitute Runner { get; private set; }
            public IStartCoroutineCall Call { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = Substitute.For<ICoroutineRunnerSubstitute>();
                Call = Substitute.For<IStartCoroutineCall>();
                Runner.ActiveCoroutines.Returns(new[] { Call });
            }

            [TearDown]
            public void TearDown ()
            {
                CoroutineRunnerAssertions.customTypes.Clear();
            }
        }

        class MoveNextAndExpect_Generic : BaseCoroutineAssertionsTests
        {
            [Test]
            public void Throws_If_No_Active_Routines ()
            {
                Runner.ActiveCoroutines.Returns(new IStartCoroutineCall[0]);
                Assert.Throws<InvalidOperationException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect<object>(Runner)
                );
            }

            [Test]
            public void Throws_If_Null ()
            {
                Call.Current.Returns(null);
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect<object>(Runner)
                );
            }

            [Test]
            public void Throws_If_Not_Expected_Type ()
            {
                Call.Current.Returns(new WaitForSeconds(default));
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect<WaitForFixedUpdate>(Runner)
                );
            }

            [Test]
            public void Pass_If_Is_Expected_Type ()
            {
                Call.Current.Returns(new WaitForSeconds(default));
                CoroutineRunnerAssertions.MoveNextAndExpect<WaitForSeconds>(Runner);
            }

            [Test]
            public void Pass_If_Inherits_From_Expected_Type ()
            {
                Call.Current.Returns(new WaitForSeconds(default));
                CoroutineRunnerAssertions.MoveNextAndExpect<YieldInstruction>(Runner);
            }

            [Test]
            public void Pass_If_Expected_Is_IEnumerator ()
            {
                Call.Current.Returns(Substitute.For<IEnumerator>());
                CoroutineRunnerAssertions.MoveNextAndExpect<IEnumerator>(Runner);
            }

            [Test]
            public void Pass_If_Expected_Is_Coroutine ()
            {
                Call.Current.Returns(CoroutineFactory.Create(default));
                CoroutineRunnerAssertions.MoveNextAndExpect<Coroutine>(Runner);
            }
        }

        class MoveNextAndExpect : BaseCoroutineAssertionsTests
        {
            [Test]
            public void Throws_If_No_Active_Routines ()
            {
                Call.Current.Returns(new WaitForSeconds(1));
                Runner.ActiveCoroutines.Returns(new IStartCoroutineCall[0]);
                Assert.Throws<InvalidOperationException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForSeconds(1))
                );
            }

            [Test]
            public void Throws_If_Type_Mismatch ()
            {
                Call.Current.Returns(new WaitForSecondsRealtime(1));
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForSeconds(1))
                );
            }

            [Test]
            public void WaitForSeconds_Equals ()
            {
                Call.Current.Returns(new WaitForSeconds(2));
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForSeconds(2));
            }

            [Test]
            public void Throws_If_WaitForSeconds_Not_Equal ()
            {
                Call.Current.Returns(new WaitForSeconds(1));
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForSeconds(2))
                );
            }

            [Test]
            public void WaitForSecondsRealtime_Equals ()
            {
                Call.Current.Returns(new WaitForSecondsRealtime(2));
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForSecondsRealtime(2));
            }

            [Test]
            public void Throws_If_WaitForSecondsRealtime_Not_Equal ()
            {
                Call.Current.Returns(new WaitForSecondsRealtime(1));
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(
                        Runner,
                        new WaitForSecondsRealtime(2)
                    )
                );
            }

            [Test]
            public void WaitForFixedUpdate_Equals ()
            {
                Call.Current.Returns(new WaitForFixedUpdate());
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForFixedUpdate());
            }

            [Test]
            public void WaitForEndOfFrame_Equals ()
            {
                Call.Current.Returns(new WaitForEndOfFrame());
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new WaitForEndOfFrame());
            }

            [Test]
            public void IEnumerator_Equals ()
            {
                Call.Current.Returns(Substitute.For<IEnumerator>());
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, Substitute.For<IEnumerator>());
            }

            [Test]
            public void Null_Equals ()
            {
                Call.Current.Returns(null);
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, null);
            }

            [Test]
            public void Throws_NotSupportedException_If_Unknown_Type ()
            {
                Call.Current.Returns(new DummyType(1));
                Assert.Throws<NotSupportedException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new DummyType(1))
                );
            }

            [Test]
            public void CustomType_Equals ()
            {
                CoroutineRunnerAssertions.RegisterCustomType<DummyType>(
                    (expected, actual) => Assert.AreEqual(
                        ((DummyType)expected).Value,
                        ((DummyType)actual).Value
                    )
                );

                Call.Current.Returns(new DummyType(1));
                CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new DummyType(1));
            }

            [Test]
            public void CustomType_Throws_If_Not_Equals ()
            {
                CoroutineRunnerAssertions.RegisterCustomType<DummyType>(
                    (expected, actual) => Assert.AreEqual(
                        ((DummyType)expected).Value,
                        ((DummyType)actual).Value
                    )
                );

                Call.Current.Returns(new DummyType(1));
                Assert.Throws<AssertionException>(() =>
                    CoroutineRunnerAssertions.MoveNextAndExpect(Runner, new DummyType(2))
                );
            }

            class DummyType
            {
                public int Value;
                public DummyType (int value) => Value = value;
            }
        }
    }
}
