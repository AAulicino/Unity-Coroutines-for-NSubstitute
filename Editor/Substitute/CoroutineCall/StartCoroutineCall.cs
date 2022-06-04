using System;
using System.Collections;

namespace CoroutineSubstitute.Call
{
    public class StartCoroutineCall : IStartCoroutineCall, IEnumerator
    {
        readonly IEnumerator enumerator;

        public Guid CallId { get; }
        public object Current => enumerator.Current;

        public StartCoroutineCall (IEnumerator enumerator)
        {
            CallId = Guid.NewGuid();
            this.enumerator = enumerator;
        }

        public bool MoveNext () => enumerator.MoveNext();
        public void Reset () => enumerator.Reset();

        public void StopCalled ()
        {
            throw new NotImplementedException();
        }
    }
}
