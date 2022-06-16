using NUnit.Framework;

namespace CoroutineSubstitute.SystemTests
{
    public class MultipleRunnersCounter
    {
        class BaseMultipleRunnersCounter
        {
            public ICoroutineRunner Runner1 { get; private set; }
            public ICoroutineRunner Runner2 { get; private set; }

            public ParallelNestedCounter Counter1 { get; private set; }
            public ParallelNestedCounter Counter2 { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner1 = CoroutineSubstitute.Create();
                Runner2 = CoroutineSubstitute.Create();

                Counter1 = new ParallelNestedCounter(Runner1);
                Counter2 = new ParallelNestedCounter(Runner2);
            }
        }

        class StartIEnumerator : BaseMultipleRunnersCounter
        {
            [Test]
            public void MoveNext_Increment_Multiple_Counters ()
            {
                Counter1.StartIEnumerators(5);
                Counter2.StartIEnumerators(5);

                for (int i = 0; i < 3; i++)
                {
                    Runner1.MoveNext();
                    Runner2.MoveNext();
                }

                foreach (int counter in Counter1.Counters)
                    Assert.AreEqual(3, counter);
                foreach (int counter in Counter2.Counters)
                    Assert.AreEqual(3, counter);
            }
        }

        class StartCoroutine : BaseMultipleRunnersCounter
        {
            [Test]
            public void MoveNext_Increment_Multiple_Counters ()
            {
                Counter1.StartCoroutines(5);
                Counter2.StartCoroutines(5);

                for (int i = 0; i < 3; i++)
                {
                    Runner1.MoveNext();
                    Runner2.MoveNext();
                }

                foreach (int counter in Counter1.Counters)
                    Assert.AreEqual(3, counter);
                foreach (int counter in Counter2.Counters)
                    Assert.AreEqual(3, counter);
            }
        }

        class StopIEnumerator : BaseMultipleRunnersCounter
        {
            [Test]
            public void Stop_Stops_All_Counters ()
            {
                Counter1.StartIEnumerators(5);
                Counter2.StartIEnumerators(5);
                Runner1.MoveNext();
                Runner2.MoveNext();

                Counter1.StopIEnumerators();
                Counter2.StopIEnumerators();
                Runner1.MoveNext();
                Runner2.MoveNext();

                foreach (int counter in Counter1.Counters)
                    Assert.AreEqual(1, counter);
                foreach (int counter in Counter2.Counters)
                    Assert.AreEqual(1, counter);
            }
        }

        class StopCoroutine : BaseMultipleRunnersCounter
        {
            [Test]
            public void Stop_Stops_All_Counters ()
            {
                Counter1.StartCoroutines(5);
                Counter2.StartCoroutines(5);
                Runner1.MoveNext();
                Runner2.MoveNext();

                Counter1.StopCoroutines();
                Counter2.StopCoroutines();
                Runner1.MoveNext();
                Runner2.MoveNext();

                foreach (int counter in Counter1.Counters)
                    Assert.AreEqual(1, counter);
                foreach (int counter in Counter2.Counters)
                    Assert.AreEqual(1, counter);
            }
        }
    }
}
