using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoroutineSubstitute.Substitutes.Call;
using CoroutineSubstitute.Utils;
using UnityEngine;

namespace CoroutineSubstitute.Substitutes
{
    public class CoroutineRunnerSubstitute : ICoroutineRunner, ICoroutineRunnerSubstitute
    {
        public ICollection<IStartCoroutineCall> ActiveCoroutines => activeCoroutinesDict.Values;

        static readonly IStartCoroutineCallFactory defaultCallFactory = new StartCoroutineCallFactory();

        readonly Dictionary<int, IStartCoroutineCall> activeCoroutinesDict;
        readonly IStartCoroutineCallFactory callFactory;

        public CoroutineRunnerSubstitute () : this(defaultCallFactory)
        {
        }

        public CoroutineRunnerSubstitute (IStartCoroutineCallFactory callFactory)
        {
            this.callFactory = callFactory;
            activeCoroutinesDict = new Dictionary<int, IStartCoroutineCall>();
        }

        public virtual Coroutine StartCoroutine (IEnumerator enumerator)
        {
            if (enumerator is null)
                throw new ArgumentNullException(nameof(enumerator));

            IStartCoroutineCall call = callFactory.Create(enumerator);
            activeCoroutinesDict.Add(call.Id, call);
            return CoroutineFactory.Create(call.Id);
        }

        public virtual void StopAllCoroutines ()
        {
            activeCoroutinesDict.Clear();
        }

        public virtual void StopCoroutine (Coroutine routine)
        {
            if (routine == null)
                throw new ArgumentNullException(nameof(routine));

            activeCoroutinesDict.Remove(routine.GetId());
        }

        public virtual bool MoveNext ()
        {
            bool anySucceeded = false;
            HashSet<int> callsToRemove = new HashSet<int>();

            foreach (IStartCoroutineCall call in activeCoroutinesDict.Values.ToArray())
            {
                bool result = call.MoveNext();

                if (callsToRemove.Contains(call.Id))
                    continue;

                if (call.Current is Coroutine coroutine)
                {
                    int coroutineId = coroutine.GetId();
                    call.SetNestedCoroutine(activeCoroutinesDict[coroutineId]);
                    callsToRemove.Add(coroutineId);
                }
                anySucceeded |= result;
            }

            foreach (int id in callsToRemove)
                activeCoroutinesDict.Remove(id);

            return anySucceeded;
        }

        public virtual void Reset ()
        {
            foreach (IStartCoroutineCall call in activeCoroutinesDict.Values)
                call.Reset();
        }
    }
}
