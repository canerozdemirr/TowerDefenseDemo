using Data.Configs;
using NaughtyAttributes;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Data.Configs.TowerConfigs
{
    [CreateAssetMenu(fileName = "New Tower Config",
        menuName = "Tower Defense/Configs/Tower Config/New Tower Config")]
    public class TowerConfig : BaseDataConfig
    {
        [Foldout("Tower Attributes")] 
        [SerializeField]
        private Enums.TowerType _towerType;
        
        [Foldout("Tower Attributes")] 
        [SerializeField]
        private float _towerBaseDamage;

        [Foldout("Tower Attributes")] 
        [SerializeField] 
        private float _delayBetweenEachFiring;
        
        [Foldout("Tower Attributes")] 
        [SerializeField] 
        private float _towerCheckRadius;
        
        [Foldout("Tower Attributes")] 
        [SerializeField] 
        private GameObject _towerPrefab;

        public Enums.TowerType TowerType => _towerType;
        public float TowerBaseDamage => _towerBaseDamage;
        public float DelayBetweenEachFiring => _delayBetweenEachFiring;
        public float TowerCheckRadius => _towerCheckRadius;
        public GameObject TowerPrefab => _towerPrefab;
    }
}
