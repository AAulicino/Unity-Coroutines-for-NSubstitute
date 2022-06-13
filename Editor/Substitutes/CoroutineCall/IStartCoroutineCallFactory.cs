using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public interface IStartCoroutineCallFactory
    {
        IStartCoroutineCall Create (int id, IEnumerator enumerator);
    }
}
