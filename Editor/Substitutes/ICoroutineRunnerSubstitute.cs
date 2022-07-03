using System.Collections.Generic;
using CoroutineSubstitute.Substitutes.Call;

namespace CoroutineSubstitute.Substitutes
{
    public interface ICoroutineRunnerSubstitute : ICoroutineRunner
    {
        ICollection<IStartCoroutineCall> ActiveCoroutines { get; }
        bool MoveNext ();
        void Reset ();
    }
}
