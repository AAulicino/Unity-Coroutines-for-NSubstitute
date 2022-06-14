using System.Collections;
using CoroutineSubstitute.Utils;
using UnityEngine;

namespace CoroutineSubstitute.Substitutes.Call
{
    public class StartCoroutineCall : IStartCoroutineCall, IEnumerator
    {
        public int Id { get; }
        public object Current => nestedCoroutine ?? nestedCall?.Current ?? enumerator.Current;

        readonly IEnumerator enumerator;
        readonly IStartCoroutineCallFactory callFactory;
        IStartCoroutineCall nestedCall;
        Coroutine nestedCoroutine;

        public StartCoroutineCall (
            int id,
            IEnumerator enumerator,
            IStartCoroutineCallFactory callFactory
        )
        {
            Id = id;
            this.enumerator = enumerator;
            this.callFactory = callFactory;
        }

        public bool MoveNext ()
        {
            if (nestedCall != null)
            {
                if (nestedCall.MoveNext())
                    return true;
                nestedCall = null;
            }

            if (enumerator.MoveNext())
            {
                if (Current is IEnumerator enumerator)
                    nestedCall = callFactory.Create(enumerator);
                else if (Current is Coroutine coroutine)
                    nestedCoroutine = coroutine;
                return true;
            }
            return false;
        }

        public void SetNestedCoroutine (IStartCoroutineCall call)
        {
            Debug.Assert(nestedCoroutine.GetId() == call.Id);
            nestedCall = call;
            nestedCoroutine = null;
        }

        public void Reset () => enumerator.Reset();
    }
}
