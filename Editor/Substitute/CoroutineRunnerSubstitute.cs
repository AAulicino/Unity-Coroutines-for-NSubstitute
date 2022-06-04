using System.Collections;
using System.Collections.Generic;
using CoroutineSubstitute.Call;
using UnityEngine;

namespace CoroutineSubstitute
{
    public class CoroutineRunnerSubstitute : ICoroutineRunner, ICoroutineRunnerSubstitute
    {
        readonly IStartCoroutineCallFactory callFactory;
        readonly List<IStartCoroutineCall> startCoroutineCalls;

        public IReadOnlyList<IStartCoroutineCall> ReceivedStartCoroutineCalls => startCoroutineCalls;

        public CoroutineRunnerSubstitute (IStartCoroutineCallFactory callFactory)
        {
            this.callFactory = callFactory;
            startCoroutineCalls = new List<IStartCoroutineCall>();
        }

        public Coroutine StartCoroutine (IEnumerator enumerator)
        {
            IStartCoroutineCall call = callFactory.Create(startCoroutineCalls.Count, enumerator);
            startCoroutineCalls.Add(call);
            return null;
        }

        public void StopAllCoroutines ()
        {
            foreach (IStartCoroutineCall call in startCoroutineCalls)
                call.StopCalled();
        }

        public void StopCoroutine (Coroutine routine)
        {
        }

        public bool MoveNext ()
        {
            bool anySucceeded = false;
            foreach (IStartCoroutineCall call in startCoroutineCalls)
                anySucceeded |= call.MoveNext();
            return anySucceeded;
        }

        public void Reset ()
        {
            foreach (IStartCoroutineCall call in startCoroutineCalls)
                call.Reset();
        }
    }
}
