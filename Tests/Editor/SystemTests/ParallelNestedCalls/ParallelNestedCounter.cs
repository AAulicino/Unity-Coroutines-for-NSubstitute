using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute.SystemTests
{
    public class ParallelNestedCounter
    {
        public int[] Counters { get; private set; }

        readonly ICoroutineRunner runner;
        Coroutine[] coroutines;

        public ParallelNestedCounter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void StartIEnumerators (int parallelCount)
        {
            Counters = new int[parallelCount];
            coroutines = new Coroutine[parallelCount];

            for (int i = 0; i < parallelCount; i++)
                coroutines[i] = runner.StartCoroutine(CounterRoutine(i));
        }

        public void StopIEnumerators ()
        {
            foreach (Coroutine coroutine in coroutines)
                runner.StopCoroutine(coroutine);
        }

        public void StartCoroutines (int parallelCount)
        {
            Counters = new int[parallelCount];
            coroutines = new Coroutine[parallelCount];

            for (int i = 0; i < parallelCount; i++)
                coroutines[i] = runner.StartCoroutine(CounterRoutineCoroutine(i));
        }

        public void StopCoroutines ()
        {
            foreach (Coroutine coroutine in coroutines)
                runner.StopCoroutine(coroutine);
        }

        IEnumerator CounterRoutine (int index)
        {
            Counters[index]++;
            yield return CounterRoutine(index);
        }

        IEnumerator CounterRoutineCoroutine (int index)
        {
            Counters[index]++;
            yield return runner.StartCoroutine(CounterRoutineCoroutine(index));
        }
    }
}
