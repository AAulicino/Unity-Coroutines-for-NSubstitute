using System;
using System.Collections;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.Tests.Substitutes.Call
{
    public class StartCoroutineCallTests
    {
        class BaseStartCoroutineCallTests
        {
            public const int ID = 1;
            public StartCoroutineCall Call { get; private set; }
            public IEnumerator Enumerator { get; private set; }
            public IStartCoroutineCallFactory CallFactory { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Enumerator = Substitute.For<IEnumerator>();
                CallFactory = Substitute.For<IStartCoroutineCallFactory>();
                Call = new StartCoroutineCall(ID, Enumerator, CallFactory);
            }
        }

        class PublicProperties : BaseStartCoroutineCallTests
        {
            [Test]
            public void Id_Is_Set ()
            {
                Assert.AreEqual(ID, Call.Id);
            }

            [Test]
            public void Returns_Enumerator_Current ()
            {
                Enumerator.Current.Returns("Hello");
                Assert.AreEqual("Hello", Call.Current);
            }
        }

        class MoveNext : BaseStartCoroutineCallTests
        {
            [Test]
            public void Calls_Enumerator_MoveNext ()
            {
                Call.MoveNext();
                Enumerator.Received().MoveNext();
            }

            [Test]
            public void Returns_Enumerator_MoveNext_Result ()
            {
                Enumerator.MoveNext().Returns(true);
                Assert.IsTrue(Call.MoveNext());
            }

            [Test]
            public void Creates_StartCoroutineCall_For_Nested_IEnumerator ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);

                Call.MoveNext();

                CallFactory.Received().Create(nested);
            }

            [Test]
            public void Calls_MoveNext_On_Nested_IEnumerator ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);
                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(nested).Returns(nestedCall);

                Call.MoveNext();
                Call.MoveNext();

                nestedCall.Received().MoveNext();
            }

            [Test]
            public void Does_Not_Call_MoveNext_On_Base_While_Running_Nested_IEnumerator ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);
                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                nestedCall.MoveNext().Returns(true);
                CallFactory.Create(nested).Returns(nestedCall);
                Call.MoveNext();
                Enumerator.ClearReceivedCalls();

                Call.MoveNext();

                Enumerator.DidNotReceive().MoveNext();
            }

            [Test]
            public void Calls_MoveNext_On_Base_After_Nested_MoveNext_Returns_False ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);
                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(nested).Returns(nestedCall);

                Call.MoveNext();
                nestedCall.MoveNext().Returns(false);

                Call.MoveNext();

                Enumerator.Received().MoveNext();
            }

            [Test]
            public void Does_Not_Call_MoveNext_On_Nested_After_Nested_MoveNext_Returns_False ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);
                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(nested).Returns(nestedCall);
                Call.MoveNext();
                nestedCall.MoveNext().Returns(false);
                Enumerator.Current.Returns(null);
                Call.MoveNext();
                nestedCall.ClearReceivedCalls();

                Call.MoveNext();

                nestedCall.DidNotReceive().MoveNext();
            }

            [Test]
            public void Calls_MoveNext_On_Nested_After_Nested_MoveNext_Returns_True ()
            {
                IEnumerator nested = Substitute.For<IEnumerator>();
                Enumerator.MoveNext().Returns(true);
                Enumerator.Current.Returns(nested);
                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                CallFactory.Create(nested).Returns(nestedCall);
                Call.MoveNext();
                nestedCall.MoveNext().Returns(true);

                Call.MoveNext();
                Call.MoveNext();

                nestedCall.Received(2).MoveNext();
            }
        }

        class SetNestedCoroutine : BaseStartCoroutineCallTests
        {
            [Test]
            public void Calls_MoveNext_On_Set_NestedRoutine ()
            {
                Coroutine nested = CoroutineFactory.Create(ID + 1);
                Call.MoveNext().Returns(true);
                Call.Current.Returns(nested);
                Call.MoveNext();

                IStartCoroutineCall nestedCall = Substitute.For<IStartCoroutineCall>();
                nestedCall.Id.Returns(ID + 1);
                Call.SetNestedCoroutine(nestedCall);

                Call.MoveNext();

                nestedCall.Received().MoveNext();
            }

            [Test]
            public void Passes_Call_To_Nested_Calls ()
            {
                Coroutine child = CoroutineFactory.Create(ID + 1);
                Call.MoveNext().Returns(true);
                Call.Current.Returns(child);
                Call.MoveNext();

                IStartCoroutineCall childCall = Substitute.For<IStartCoroutineCall>();
                childCall.Id.Returns(ID + 1);
                childCall.MoveNext().Returns(true);
                Coroutine grandChild = CoroutineFactory.Create(ID + 2);
                childCall.Current.Returns(grandChild);
                Call.SetNestedCoroutine(childCall);
                Call.MoveNext();

                IStartCoroutineCall grandChildCall = Substitute.For<IStartCoroutineCall>();
                grandChildCall.Id.Returns(ID + 2);
                Call.SetNestedCoroutine(grandChildCall);

                Call.MoveNext();

                childCall.Received().SetNestedCoroutine(grandChildCall);
            }

            [Test]
            public void Throws_If_Received_Unexpected_Call ()
            {
                IStartCoroutineCall nested = Substitute.For<IStartCoroutineCall>();
                Assert.Throws<InvalidOperationException>(() => Call.SetNestedCoroutine(nested));
            }

            [Test]
            public void Throws_If_Received_NonMatching_Call ()
            {
                IStartCoroutineCall nested = Substitute.For<IStartCoroutineCall>();
                nested.Id.Returns(ID + 1);
                Enumerator.Current.Returns(CoroutineFactory.Create(ID + 2));
                Enumerator.MoveNext().Returns(true);

                Call.MoveNext();

                Assert.Throws<InvalidOperationException>(() => Call.SetNestedCoroutine(nested));
            }
        }

        class Reset : BaseStartCoroutineCallTests
        {
            [Test]
            public void Calls_Enumerator_Reset ()
            {
                Call.Reset();
                Enumerator.Received().Reset();
            }
        }
    }
}
