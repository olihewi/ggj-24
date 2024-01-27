using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CaptchaGame
{
    public class GenericButton : MonoBehaviour
    {
        public Animator animator;
        private static readonly int anim_IsEnabled = Animator.StringToHash("Is Enabled");
        private static readonly int anim_IsHovered = Animator.StringToHash("Is Enabled");
        private static readonly int anim_IsHeld = Animator.StringToHash("Is Enabled");
        public event Action OnPressed, OnReleased;

        public bool isPressed
        {
            get => _isPressed;
            set
            {
                if (_isPressed == value) return;
                _isPressed = value;
                if (value)
                {
                    OnPressed?.Invoke();
                    if (animator != null) animator.SetBool(anim_IsHeld, true);
                }
            }
        }
        private bool _isPressed;

        public bool isReleased
        {
            get => _isReleased;
            set
            {
                if (_isReleased == value) return;
                _isReleased = value;
                if (value)
                {
                    OnReleased?.Invoke();
                    if (animator != null) animator.SetBool(anim_IsHeld, false);
                }
            }
        }
        private bool _isReleased;

        protected void OnMouseDown()
        {
            isPressed = true;
            OnPressed?.Invoke();
        }

        private void OnMouseUp()
        {
            isReleased = true;
            OnReleased?.Invoke();
        }
    }
}