using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.SystemTests
{
    public class ParallelCounter
    {
        public int[] Counters { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine[] coroutines;

        public ParallelCounter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void Start (int parallelCount)
        {
            Counters = new int[parallelCount];
            coroutines = new Coroutine[parallelCount];

            for (int i = 0; i < parallelCount; i++)
                coroutines[i] = runner.StartCoroutine(CounterRoutine(i));
        }

        public void Stop ()
        {
            foreach (Coroutine coroutine in coroutines)
                runner.StopCoroutine(coroutine);
        }

        IEnumerator CounterRoutine (int index)
        {
            while (true)
            {
                Counters[index]++;
                yield return null;
            }
        }
    }
}
