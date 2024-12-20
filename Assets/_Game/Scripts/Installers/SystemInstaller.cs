using Systems;
using Zenject;

namespace Installers
{
    public class SystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyWaveSystem>().AsSingle();
        }
    }
}