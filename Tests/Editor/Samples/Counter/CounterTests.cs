using System.Collections;
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

        class Start : BaseSubstitutionTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                Counter.Start();
                Runner.Received().StartCoroutine(Arg.Any<IEnumerator>());
            }

            [Test]
            public void Starts_At_Zero ()
            {
                Counter.Start();
                Assert.AreEqual(0, Counter.Current);
            }

            [Test]
            public void MoveNext_Increments_To_One ()
            {
                Counter.Start();
                Runner.MoveNext();
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void Two_MoveNext_Increments_To_Two ()
            {
                Counter.Start();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(2, Counter.Current);
            }

            [Test]
            public void Three_MoveNext_Increments_To_Three ()
            {
                Counter.Start();

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(3, Counter.Current);
            }
        }

        class Stop : BaseSubstitutionTests
        {
            [Test]
            public void Calls_StopCoroutine ()
            {
                Counter.Start();
                Counter.Stop();
                Runner.ReceivedWithAnyArgs().StopCoroutine(Arg.Any<Coroutine>());
            }

            [Test]
            public void Stops_Incrementing ()
            {
                Counter.Start();
                Counter.Stop();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(0, Counter.Current);
            }
        }
    }
}
