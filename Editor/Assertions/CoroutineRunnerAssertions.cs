using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoroutineSubstitute.Substitutes;
using NUnit.Framework;
using UnityEngine;

namespace CoroutineSubstitute.Assertions
{
    public static class CoroutineRunnerAssertions
    {
        internal static readonly Dictionary<Type, TypeAssert> customTypes = new Dictionary<Type, TypeAssert>();

        public static void RegisterCustomType<T> (TypeAssert typeAssert)
        {
            customTypes[typeof(T)] = typeAssert;
        }

        public static void MoveNextAndExpect<T> (ICoroutineRunnerSubstitute runner)
        {
            if (runner.ActiveCoroutines.Count > 1)
            {
                throw new InvalidOperationException(
                    "MoveNextAndExpect currently only supports a single Coroutine instance"
                );
            }

            runner.MoveNext();
            object current = runner.ActiveCoroutines.Single().Current;

            if (current is T)
                return;

            Assert.Fail($"Expected: {typeof(T).Name}\nBut was: {GetTypeName(current)}");
        }

        public static void MoveNextAndExpect (ICoroutineRunnerSubstitute runner, object value)
        {
            if (runner.ActiveCoroutines.Count > 1)
            {
                throw new InvalidOperationException(
                    "MoveNextAndExpect currently only supports a single Coroutine instance"
                );
            }

            runner.MoveNext();
            object current = runner.ActiveCoroutines.Single().Current;

            AssertType(value, current);

            switch (value)
            {
                case WaitForSeconds val:
                    Assert.AreEqual(GetTotalSeconds(val), GetTotalSeconds(Cast(current, val)));
                    break;

                case WaitForSecondsRealtime val:
                    Assert.AreEqual(val.waitTime, Cast(current, val).waitTime);
                    break;

                case WaitForEndOfFrame _:
                case WaitForFixedUpdate _:
                case IEnumerator _:
                case null:
                    Assert.Pass();
                    break;

                default:
                    if (customTypes.TryGetValue(value.GetType(), out TypeAssert typeAssert))
                        typeAssert(value, current);
                    else
                        throw new NotSupportedException(
                            $"{GetTypeName(value)} is not supported. You can register your custom types"
                            + $" using {nameof(CoroutineSubstitute)}.{nameof(CoroutineSubstitute.RegisterCustomType)}."
                        );
                    break;
            }
        }

        static void AssertType (object expected, object actual)
        {
            if (expected?.GetType() != actual?.GetType())
                Assert.Fail($"Expected {GetTypeName(expected)} but got {GetTypeName(actual)}");
        }

        static string GetTypeName (object obj) => obj?.GetType()?.Name ?? "null";

        static T Cast<T> (object value, T _) => (T)value;

        static float GetTotalSeconds (WaitForSeconds waitForSeconds)
        {
            return (float)typeof(WaitForSeconds)
                .GetField("m_Seconds", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(waitForSeconds);
        }
    }
}
