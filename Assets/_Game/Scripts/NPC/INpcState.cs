using _Game.Scripts.NPC;

namespace _Game.Scripts.GameStages
{
    public interface INpcState
    {
        
        void Enter(NpcStateMachine stateMachine);
        void Exit();
        void Update();
        
        void OnDestroy();
        void OnNpcEnable();
        void OnNpcDisable();
    }
}