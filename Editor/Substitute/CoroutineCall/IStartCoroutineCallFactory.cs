using System.Collections;

namespace CoroutineSubstitute.Call
{
    public interface IStartCoroutineCallFactory
    {
        IStartCoroutineCall Create (IEnumerator enumerator);
    }
}
