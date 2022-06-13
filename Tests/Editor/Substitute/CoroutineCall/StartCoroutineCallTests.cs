using System.Collections;
using CoroutineSubstitute.Substitutes.Call;
using NSubstitute;
using NUnit.Framework;

namespace CoroutineSubstitute.Tests.Substitutes.Call
{
    public class StartCoroutineCallTests
    {
        class BaseStartCoroutineCallTests
        {
            public const int ID = 1;
            public StartCoroutineCall Call { get; private set; }
            public IEnumerator Enumerator { get; private set; }

            [SetUp]
            public void Setup ()
            {
                Enumerator = Substitute.For<IEnumerator>();
                Call = new StartCoroutineCall(ID, Enumerator);
            }
        }

        class PublicProperties : BaseStartCoroutineCallTests
        {
            [Test]
            public void Id_Is_Set ()
            {
                Assert.AreEqual(ID, Call.Id);
            }

            [Test]
            public void Returns_Enumerator_Current ()
            {
                Enumerator.Current.Returns("Hello");
                Assert.AreEqual("Hello", Call.Current);
            }
        }

        class MoveNext : BaseStartCoroutineCallTests
        {
            [Test]
            public void Calls_Enumerator_MoveNext ()
            {
                Call.MoveNext();
                Enumerator.Received().MoveNext();
            }

            [Test]
            public void Returns_Enumerator_MoveNext_Result ()
            {
                Enumerator.MoveNext().Returns(true);
                Assert.IsTrue(Call.MoveNext());
            }
        }

        class Reset : BaseStartCoroutineCallTests
        {
            [Test]
            public void Calls_Enumerator_Reset ()
            {
                Call.Reset();
                Enumerator.Received().Reset();
            }
        }
    }
}
