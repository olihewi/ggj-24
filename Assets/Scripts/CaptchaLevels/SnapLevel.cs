using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using Utilities;

namespace CaptchaGame
{
	public class SnapLevel : CaptchaLevelBase
	{
		public PlayableDirector appear, complete, skip, fail;
		public List<SnapElement> snapElements = new();
		public GenericButton submitButton;
		public GenericButton skipButton;
		private SnapElement _LastSelected;

		public bool ConditionMet
		{
			get { return snapElements.All(element => element.IsMatched); }
		}

		private void Start()
		{
			snapElements = GetComponentsInChildren<SnapElement>().ToList();
			List<Vector3> position = new List<Vector3>(snapElements.Count);
			foreach (SnapElement element in snapElements)
			{
				position.Add(element.transform.position);
				element.OnSelected += ElementOnOnSelected;
			}

			position.Shuffle();
			for (int index = 0; index < snapElements.Count; index++)
			{
				snapElements[index].transform.position = position[index];
			}
		}

		private void OnValidate()
		{
			snapElements = GetComponentsInChildren<SnapElement>().ToList();
		}

		public override IEnumerator LevelRoutine()
		{
			appear.Play();

			while (!skipButton.isReleased && !ConditionMet)
			{
				yield return new WaitUntil(() => submitButton.isReleased || skipButton.isReleased);
				if (skipButton.isReleased)
				{
					skip.Play();
					yield return new WaitForSeconds((float) skip.duration);
					yield break;
				}

				if (ConditionMet) continue;

				submitButton.isReleased = false;
				fail.Play();
				yield return new WaitForSeconds((float) fail.duration);
			}

			complete.Play();
			yield return new WaitForSeconds((float) complete.duration);
		}

		private void ElementOnOnSelected(SnapElement snapElement)
		{
			if (_LastSelected == null)
			{
				_LastSelected = snapElement;
			}
			else if (_LastSelected.snapCombo == snapElement.snapCombo)
			{
				Debug.Log("SNAP! :) ");

				snapElement.IsMatched = true;
				_LastSelected.IsMatched = true;

				_LastSelected = null;
			}
			else
			{
				Debug.Log("No Snap :( ");
				StartCoroutine(NoSnap(snapElement));
			}
		}

		private IEnumerator NoSnap(SnapElement snapElement)
		{
			yield return new WaitForSeconds(1f);
			_LastSelected.IsSelected = false;
			snapElement.IsSelected = false;
			_LastSelected = null;
		}
	}
}