using UniRx;

namespace UI.ViewStateMachine
{
    public class ViewStateMachine
    {
        public ReactiveProperty<IViewState> CurrentState { get; }

        public ViewStateMachine()
        {
            CurrentState = new ReactiveProperty<IViewState>();
        }

        public void ChangeState(IViewState newState)
        {
            CurrentState.Value?.Exit();
            CurrentState.Value = newState;
            CurrentState.Value?.Enter();
        }
    }
}