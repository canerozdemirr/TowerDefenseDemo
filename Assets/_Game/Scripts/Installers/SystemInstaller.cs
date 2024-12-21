using Interfaces.TowerInterfaces;
using Systems;
using Zenject;

namespace Installers
{
    public class SystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyWaveSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSystem>().AsSingle();
        }
    }
}