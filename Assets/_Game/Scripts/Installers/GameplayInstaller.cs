using Gameplay;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] 
        private EnemySpawner _enemySpawner;
        public override void InstallBindings()
        {
            Container.Bind<IEventDispatcher>().To<EventDispatcher>().FromInstance(EventDispatcher.Instance).AsSingle();
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
            Container.BindInstance(Camera.main).AsSingle();
        }
    }
}
