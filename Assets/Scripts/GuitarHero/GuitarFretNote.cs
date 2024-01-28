using System.Collections;
using System.Linq;
using UnityEngine;

namespace GuitarHero
{
    public class GuitarFretNote : MonoBehaviour
    {
        public Animator animator;
        private static readonly int anim_Held = Animator.StringToHash("Held");
        private static readonly int anim_Strummed = Animator.StringToHash("Strummed");

        public KeyCode[] keyCodes;
        public string[] buttonNames;
        
        public bool IsHeld
        {
            get => _isHeld;
            private set
            {
                if (_isHeld == value) return;
                _isHeld = value;
                animator.SetBool(anim_Held, value);
            }
        }
        
        private bool _isHeld = false;
        private void Update()
        {
            IsHeld = keyCodes.Any(Input.GetKey) || buttonNames.Any(Input.GetButton);
        }

        public void Strum() => StartCoroutine(StrumRoutine());

        private IEnumerator StrumRoutine()
        {
            animator.SetTrigger(anim_Strummed);
            yield return null;
            animator.ResetTrigger(anim_Strummed);
        }
    }
}