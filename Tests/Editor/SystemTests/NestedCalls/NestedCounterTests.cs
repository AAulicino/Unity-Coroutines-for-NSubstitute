using System.Collections;
using NSubstitute;
using NUnit.Framework;

namespace CoroutineSubstitute.SystemTests
{
    public class NestedCounterTests
    {
        class BaseNestedCounterTests
        {
            public ICoroutineRunner Runner { get; private set; }
            public NestedCounter Counter { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = CoroutineSubstitute.Create();
                Counter = new NestedCounter(Runner);
            }
        }

        class StartNestedIEnumerator : BaseNestedCounterTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                Counter.StartNestedIEnumerator();
                Runner.Received().StartCoroutine(Arg.Any<IEnumerator>());
            }

            [Test]
            public void Starts_At_Zero ()
            {
                Counter.StartNestedIEnumerator();
                Assert.AreEqual(0, Counter.Current);
            }

            [Test]
            public void MoveNext_Increments_To_One ()
            {
                Counter.StartNestedIEnumerator();
                Runner.MoveNext();
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void Two_MoveNext_Increments_To_Two ()
            {
                Counter.StartNestedIEnumerator();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(2, Counter.Current);
            }

            [Test]
            public void Three_MoveNext_Increments_To_Three ()
            {
                Counter.StartNestedIEnumerator();

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(3, Counter.Current);
            }
        }

        class StartNestedCoroutine : BaseNestedCounterTests
        {
            [Test]
            public void Calls_StartCoroutine ()
            {
                Counter.StartNestedCoroutine();
                Runner.Received().StartCoroutine(Arg.Any<IEnumerator>());
            }

            [Test]
            public void Starts_At_Zero ()
            {
                Counter.StartNestedCoroutine();
                Assert.AreEqual(0, Counter.Current);
            }

            [Test]
            public void MoveNext_Increments_To_One ()
            {
                Counter.StartNestedCoroutine();
                Runner.MoveNext();
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void Two_MoveNext_Increments_To_Two ()
            {
                Counter.StartNestedCoroutine();

                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(2, Counter.Current);
            }

            [Test]
            public void Three_MoveNext_Increments_To_Three ()
            {
                Counter.StartNestedCoroutine();

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                Assert.AreEqual(3, Counter.Current);
            }
        }

        class StopIEnumerator : BaseNestedCounterTests
        {
            [Test]
            public void Stop_Stops_Counter ()
            {
                Counter.StartNestedIEnumerator();
                Runner.MoveNext();

                Counter.StopNestedIEnumerator();
                Runner.MoveNext();

                Assert.AreEqual(1, Counter.Current);
            }
        }

        class StopCoroutine : BaseNestedCounterTests
        {
            [Test]
            public void Stop_Stops_Counter ()
            {
                Counter.StartNestedCoroutine();
                Runner.MoveNext();

                Counter.StopNestedCoroutine();
                Runner.MoveNext();

                Assert.AreEqual(1, Counter.Current);
            }
        }
    }
}
