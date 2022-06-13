namespace CoroutineSubstitute.Substitutes
{
    public interface ICoroutineRunnerSubstitute : ICoroutineRunner
    {
        bool MoveNext ();
        void Reset ();
    }
}
