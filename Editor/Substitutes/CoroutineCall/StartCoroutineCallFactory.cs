using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public class StartCoroutineCallFactory : IStartCoroutineCallFactory
    {
        int id;

        public IStartCoroutineCall Create (IEnumerator enumerator)
        {
            return new StartCoroutineCall(id++, enumerator, this);
        }
    }
}
