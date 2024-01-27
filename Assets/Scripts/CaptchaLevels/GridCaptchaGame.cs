using System;
using System.Collections;
using UnityEngine;

namespace CaptchaGame
{
    public class GridCaptchaGame : CaptchaLevelBase
    {
        [Serializable]
        public struct GridCaptchaElementState
        {
            public GridCaptchaElement element;
            public bool targetValue;
        }
        public GridCaptchaElementState[] gridCaptchaElements;
        public GenericButton submitButton;
        public GenericButton skipButton;
        public override IEnumerator LevelRoutine()
        {
            foreach (var element in gridCaptchaElements)
            {
                element.element.IsEnabled = true;
            }

            while (!skipButton.isPressed && !ConditionMet)
            {
                yield return new WaitUntil(() => submitButton.isPressed || skipButton.isPressed);
            }
            yield return new WaitForSeconds(1.0F);
            
            foreach (var element in gridCaptchaElements)
            {
                element.element.IsEnabled = true;
            }
        }

        public bool ConditionMet
        {
            get
            {
                foreach (var element in gridCaptchaElements)
                {
                    if (element.element.IsSelected != element.targetValue) return false;
                }

                return true;
            }
        }
    }
}