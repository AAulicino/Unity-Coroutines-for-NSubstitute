using System.Collections;
using UnityEngine;

namespace CoroutineSubstitute
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine (IEnumerator routine);
        void StopCoroutine (Coroutine routine);
        void StopAllCoroutines ();
    }
}
