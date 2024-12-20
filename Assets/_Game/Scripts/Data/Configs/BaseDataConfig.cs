using System;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configs
{
    public class BaseDataConfig : ScriptableObject
    {
        [SerializeField] protected string _configID;
        
        [Button]
        public void GenerateConfigID()
        {
            _configID = Guid.NewGuid().ToString();
        }
    }
}
