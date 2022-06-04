using System;

namespace CoroutineSubstitute
{
    // TODO find a way to handle multiple runners
    public class CoroutineRunnerRouter
    {
        CoroutineRunnerSubstitute _activeSubstitute;

        CoroutineRunnerSubstitute ActiveSubstitute
        {
            get
            {
                if (_activeSubstitute == null)
                {
                    throw new InvalidOperationException(
                        "ICoroutineRunner must be created using CoroutineSubstitute.Create()"
                    );
                }
                return _activeSubstitute;
            }
        }

        public void Register (CoroutineRunnerSubstitute substitute)
        {
            _activeSubstitute = substitute;
        }

        public void ValidateRunner (ICoroutineRunner runner)
        {
            if (runner != ActiveSubstitute)
            {
                throw new InvalidOperationException(
                    "No support for multiple coroutine runners in the same test"
                );
            }
        }

        public bool MoveNext (ICoroutineRunner runner)
        {
            ValidateRunner(runner);
            return ActiveSubstitute.MoveNext();
        }
    }
}
