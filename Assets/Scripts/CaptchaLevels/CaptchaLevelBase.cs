using System;
using System.Collections;
using UnityEngine;

namespace CaptchaGame
{
    public abstract class CaptchaLevelBase : MonoBehaviour
    {
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted == value) return;
                _isCompleted = value;
                if (value)
                {
                    OnLevelCompleted?.Invoke();
                    OnAnyLevelCompleted?.Invoke(this);
                }
            }
        }
        private bool _isCompleted = false; // Do not use, does not trigger events
        public event Action OnLevelCompleted;
        public static event Action<CaptchaLevelBase> OnAnyLevelCompleted;

        public abstract IEnumerator LevelRoutine();
    }
}