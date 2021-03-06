using CoroutineSubstitute.Utils;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.UnitTests.Utils
{
    public class CoroutineFactoryTests
    {
        [Test]
        public void Returns_Coroutine_With_Id ()
        {
            Coroutine created = CoroutineFactory.Create(1);
            Assert.AreEqual(1, created.GetId());
        }
    }
}
