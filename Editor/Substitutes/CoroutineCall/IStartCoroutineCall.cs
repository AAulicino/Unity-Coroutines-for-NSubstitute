using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public interface IStartCoroutineCall : IEnumerator
    {
        int Id { get; }
        void SetNestedCoroutine (IStartCoroutineCall call);
    }
}
