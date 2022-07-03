using System;
using System.Collections;
using CoroutineSubstitute.Substitutes;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.Tests
{
    public class CoroutineRunnerSubstituteTests
    {
        class BaseCoroutineRunnerSubstituteTests
        {
            public CoroutineRunnerSubstitute CoroutineRunner { get; private set; }
            public IStartCoroutineCallFactory CallFactory { get; private set; }

            [SetUp]
            public void Setup ()
            {
                CallFactory = Substitute.For<IStartCoroutineCallFactory>();
                CoroutineRunner = new CoroutineRunnerSubstitute(CallFactory);
            }
        }

        class StartCoroutine : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Calls_CallFactory_Create ()
            {
                CallFactory.Create(default).ReturnsForAnyArgs(
                    Substitute.For<IStartCoroutineCall>()
                );

                IEnumerator enumerator = Substitute.For<IEnumerator>();

                CoroutineRunner.StartCoroutine(enumerator);

                CallFactory.Received().Create(enumerator);
            }

            [Test]
            public void Returns_Coroutine_With_Id ()
            {
                const int EXPECTED = 1;
                CallFactory.Create(default).ReturnsForAnyArgs(
                    x =>
                    {
                        IStartCoroutineCall sub = Substitute.For<IStartCoroutineCall>();
                        sub.Id.Returns(EXPECTED);
                        return sub;
                    }
                );
                IEnumerator enumerator = Substitute.For<IEnumerator>();

                Coroutine coroutine = CoroutineRunner.StartCoroutine(enumerator);

                Assert.AreEqual(EXPECTED, coroutine.GetId());
            }

            [Test]
            public void Throws_For_Null_Argument ()
            {
                Assert.Throws<ArgumentNullException>(() => CoroutineRunner.StartCoroutine(null));
            }
        }

        class StopCoroutine : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Dont_Call_MoveNext_For_Stopped_Calls ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(default).ReturnsForAnyArgs(call0);
                Coroutine coroutine = CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CoroutineRunner.StopCoroutine(coroutine);

                call0.DidNotReceive().MoveNext();
            }

            [Test]
            public void Throws_For_Null_Argument ()
            {
                Assert.Throws<ArgumentNullException>(() => CoroutineRunner.StopCoroutine(null));
            }
        }

        class StopAllCoroutines : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Dont_Call_MoveNext_For_All_Calls ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CoroutineRunner.StopAllCoroutines();

                call0.DidNotReceive().MoveNext();
                call1.DidNotReceive().MoveNext();
            }
        }

        class MoveNext : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Returns_False_If_No_Calls_Started ()
            {
                Assert.IsFalse(CoroutineRunner.MoveNext());
            }

            [Test]
            public void Calls_MoveNext_On_StartCoroutineCall ()
            {
                IStartCoroutineCall call = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(default).ReturnsForAnyArgs(call);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CoroutineRunner.MoveNext();

                call.Received().MoveNext();
            }

            [Test]
            public void Calls_MoveNext_On_StartCoroutineCall_For_All_Calls ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CoroutineRunner.MoveNext();

                call0.Received().MoveNext();
                call1.Received().MoveNext();
            }

            [Test]
            public void Returns_True_If_All_Calls_Returns_True ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                call0.MoveNext().Returns(true);
                call1.MoveNext().Returns(true);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                Assert.IsTrue(CoroutineRunner.MoveNext());
            }

            [Test]
            public void Returns_True_If_Any_Call_Returns_False ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                call0.MoveNext().Returns(true);
                call1.MoveNext().Returns(false);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                Assert.IsTrue(CoroutineRunner.MoveNext());
            }

            [Test]
            public void Returns_False_If_All_Calls_Returns_False ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                call0.MoveNext().Returns(false);
                call1.MoveNext().Returns(false);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                Assert.IsFalse(CoroutineRunner.MoveNext());
            }

            [Test]
            public void Redirects_Nested_StartCoroutine_Calls_To_Source_IStartCoroutineCall ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                CallFactory.Create(default).ReturnsForAnyArgs(call0);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CallFactory.Create(default).ReturnsForAnyArgs(call1);
                Coroutine nested = CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                call0.Current.Returns(nested);

                CoroutineRunner.MoveNext();

                call0.Received().SetNestedCoroutine(call1);
            }
        }

        class MoveNextAndExpect : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Fails_If_Null ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(null);
                CoroutineRunner.StartCoroutine(enumerator);

                Assert.Throws<AssertionException>(CoroutineRunner.MoveNextAndExpect<object>);
            }

            [Test]
            public void Fails_If_Not_Expected_Type ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(new WaitForSeconds(default));
                CoroutineRunner.StartCoroutine(enumerator);

                Assert.Throws<AssertionException>(
                    CoroutineRunner.MoveNextAndExpect<WaitForFixedUpdate>
                );
            }

            [Test]
            public void Pass_If_Is_Expected_Type ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(new WaitForSeconds(default));
                CoroutineRunner.StartCoroutine(enumerator);

                Assert.Throws<AssertionException>(
                    CoroutineRunner.MoveNextAndExpect<WaitForSeconds>
                );
            }

            [Test]
            public void Pass_If_Inherits_From_Expected_Type ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(new WaitForSeconds(default));
                CoroutineRunner.StartCoroutine(enumerator);

                Assert.Throws<AssertionException>(
                    CoroutineRunner.MoveNextAndExpect<YieldInstruction>
                );
            }

            [Test]
            public void Pass_If_Expected_Is_IEnumerator ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(enumerator);

                Assert.Throws<AssertionException>(
                    CoroutineRunner.MoveNextAndExpect<IEnumerator>
                );
            }

            [Test]
            public void Pass_If_Expected_Is_Coroutine ()
            {
                IEnumerator enumerator = Substitute.For<IEnumerator>();
                enumerator.Current.Returns(CoroutineFactory.Create(default));
                CoroutineRunner.StartCoroutine(enumerator);
                Assert.Throws<AssertionException>(
                    CoroutineRunner.MoveNextAndExpect<Coroutine>
                );
            }
        }

        class Reset : BaseCoroutineRunnerSubstituteTests
        {
            [Test]
            public void Calls_Reset_On_All_Calls ()
            {
                IStartCoroutineCall call0 = Substitute.For<IStartCoroutineCall>();
                IStartCoroutineCall call1 = Substitute.For<IStartCoroutineCall>();
                call1.Id.Returns(1);
                CallFactory.Create(default).ReturnsForAnyArgs(call0, call1);
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());
                CoroutineRunner.StartCoroutine(Substitute.For<IEnumerator>());

                CoroutineRunner.Reset();

                call0.Received().Reset();
                call1.Received().Reset();
            }
        }
    }
}
