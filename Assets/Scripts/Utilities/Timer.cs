using System;

namespace Utilities
{
    public class Timer
    {
        public event Action OnTimerEnded;
        
        private float _startTime;
        private float _duration;
        private float _targetTime;
        private bool _isActive;
        
        public Timer(float duration)
        {
            _duration = duration;
            _startTime = 0f;
        }

        public void StartTimer()
        {
            _isActive = true;
            _startTime = 0f;
            _targetTime = _duration;
        }
        public void StopTimer()
        {
            _isActive = false;
        }
        
        public void Tick(float deltaTime)
        {
            if (!_isActive) return;

            _startTime += deltaTime;
            if (_startTime >= _targetTime)
            {
                _isActive = false;
                OnTimerEnded?.Invoke();
            }
        }
        
    }
}