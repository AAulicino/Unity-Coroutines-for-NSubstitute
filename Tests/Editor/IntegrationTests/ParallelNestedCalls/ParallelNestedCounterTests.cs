using NUnit.Framework;

namespace CoroutineSubstitute.IntegrationTests
{
    public class ParallelNestedCounterTests
    {
        class BaseParallelNestedCounterTests
        {
            public ICoroutineRunner Runner { get; private set; }
            public ParallelNestedCounter Counter { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = CoroutineSubstitute.Create();
                Counter = new ParallelNestedCounter(Runner);
            }
        }

        class StartIEnumerator : BaseParallelNestedCounterTests
        {
            [Test]
            public void MoveNext_Increment_Multiple_Counters ()
            {
                Counter.StartIEnumerators(5);

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(3, counter);
            }
        }

        class StartCoroutine : BaseParallelNestedCounterTests
        {
            [Test]
            public void MoveNext_Increment_Multiple_Counters ()
            {
                Counter.StartCoroutines(5);

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(3, counter);
            }
        }

        class StopIEnumerator : BaseParallelNestedCounterTests
        {
            [Test]
            public void Stop_Stops_All_Counters ()
            {
                Counter.StartIEnumerators(5);
                Runner.MoveNext();

                Counter.StopIEnumerators();
                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(1, counter);
            }
        }

        class StopCoroutine : BaseParallelNestedCounterTests
        {
            [Test]
            public void Stop_Stops_All_Counters ()
            {
                Counter.StartCoroutines(5);
                Runner.MoveNext();

                Counter.StopCoroutines();
                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(1, counter);
            }
        }
    }
}
