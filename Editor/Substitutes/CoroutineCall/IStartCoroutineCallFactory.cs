using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public interface IStartCoroutineCallFactory
    {
        IStartCoroutineCall Create (int id, string calledMemberName, IEnumerator enumerator);
    }
}
