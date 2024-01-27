using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CaptchaGame
{
    public class GridCaptchaGame : CaptchaLevelBase
    {
        public GridCaptchaElement[] gridCaptchaElements;
        public GenericButton submitButton;
        public GenericButton skipButton;
        public override IEnumerator LevelRoutine()
        {
            foreach (var element in gridCaptchaElements)
            {
                element.IsEnabled = true;
            }

            while (!skipButton.isPressed && !ConditionMet)
            {
                yield return new WaitUntil(() => submitButton.isPressed || skipButton.isPressed);
            }
            yield return new WaitForSeconds(1.0F);
            
            foreach (var element in gridCaptchaElements)
            {
                element.IsEnabled = true;
            }
        }

        public bool ConditionMet
        {
            get
            {
                foreach (var element in gridCaptchaElements)
                {
                    if (!element.IsCorrect) return false;
                }
                return true;
            }
        }

        private void OnValidate()
        {
            gridCaptchaElements = GetComponentsInChildren<GridCaptchaElement>();
        }
    }
}