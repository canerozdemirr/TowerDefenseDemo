using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    public abstract class BaseSystem : ISystem
    {
        private bool _isInitialized;
        
        public virtual void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning($"{nameof(BaseSystem)} is already initialized.");
                return;
            }
            _isInitialized = true;
        }

        public void DeInitialize()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"{nameof(BaseSystem)} is not initialized or already deinitialized.");
                return;
            }

            _isInitialized = false;
        }
    }
}
