using System;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI.Overlays
{
    public class TimerOverlay : BaseOverlay
    {
        [SerializeField] private TMP_Text textComp;
        
            private float _elapsedRunningTime = 0f;
            private float _runningStartTime = 0f;
            private float _pauseStartTime = 0f;
            private float _elapsedPausedTime = 0f;
            private float _totalElapsedPausedTime = 0f;
            private bool _running;
            private bool _paused;
        
            void Update()
            {
                if (_running)
                {
                    _elapsedRunningTime = Time.time - _runningStartTime - _totalElapsedPausedTime;
                    UpdateUI();
                }
                else if (_paused)
                {
                    _elapsedPausedTime = Time.time - _pauseStartTime;
                }
        
            }
            private void UpdateUI()
            {
                textComp.text = 
                    (GetTimeSeconds() > 40 ? "<color=red>": "")
                    + GetTime().ToString(@"mm\:ss\.fff");
            }
        
            public void Begin()
            {
                if (!_running && !_paused)
                {
                    _runningStartTime = Time.time;
                    _running = true;
                }
            }
        
            public void Pause()
            {
                if (_running && !_paused)
                {
                    _running = false;
                    _pauseStartTime = Time.time;
                    _paused = true;
                }
            }
        
            public void Unpause()
            {
                if (!_running && _paused)
                {
                    _totalElapsedPausedTime += _elapsedPausedTime;
                    _running = true;
                    _paused = false;
                }
            }
        
            public void ResetTime()
            {
                _elapsedRunningTime = 0f;
                _runningStartTime = 0f;
                _pauseStartTime = 0f;
                _elapsedPausedTime = 0f;
                _totalElapsedPausedTime = 0f;
                _running = false;
                _paused = false;
            }
        
            public int GetTimeSeconds()
            {
                return (int)_elapsedRunningTime;
            }
            public TimeSpan GetTime()
            {
                return TimeSpan.FromSeconds(_elapsedRunningTime);
            }
    }
}