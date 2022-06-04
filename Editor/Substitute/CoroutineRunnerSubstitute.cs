using System;
using System.Collections;
using System.Collections.Generic;
using CoroutineSubstitute.Call;
using UnityEngine;

namespace CoroutineSubstitute
{
    public class CoroutineRunnerSubstitute : ICoroutineRunner
    {
        readonly IStartCoroutineCallFactory callFactory;

        readonly Dictionary<Guid, IStartCoroutineCall> calls =
            new Dictionary<Guid, IStartCoroutineCall>();

        public CoroutineRunnerSubstitute (IStartCoroutineCallFactory callFactory)
        {
            this.callFactory = callFactory;
        }

        public Coroutine StartCoroutine (IEnumerator enumerator)
        {
            IStartCoroutineCall call = callFactory.Create(enumerator);
            calls.Add(call.CallId, call);
            return null;  // TODO find a way to workaround unity Coroutine class
        }

        public void StopAllCoroutines ()
        {
            foreach (IStartCoroutineCall call in calls.Values)
                call.StopCalled();
        }

        public void StopCoroutine (Coroutine routine)
        {
            // TODO find a way to workaround unity Coroutine class
        }
    }
}
