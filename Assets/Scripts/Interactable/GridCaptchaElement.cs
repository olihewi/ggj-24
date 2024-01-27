using System;
using UnityEngine;

namespace CaptchaGame
{
    public class GridCaptchaElement : MonoBehaviour
    {
        public enum GridContains
        {
            Yes,
            Maybe,
            No
        }
        public GridContains gridContains = GridContains.No;
        public bool IsCorrect => gridContains switch
        {
            GridContains.Yes => IsSelected,
            GridContains.Maybe => true,
            GridContains.No => !IsSelected,
            _ => throw new ArgumentOutOfRangeException()
        };
        public Animator animator;
        private static readonly int anim_IsSelected = Animator.StringToHash("Is Selected");
        private static readonly int anim_IsEnabled = Animator.StringToHash("Is Enabled");

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                (value ? OnEnabled : OnDisabled)?.Invoke();
                animator.SetBool(anim_IsEnabled, value);
            }
        }
        private bool _isEnabled = false;
        public event Action OnEnabled, OnDisabled;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                (value ? OnSelected : OnDeselected)?.Invoke();
                animator.SetBool(anim_IsSelected, value);
            }
        }
        private bool _isSelected = false; // Do not use, does not trigger events
        public event Action OnSelected, OnDeselected;

        private void OnMouseDown()
        {
            IsSelected = !IsSelected;
        }
    }
}