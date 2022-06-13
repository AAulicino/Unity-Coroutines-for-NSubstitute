using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.Utils
{
    public class CoroutineExtensionsTests
    {
        class GetId
        {
            [Test]
            public void Returns_Coroutine_Id ()
            {
                Coroutine coroutine = CoroutineFactory.Create(1);
                Assert.AreEqual(1, CoroutineExtensions.GetId(coroutine));
            }
        }

        class SetId
        {
            [Test]
            public void Set_Id_Changes_Id ()
            {
                Coroutine coroutine = CoroutineFactory.Create(1);
                CoroutineExtensions.SetId(coroutine, 2);
                Assert.AreEqual(2, CoroutineExtensions.GetId(coroutine));
            }
        }
    }
}
