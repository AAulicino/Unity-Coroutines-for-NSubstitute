using System;
using System.Collections;

namespace CoroutineSubstitute.Call
{
    public interface IStartCoroutineCall : IEnumerator
    {
        Guid CallId { get; }

        void StopCalled ();
    }
}
