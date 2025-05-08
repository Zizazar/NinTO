namespace _Game.Scripts.GameStages
{
    public class GameStateMachine
    {
        private IState _currentState;
    
        public IState CurrentState => _currentState;

        // <summary>
        // Generic класс для смены состояния.
        // </summary>
        public void ChangeState<T>() where T : IState, new()
        {
            _currentState?.Exit();
            _currentState = new T();
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}