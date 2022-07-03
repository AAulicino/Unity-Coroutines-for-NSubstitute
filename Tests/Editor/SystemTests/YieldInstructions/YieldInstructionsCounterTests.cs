using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.SystemTests
{
    public class YieldInstructionsCounterTests
    {
        class BaseYieldInstructionsCounterTests
        {
            public ICoroutineRunner Runner { get; private set; }
            public YieldInstructionsCounter Counter { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Runner = CoroutineSubstitute.Create();
                Counter = new YieldInstructionsCounter(Runner);
            }
        }

        class MoveNextAndExpect_Generic : BaseYieldInstructionsCounterTests
        {
            [Test]
            public void Increments ()
            {
                Counter.Start(new WaitForSeconds(default));
                Runner.MoveNextAndExpect<WaitForSeconds>();
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void WaitForSeconds ()
            {
                Counter.Start(new WaitForSeconds(default));
                Runner.MoveNextAndExpect<WaitForSeconds>();
            }

            [Test]
            public void Base_Class ()
            {
                Counter.Start(new WaitForSeconds(default));
                Runner.MoveNextAndExpect<YieldInstruction>();
            }
        }

        class MoveNextAndExpect : BaseYieldInstructionsCounterTests
        {
            [Test]
            public void Increments ()
            {
                Counter.Start(new WaitForSeconds(1));

                Runner.MoveNextAndExpect(new WaitForSeconds(1));
                Assert.AreEqual(1, Counter.Current);
            }

            [Test]
            public void WaitForSeconds ()
            {
                Counter.Start(new WaitForSeconds(1));
                Runner.MoveNextAndExpect(new WaitForSeconds(1));
            }

            [Test]
            public void WaitForSecondsRealtime ()
            {
                Counter.Start(new WaitForSecondsRealtime(1));
                Runner.MoveNextAndExpect(new WaitForSecondsRealtime(1));
            }

            [Test]
            public void WaitForEndOfFrame ()
            {
                Counter.Start(new WaitForEndOfFrame());
                Runner.MoveNextAndExpect(new WaitForEndOfFrame());
            }

            [Test]
            public void WaitForFixedUpdate ()
            {
                Counter.Start(new WaitForFixedUpdate());
                Runner.MoveNextAndExpect(new WaitForFixedUpdate());
            }

            [Test]
            public void IEnumerator ()
            {
                Counter.Start(Substitute.For<IEnumerator>());
                Runner.MoveNextAndExpect(Substitute.For<IEnumerator>());
            }

            [Test]
            public void Null ()
            {
                Counter.Start(null);
                Runner.MoveNextAndExpect(null);
            }

            [Test]
            public void DummyType_Equals ()
            {
                CoroutineSubstitute.RegisterCustomType<DummyType>(
                    (expected, actual) => Assert.AreEqual(
                        ((DummyType)expected).Value,
                        ((DummyType)actual).Value
                    )
                );
                Counter.Start(new DummyType(1));
                Runner.MoveNextAndExpect(new DummyType(1));
            }

            class DummyType
            {
                public int Value;
                public DummyType (int value) => Value = value;
            }
        }
    }
}
