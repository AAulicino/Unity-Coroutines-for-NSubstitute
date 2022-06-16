using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public interface IStartCoroutineCallFactory
    {
        IStartCoroutineCall Create (IEnumerator enumerator);
    }
}
