using System.Collections;

namespace CoroutineSubstitute.Examples
{
    public class Counter
    {
        public int Current { get; private set; }

        readonly ICoroutineRunner runner;

        public Counter (ICoroutineRunner runner)
        {
            this.runner = runner;
        }

        public void StartCounter ()
        {
            runner.StartCoroutine(CounterRoutine());
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
