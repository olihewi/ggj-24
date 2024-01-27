using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using Object = System.Object;

namespace CaptchaGame
{
	public class SnapLevel : CaptchaLevelBase
	{
		public PlayableDirector appear, complete, skip, fail;
		public SnapElement[] snapElements;
		public GenericButton submitButton;
		public GenericButton skipButton;
		[SerializeField] private SnapElement _LastSelected;

		public bool ConditionMet
		{
			get
			{
				return snapElements.All(element => element.IsMatched);
			}
		}

		private void OnValidate()
		{
			snapElements = GetComponentsInChildren<SnapElement>();
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

				if (!ConditionMet)
				{
					submitButton.isReleased = false;
					fail.Play();
					yield return new WaitForSeconds((float) fail.duration);
				}
			}

			complete.Play();
			yield return new WaitForSeconds((float) complete.duration);
		}

		private void Start()
		{
			snapElements = GetComponentsInChildren<SnapElement>();

			foreach (SnapElement element in snapElements)
			{
				element.OnSelected += ElementOnOnSelected;
			}
		}

		private void ElementOnOnSelected(SnapElement snapElement)
		{
			if (_LastSelected.snapCombo == snapElement.snapCombo)
			{
				Debug.Log("SNAP!");
			}
			else if (_LastSelected == null)
			{
				_LastSelected = snapElement;
			}
		}
	}
}