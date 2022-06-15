using System.Collections;
using NSubstitute;
using NUnit.Framework;

namespace CoroutineSubstitute.Samples
{
    public class ParallelCounterTests
    {
        class BaseParallelCounterTests
        {
            public ICoroutineRunner Runner { get; private set; }
            public ParallelCounter Counter { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = CoroutineSubstitute.Create();
                Counter = new ParallelCounter(Runner);
            }
        }

        class Start : BaseParallelCounterTests
        {
            [Test]
            public void MoveNext_Increment_Multiple_Counters ()
            {
                Counter.Start(5);

                Runner.MoveNext();

                foreach (int counter in Counter.Counters)
                    Assert.AreEqual(1, counter);
            }
        }
    }
}
