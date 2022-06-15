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
                Counter.StartIEnumerator(5);

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
                Counter.StartCoroutine(5);

                Runner.MoveNext();
                Runner.MoveNext();
                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(3, counter);
            }
        }
    }
}
