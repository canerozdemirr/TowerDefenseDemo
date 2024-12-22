using System;
using Data.Configs;
using Events;
using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class WaveNumberText : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _waveText;

        private IEventDispatcher _eventDispatcher;
        private LevelWaveConfig _levelWaveConfig;

        [Inject]
        public void Inject(IEventDispatcher eventDispatcher, LevelWaveConfig levelWaveConfig)
        {
            _eventDispatcher = eventDispatcher;
            _levelWaveConfig = levelWaveConfig;
        }
        
        private void OnEnable()
        {
            _eventDispatcher.Subscribe<WaveStartedEvent>(OnWaveStarted);
            _eventDispatcher.Subscribe<LevelFailedEvent>(OnLevelFailed);
            _waveText.SetText($"Wave number: 0 / {_levelWaveConfig.EnemyWaveConfigList.Count}");
        }

        private void OnDestroy()
        {
            _eventDispatcher.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
            _eventDispatcher.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
        }

        private void OnLevelFailed(LevelFailedEvent levelFailedEvent)
        {
            _waveText.SetText($"Wave number: 0 / {_levelWaveConfig.EnemyWaveConfigList.Count}");
        }

        private void OnWaveStarted(WaveStartedEvent waveStartedEvent)
        {
            _waveText.SetText($"Wave number: {waveStartedEvent.CurrentWaveIndex} / {_levelWaveConfig.EnemyWaveConfigList.Count}");
        }
    }
}
