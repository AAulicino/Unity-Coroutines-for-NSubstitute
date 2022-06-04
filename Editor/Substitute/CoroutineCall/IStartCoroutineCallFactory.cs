using System.Collections;

namespace CoroutineSubstitute.Call
{
    public interface IStartCoroutineCallFactory
    {
        IStartCoroutineCall Create (int id, IEnumerator enumerator);
    }
}
