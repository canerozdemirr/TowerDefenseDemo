using Data.Configs;
using Data.Configs.EnemyConfigs;
using Data.Configs.ProjectileConfigs;
using Data.Configs.TowerConfigs;
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

        [SerializeField] 
        private TowerConfigList _towerConfigList;

        [SerializeField] 
        private ProjectileListConfig _projectileListConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelWaveConfig>().FromInstance(_levelWaveConfig).AsSingle();
            Container.Bind<AllEnemyConfigs>().FromInstance(_allEnemyConfigs).AsSingle();
            Container.Bind<TowerConfigList>().FromInstance(_towerConfigList).AsSingle();
            Container.Bind<ProjectileListConfig>().FromInstance(_projectileListConfig).AsSingle();
        }
    }
}