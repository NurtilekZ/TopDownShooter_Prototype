using _current.Core.Logic;

namespace _current.Core.Pawns.Components
{
    public interface IAnimationStateReader
    {
        AnimatorState State { get; }
        void OnEnter(int stateHash);
        void OnExit(int stateHash);
    }
}