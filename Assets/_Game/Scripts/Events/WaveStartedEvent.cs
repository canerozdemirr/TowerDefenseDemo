using Interfaces;

namespace Events
{
    public struct WaveStartedEvent : IEvent
    {
        public int CurrentWaveIndex;

        public WaveStartedEvent(int currentWaveIndex)
        {
            CurrentWaveIndex = currentWaveIndex;
        }
    }
}
