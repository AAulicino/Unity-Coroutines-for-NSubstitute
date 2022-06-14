using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class NestedCounter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine coroutine;

        public NestedCounter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void Start ()
        {
            coroutine = runner.StartCoroutine(CounterRoutine());
        }

        public void Stop ()
        {
            runner.StopCoroutine(coroutine);
            coroutine = null;
        }

        IEnumerator CounterRoutine ()
        {
            Current++;
            yield return CounterRoutine();
        }
    }
}
