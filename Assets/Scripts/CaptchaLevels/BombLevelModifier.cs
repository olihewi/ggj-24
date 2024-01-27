using System;
using TMPro;
using UnityEngine;

namespace CaptchaGame
{
    public class BombLevelModifier : MonoBehaviour
    {
        public TMP_Text text;
        public GenericButton skipButton;
        public float time = 10.0F;
        private float _currentTime = 0.0F;

        private void Start()
        {
            _currentTime = time;
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime < 0.0F)
            {
                skipButton.isReleased = true;
                _currentTime = 0.0F;
            }
            text.text = $"{_currentTime:F3}";
        }
    }
}