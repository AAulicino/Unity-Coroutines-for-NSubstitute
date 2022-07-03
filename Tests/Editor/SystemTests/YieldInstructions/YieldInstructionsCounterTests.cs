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

        class MoveNextAndExpect : BaseYieldInstructionsCounterTests
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
    }
}
