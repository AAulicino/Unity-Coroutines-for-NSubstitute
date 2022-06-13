using System;
using System.Reflection;
using UnityEngine;

namespace CoroutineSubstitute.Utils
{
    internal static class CoroutineFactory
    {
        public static Coroutine Create (int id)
        {
            Coroutine coroutine = (Coroutine)Activator.CreateInstance(
                typeof(Coroutine),
                BindingFlags.NonPublic | BindingFlags.Instance,
                default,
                default,
                default
            );
            GC.SuppressFinalize(coroutine);
            coroutine.SetId(id);
            return coroutine;
        }
    }
}
