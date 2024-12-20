using Gameplay;
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
            Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
        }
    }
}
