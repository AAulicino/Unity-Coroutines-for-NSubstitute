using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.SystemTests
{
    public class YieldInstructionsCounter
    {
        public int Current { get; private set; }

        public bool KeepRunning { get; set; } = true;

        readonly ICoroutineRunner runner;
        Coroutine coroutine;

        public YieldInstructionsCounter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void Start (object customReturn)
        {
            coroutine = runner.StartCoroutine(CounterRoutine(customReturn));
        }

        public void Stop ()
        {
            runner.StopCoroutine(coroutine);
            coroutine = null;
        }

        IEnumerator CounterRoutine (object customReturn)
        {
            while (KeepRunning)
            {
                Current++;
                yield return customReturn;
            }
        }
    }
}
