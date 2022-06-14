using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using UnityEngine;

namespace CoroutineSubstitute.Substitutes
{
    public class CoroutineRunnerSubstitute : ICoroutineRunner, ICoroutineRunnerSubstitute
    {
        static readonly IStartCoroutineCallFactory defaultCallFactory = new StartCoroutineCallFactory();

        readonly IStartCoroutineCallFactory callFactory;
        readonly Dictionary<int, IStartCoroutineCall> activeCoroutines;

        public CoroutineRunnerSubstitute () : this(defaultCallFactory)
        {
        }

        public CoroutineRunnerSubstitute (IStartCoroutineCallFactory callFactory)
        {
            this.callFactory = callFactory;
            activeCoroutines = new Dictionary<int, IStartCoroutineCall>();
        }

        public virtual Coroutine StartCoroutine (IEnumerator enumerator)
        {
            if (enumerator is null)
                throw new ArgumentNullException(nameof(enumerator));

            StackTrace trace = new StackTrace();
            MethodBase callingMethod = trace.GetFrame(1).GetMethod();

            IStartCoroutineCall call = callFactory.Create(
                activeCoroutines.Count,
                callingMethod.Name,
                enumerator
            );
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

        public bool MoveNext ()
        {
            bool anySucceeded = false;
            foreach (IStartCoroutineCall call in activeCoroutines.Values)
                anySucceeded |= call.MoveNext();
            return anySucceeded;
        }

        public void Reset ()
        {
            foreach (IStartCoroutineCall call in activeCoroutines.Values)
                call.Reset();
        }

        public IStartCoroutineCall FindCall (int callId)
        {
            if (!activeCoroutines.TryGetValue(callId, out IStartCoroutineCall call))
                throw new ArgumentException($"No call with index {callId}");
            return call;
        }

        public IStartCoroutineCall FindCall (Coroutine routine)
        {
            if (routine == null)
                throw new ArgumentNullException(nameof(routine));
            return FindCall(routine.GetId());
        }

        public IStartCoroutineCall FindCall (string callerMethod)
        {
            if (string.IsNullOrWhiteSpace(callerMethod))
            {
                throw new ArgumentException(
                    $"'{nameof(callerMethod)}' cannot be null or whitespace.", nameof(callerMethod)
                );
            }

            foreach (IStartCoroutineCall activeCall in activeCoroutines.Values)
            {
                if (activeCall.CallingMethodName == callerMethod)
                    return activeCall;
            }

            throw new ArgumentException($"No call from method '{callerMethod}'");
        }
    }
}
