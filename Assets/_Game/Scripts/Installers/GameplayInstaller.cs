using Gameplay;
using Gameplay.Spawners;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] 
        private EnemySpawner _enemySpawner;

        [SerializeField] 
        private TowerSpawner _towerSpawner;

        [SerializeField] 
        private TowerPlatformController _towerPlatformController;
        
        public override void InstallBindings()
        {
            Container.Bind<IEventDispatcher>().To<EventDispatcher>().FromInstance(EventDispatcher.Instance).AsSingle();
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
            Container.Bind<ITowerSpawner>().To<TowerSpawner>().FromInstance(_towerSpawner).AsSingle();
            Container.BindInstance(_towerPlatformController).AsSingle();
            Container.BindInstance(Camera.main).AsSingle();
        }
    }
}
