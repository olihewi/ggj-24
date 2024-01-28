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
		[SerializeField] private Material _MoleMaterial;
		[SerializeField] private AudioClip[] _AudioClip;
		[SerializeField] private AudioClip _MoleSound;

		private AudioSource _AudioSource;
		private bool _IsSelected; // Do not use, does not trigger events
		private float _Timer;

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

		public IEnumerator Reset()
		{
			yield return new WaitForSeconds(0.5f);
			IsSelected = false;
		}

		private void Start()
		{
			_MeshRenderer = GetComponent<MeshRenderer>();
			animator = GetComponent<Animator>();
			_AudioSource = GetComponent<AudioSource>();
			_Timer = Random.Range(5, 20);
		}

		private void Update()
		{
			if (_Timer <= 0.0f)
			{
				_Timer = Random.Range(2, 10);
				StartCoroutine(Mole());
			}
			else
			{
				_Timer -= Time.deltaTime;
			}
		}

		private void OnMouseDown()
		{
			IsSelected = true;
		}

		private IEnumerator Mole()
		{
			var material = _MeshRenderer.material;
			_AudioSource.clip = _MoleSound;
			_MeshRenderer.material = _MoleMaterial;
			_AudioSource.Play();
			IsWack = true;
			yield return new WaitForSeconds(1f);
			IsWack = false;
			_MeshRenderer.material = material;
		}

		public event Action<WackAElement> OnSelected;

		public void PlayAudio()
		{
			int range = Random.Range(0, 2);
			_AudioSource.clip = _AudioClip[range];
			_AudioSource.Play();
		}
	}
}