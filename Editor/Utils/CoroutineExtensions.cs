using System;
using System.Reflection;
using UnityEngine;

namespace CoroutineSubstitute.Utils
{
    internal static class CoroutineExtensions
    {
        static readonly FieldInfo coroutinePtr = typeof(Coroutine).GetField(
            "m_Ptr",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        public static int GetId (this Coroutine coroutine)
            => ((IntPtr)coroutinePtr.GetValue(coroutine)).ToInt32();

        public static void SetId (this Coroutine coroutine, int id)
            => coroutinePtr.SetValue(coroutine, new IntPtr(id));
    }
}
