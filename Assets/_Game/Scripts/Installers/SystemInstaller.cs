using Interfaces.TowerInterfaces;
using Systems;
using Zenject;

namespace Installers
{
    public class SystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputSystem>().AsSingle();
            Container.Bind<ITowerPlacer>().To<TowerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyWaveSystem>().AsSingle();
        }
    }
}