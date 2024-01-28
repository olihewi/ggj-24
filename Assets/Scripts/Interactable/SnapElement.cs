using System;
using UnityEngine;

namespace CaptchaGame
{
	public class SnapElement : MonoBehaviour
	{
		public enum SnapCombo
		{
			SNAP_01,
			SNAP_02,
			SNAP_03,
			SNAP_04,
			SNAP_05,
			SNAP_06,
			SNAP_07,
			SNAP_08
		}

		private static readonly int _AnimIsSelected = Animator.StringToHash("Is Selected");
		public Animator animator;
		public SnapCombo snapCombo;
		private bool _IsSelected; // Do not use, does not trigger events

		public bool IsSelected
		{
			get => _IsSelected;
			set
			{
				if (_IsSelected == value) return;
				_IsSelected = value;
				if (value)
				{
					OnSelected?.Invoke(this);
				}

				animator.SetBool(_AnimIsSelected, value);
			}
		}

		public Boolean IsMatched { get; set; }

		private void Start()
		{
			animator = GetComponent<Animator>();
		}

		private void OnMouseDown()
		{
			IsSelected = true;
		}

		public event Action<SnapElement> OnSelected;
	}
}