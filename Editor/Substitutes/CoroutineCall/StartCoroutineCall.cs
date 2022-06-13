using System.Collections;

namespace CoroutineSubstitute.Substitutes.Call
{
    public class StartCoroutineCall : IStartCoroutineCall, IEnumerator
    {
        readonly IEnumerator enumerator;

        public int Id { get; }
        public object Current => enumerator.Current;

        public StartCoroutineCall (int id, IEnumerator enumerator)
        {
            Id = id;
            this.enumerator = enumerator;
        }

        public bool MoveNext () => enumerator.MoveNext();
        public void Reset () => enumerator.Reset();
    }
}
