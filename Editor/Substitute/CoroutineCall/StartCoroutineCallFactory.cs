using System.Collections;

namespace CoroutineSubstitute.Call
{
    public class StartCoroutineCallFactory : IStartCoroutineCallFactory
    {
        public IStartCoroutineCall Create (IEnumerator enumerator)
        {
            return new StartCoroutineCall(enumerator);
        }
    }
}
