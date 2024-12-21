using Events;
using Interfaces;
using Systems;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class LevelSystem : BaseSystem
    {
        private readonly IEventDispatcher _eventDispatcher;

        public LevelSystem(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public override void Initialize()
        {
            base.Initialize();
            _eventDispatcher.Subscribe<EnemyReachedToBaseEvent>(OnEnemyReach);
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventDispatcher.Unsubscribe<EnemyReachedToBaseEvent>(OnEnemyReach);
        }
        
        private void OnEnemyReach(EnemyReachedToBaseEvent enemyReachedToBaseEvent)
        {
            _eventDispatcher.Dispatch(new LevelFailedEvent());
        }
    }
}
