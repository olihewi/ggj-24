using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace CaptchaGame
{
	public class WackAMoleLevel : CaptchaLevelBase
	{
		public PlayableDirector appear, complete, skip, fail;
		public List<WackAElement> wackAElements = new();
		public GenericButton skipButton;
		[SerializeField] private Int32 _Wacked;

		public bool ConditionMet => _Wacked == 10;

		private void Start()
		{
			wackAElements = GetComponentsInChildren<WackAElement>().ToList();
			foreach (WackAElement element in wackAElements)
			{
				element.OnSelected += ElementOnOnSelected;
			}
		}

		private void OnValidate()
		{
			wackAElements = GetComponentsInChildren<WackAElement>().ToList();
		}

		public override IEnumerator LevelRoutine()
		{
			appear.Play();

			while (!skipButton.isReleased && !ConditionMet)
			{
				if (skipButton.isReleased)
				{
					skip.Play();
					yield return new WaitForSeconds((float) skip.duration);
					yield break;
				}

				if (ConditionMet) continue;

				yield return null;
			}

			complete.Play();
			yield return new WaitForSeconds((float) complete.duration);
		}

		private void ElementOnOnSelected(WackAElement wackAElement)
		{
			if (wackAElement.IsWack)
			{
				_Wacked++;
			}

			wackAElement.PlayAudio();
			StartCoroutine(wackAElement.Reset());
		}
	}
}