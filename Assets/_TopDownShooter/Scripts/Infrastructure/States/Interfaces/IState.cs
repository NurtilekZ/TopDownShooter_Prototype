namespace Infrastructure.States.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}