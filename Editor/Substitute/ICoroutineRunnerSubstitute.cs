namespace CoroutineSubstitute
{
    public interface ICoroutineRunnerSubstitute : ICoroutineRunner
    {
        bool MoveNext ();
        void Reset ();
    }
}
