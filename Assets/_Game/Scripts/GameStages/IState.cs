namespace _Game.Scripts.GameStages
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
    }
}