using System;
using System.Collections;

namespace CoroutineSubstitute.Call
{
    public interface IStartCoroutineCall : IEnumerator
    {
        int Id { get; }

        void StopCalled ();
    }
}
