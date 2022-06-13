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

        public void StartCounter ()
        {
            coroutine = runner.StartCoroutine(CounterRoutine());
        }

        public void StopCounter ()
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
