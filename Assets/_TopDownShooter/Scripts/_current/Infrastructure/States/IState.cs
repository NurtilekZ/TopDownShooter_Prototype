namespace _current.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface ITickableState : IState
    {
        void Tick(float deltaTime);
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }

    public interface ITickablePayloadedState<TPayload> : IPayloadedState<TPayload>
    {
        void Tick(float deltaTime);
    }

    public interface IExitableState
    {
        void Exit();
    }
}