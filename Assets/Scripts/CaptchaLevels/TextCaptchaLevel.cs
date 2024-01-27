using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace CaptchaGame
{
    public class TextCaptchaLevel : CaptchaLevelBase
    {
        public PlayableDirector appear, complete, skip, fail;
        public GenericButton submitButton;
        public GenericButton skipButton;
        
        public TMP_InputField textInput;
        public enum InputAcceptMode
        {
            Exact,
            Contains
        }

        public InputAcceptMode mode;
        public bool caseDependant = false;
        public string[] acceptedInputs;
        public override IEnumerator LevelRoutine()
        {
            appear.Play();
            
            while (!skipButton.isReleased && !ConditionMet)
            {
                while (!submitButton.isReleased && !skipButton.isReleased && !Input.GetKeyDown(KeyCode.Return))
                {
                    var str = Input.inputString;
                    if (str.Length > 0)
                    {
                        if (char.IsLetterOrDigit(str[0]) || str[0] == ' ' || char.IsSymbol(str[0]))
                        {
                            textInput.text += str;
                        }
                        else if (str[0] == 8 && textInput.text.Length > 0)
                        {
                            textInput.text = textInput.text[..^1];
                        }
                    }
                    yield return null;
                }
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
                var str = textInput.text;
                if (!caseDependant)
                {
                    str = str.ToLowerInvariant();
                    for (int i = 0; i < acceptedInputs.Length; i++)
                    {
                        acceptedInputs[i] = acceptedInputs[i].ToLowerInvariant();
                    }
                }

                return mode switch
                {
                    InputAcceptMode.Exact => acceptedInputs.Any(x => x == str),
                    InputAcceptMode.Contains => acceptedInputs.Any(x => str.Contains(x)),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}