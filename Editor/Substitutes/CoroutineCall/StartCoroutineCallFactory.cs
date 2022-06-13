using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public class StartCoroutineCallFactory : IStartCoroutineCallFactory
    {
        public IStartCoroutineCall Create (int id, IEnumerator enumerator)
        {
            return new StartCoroutineCall(id, enumerator);
        }
    }
}
