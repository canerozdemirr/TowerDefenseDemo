using Gameplay.Towers;

namespace Interfaces.StateInterfaces
{
    public interface ITowerState<T> where T : BaseTower
    {
        void Enter(T tower);
        void Execute(T tower);
        void Exit(T tower);
    }
}
