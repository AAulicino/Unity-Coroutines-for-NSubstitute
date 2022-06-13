using System;
using System.Collections;
using System.Collections.Generic;
using CoroutineSubstitute.Call;
using UnityEngine;

namespace CoroutineSubstitute
{
    public class CoroutineRunnerSubstitute : ICoroutineRunner, ICoroutineRunnerSubstitute
    {
        readonly IStartCoroutineCallFactory callFactory;
        readonly Dictionary<int, IStartCoroutineCall> activeCoroutines;

        public CoroutineRunnerSubstitute (IStartCoroutineCallFactory callFactory)
        {
            this.callFactory = callFactory;
            activeCoroutines = new Dictionary<int, IStartCoroutineCall>();
        }

        public virtual Coroutine StartCoroutine (IEnumerator enumerator)
        {
            IStartCoroutineCall call = callFactory.Create(activeCoroutines.Count, enumerator);
            activeCoroutines.Add(call.Id, call);
            return CoroutineFactory.Create(call.Id);
        }

        public virtual void StopAllCoroutines ()
        {
            activeCoroutines.Clear();
        }

        public virtual void StopCoroutine (Coroutine routine)
        {
            if (routine == null)
                throw new ArgumentNullException(nameof(routine));

            activeCoroutines.Remove(routine.GetId());
        }

        public virtual bool MoveNext ()
        {
            bool anySucceeded = false;
            foreach (IStartCoroutineCall call in activeCoroutines.Values)
                anySucceeded |= call.MoveNext();
            return anySucceeded;
        }

        public virtual void Reset ()
        {
            foreach (IStartCoroutineCall call in activeCoroutines.Values)
                call.Reset();
        }
    }
}
