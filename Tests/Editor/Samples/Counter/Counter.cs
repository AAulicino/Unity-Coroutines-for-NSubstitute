using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class Counter
    {
        public int Current { get; private set; }

        public bool KeepRunning { get; set; } = true;

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
            while (KeepRunning)
            {
                Current++;
                yield return null;
            }
        }
    }
}
