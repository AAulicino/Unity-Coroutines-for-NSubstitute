using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public interface IStartCoroutineCall : IEnumerator
    {
        int Id { get; }
        string CallingMethodName { get; }
    }
}
