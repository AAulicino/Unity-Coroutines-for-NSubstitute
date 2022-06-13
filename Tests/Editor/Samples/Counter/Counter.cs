using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class Counter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine coroutine;

        public Counter (ICoroutineRunner runner)
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
            while (true)
            {
                Current++;
                yield return null;
            }
        }
    }
}
