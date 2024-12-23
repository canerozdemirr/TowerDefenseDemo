using Data.Configs;
using NaughtyAttributes;
using UnityEngine;
using Utilities.TypeUtilities;

namespace Data.Configs.EnemyConfigs
{
    [CreateAssetMenu(fileName = "New Enemy Config",
        menuName = "Tower Defense/Configs/Enemy Config/New Enemy Config")]
    public class EnemyConfig : BaseDataConfig
    {
        [Foldout("Enemy Attributes")]
        [SerializeField] 
        private Enums.EnemyType _enemyType;
        
        [Foldout("Enemy Attributes")]
        [SerializeField] 
        private float _health;

        [Foldout("Enemy Attributes")]
        [SerializeField] 
        private float _speed;

        [SerializeField] 
        private GameObject _enemyPrefab;

        public float Health => _health;
        public float Speed => _speed;
        public Enums.EnemyType EnemyType => _enemyType;
        public GameObject EnemyPrefab => _enemyPrefab;
    }
}
