using Data.Configs;
using Data.Configs.EnemyConfigs;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
    {
        [SerializeField] 
        private LevelWaveConfig _levelWaveConfig;

        [SerializeField] 
        private AllEnemyConfigs _allEnemyConfigs;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelWaveConfig>().FromInstance(_levelWaveConfig).AsSingle();
            Container.Bind<AllEnemyConfigs>().FromInstance(_allEnemyConfigs).AsSingle();
        }
    }
}