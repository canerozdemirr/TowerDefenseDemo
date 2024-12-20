using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Systems
{
    public abstract class BaseSystem : IInitializable, ITickable, IDisposable
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

        public virtual void Tick()
        {
            
        }

        public virtual void Dispose()
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
