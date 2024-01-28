using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CaptchaGame
{
	public class WackAElement : MonoBehaviour
	{

		private static readonly int _AnimIsSelected = Animator.StringToHash("Is Selected");
		public Animator animator;
		[SerializeField] private MeshRenderer _MeshRenderer;
		private bool _IsSelected; // Do not use, does not trigger events
		private float _Timer;
		[SerializeField] private Material _MoleMaterial;

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

		public Boolean IsWack { get; set; }

		private void Start()
		{
			_MeshRenderer = GetComponent<MeshRenderer>();
			animator = GetComponent<Animator>();
			_Timer = Random.Range(5, 20);
		}

		private void Update()
		{
			if (_Timer <= 0.0f)
			{
				_Timer = Random.Range(5, 20);
				StartCoroutine(Mole());
			}
			else
			{
				_Timer -= Time.deltaTime;
			}
		}

		private IEnumerator Mole()
		{
			var material = _MeshRenderer.material;
			_MeshRenderer.material = _MoleMaterial;
			IsWack = true;
			yield return new WaitForSeconds(1f);
			IsWack = false;
			_MeshRenderer.material = material;
		}

		private void OnMouseDown()
		{
			IsSelected = true;
		}

		public event Action<WackAElement> OnSelected;

		public IEnumerator Reset()
		{
			yield return new WaitForSeconds(0.5f);
			IsSelected = false;
		}
	}
}