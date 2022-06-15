using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class NestedCounter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine nestedIEnumerator;
        Coroutine nestedCoroutine;

        public NestedCounter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void StartNestedIEnumerator ()
        {
            nestedIEnumerator = runner.StartCoroutine(CounterRoutineIEnumerator());
        }

        public void StopNestedIEnumerator ()
        {
            runner.StopCoroutine(nestedIEnumerator);
            nestedIEnumerator = null;
        }

        public void StartNestedCoroutine ()
        {
            nestedCoroutine = runner.StartCoroutine(CounterRoutineCoroutine());
        }

        public void StopNestedCoroutine ()
        {
            runner.StopCoroutine(nestedCoroutine);
            nestedCoroutine = null;
        }

        IEnumerator CounterRoutineIEnumerator ()
        {
            Current++;
            yield return CounterRoutineIEnumerator();
        }

        IEnumerator CounterRoutineCoroutine ()
        {
            Current++;
            yield return runner.StartCoroutine(CounterRoutineCoroutine());
        }
    }
}
