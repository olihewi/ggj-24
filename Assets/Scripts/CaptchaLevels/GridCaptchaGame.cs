using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace CaptchaGame
{
    public class GridCaptchaGame : CaptchaLevelBase
    {
        public PlayableDirector appear, complete, skip, fail;
        public GridCaptchaElement[] gridCaptchaElements;
        public GenericButton submitButton;
        public GenericButton skipButton;
        public override IEnumerator LevelRoutine()
        {
            appear.Play();

            while (!skipButton.isReleased && !ConditionMet)
            {
                yield return new WaitUntil(() => submitButton.isReleased || skipButton.isReleased);
                if (skipButton.isReleased)
                {
                    skip.Play();
                    yield return new WaitForSeconds((float)skip.duration);
                    yield break;
                }

                if (!ConditionMet)
                {
                    submitButton.isReleased = false;
                    fail.Play();
                    yield return new WaitForSeconds((float)fail.duration);
                }
            }
            
            complete.Play();
            yield return new WaitForSeconds((float)complete.duration);
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