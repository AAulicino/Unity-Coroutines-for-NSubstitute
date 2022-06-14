using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.Samples
{
    public class MultipleCounter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine coroutine;

        public MultipleCounter (ICoroutineRunner runner)
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
