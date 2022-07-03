using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using NUnit.Framework;
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

            IStartCoroutineCall call = callFactory.Create(enumerator);
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
            HashSet<int> callsToRemove = new HashSet<int>();

            foreach (IStartCoroutineCall call in activeCoroutines.Values.ToArray())
            {
                bool result = call.MoveNext();

                if (callsToRemove.Contains(call.Id))
                    continue;

                if (call.Current is Coroutine coroutine)
                {
                    int coroutineId = coroutine.GetId();
                    call.SetNestedCoroutine(activeCoroutines[coroutineId]);
                    callsToRemove.Add(coroutineId);
                }
                anySucceeded |= result;
            }

            foreach (int id in callsToRemove)
                activeCoroutines.Remove(id);

            return anySucceeded;
        }

        public virtual void MoveNextAndExpect<T> ()
        {
            if (activeCoroutines.Count > 1)
            {
                throw new InvalidOperationException(
                    "MoveNextAndExpect currently only supports a single Coroutine instance"
                );
            }

            MoveNext();
            object current = activeCoroutines.Single().Value.Current;

            if (current is T)
                return;

            Assert.Fail($"Expected: {typeof(T).Name}\nBut was: {GetTypeName(current)}");
        }

        public virtual void Reset ()
        {
            foreach (IStartCoroutineCall call in activeCoroutines.Values)
                call.Reset();
        }

        string GetTypeName (object obj) => obj?.GetType()?.Name ?? "null";
    }
}
