using System.Collections;
using CoroutineSubstitute.Samples;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class SubstitutionTests
    {
        class BaseSubstitutionTests
        {
            public ICoroutineRunner Runner { get; private set; }

            public Counter Counter { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = CoroutineSubstitute.Create();
                Counter = new Counter(Runner);
            }
        }

        class StartCounter : BaseSubstitutionTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                Counter.StartCounter();
                Runner.Received().StartCoroutine(Arg.Any<IEnumerator>());
            }

            [Test]
            public void Starts_At_Zero ()
            {
                Counter.StartCounter();
                Assert.AreEqual(0, Counter.Current);
            }

            [Test]
            public void MoveNext_Increments_To_One ()
            {
                Counter.StartCounter();
                Runner.MoveNext();
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void Two_MoveNext_Increments_To_Two ()
            {
                Counter.StartCounter();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(2, Counter.Current);
            }

            [Test]
            public void Three_MoveNext_Increments_To_Three ()
            {
                Counter.StartCounter();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(2, Counter.Current);
            }
        }

        class StopCounter : BaseSubstitutionTests
        {
            [Test]
            public void Calls_StopCoroutine ()
            {
                Counter.StartCounter();
                Counter.StopCounter();
                Runner.ReceivedWithAnyArgs().StopCoroutine(Arg.Any<Coroutine>());
            }

            [Test]
            public void Stops_Incrementing ()
            {
                Counter.StartCounter();
                Counter.StopCounter();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(0, Counter.Current);
            }
        }
    }
}
