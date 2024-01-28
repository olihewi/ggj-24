using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CaptchaGame;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Utilities;
using Object = System.Object;
using Random = UnityEngine.Random;

public class WordleLevel : CaptchaLevelBase
{
	public enum InputAcceptMode
	{
		EXACT,
		CONTAINS
	}

	public PlayableDirector appear, complete, skip, fail;
	public GenericButton submitButton;
	public GenericButton skipButton;

	public InputAcceptMode mode;
	public bool caseDependant;
	[SerializeField] private String _SelectedWord;

	[SerializeField] private GameObject[] _WordleRow1;
	[SerializeField] private GameObject[] _WordleRow2;
	[SerializeField] private GameObject[] _WordleRow3;
	[SerializeField] private GameObject[] _WordleRow4;
	[SerializeField] private GameObject[] _WordleRow5;
	[SerializeField] private GameObject[] _WordleRow6;

	private readonly GameObject[][] _WordleRows = new GameObject[6][];
	private string _TextInput = "";
	private Int32 _Attempts;
	private List<String> _SplitStrings;

	public bool ConditionMet
	{
		get
		{
			String str = _TextInput;
			if (!caseDependant)
			{
				str = str.ToLowerInvariant();

				_SelectedWord = _SelectedWord.ToLowerInvariant();
			}

			return mode switch
			{
				InputAcceptMode.EXACT => _SelectedWord == str,
				InputAcceptMode.CONTAINS => _SelectedWord.Any(x => str.Contains(x)),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}

	private void Awake()
	{
		ReadString();
	}

	private void Start()
	{
		_WordleRows[0] = _WordleRow1;
		_WordleRows[1] = _WordleRow2;
		_WordleRows[2] = _WordleRow3;
		_WordleRows[3] = _WordleRow4;
		_WordleRows[4] = _WordleRow5;
		_WordleRows[5] = _WordleRow6;
	}

	public override IEnumerator LevelRoutine()
	{
		appear.Play();

		while (!skipButton.isReleased && !ConditionMet)
		{
			while (!submitButton.isReleased && !skipButton.isReleased && !Input.GetKeyDown(KeyCode.Return))
			{
				String str = Input.inputString;
				if (_Attempts < 6)
				{
					if (str.Length > 0 && _TextInput.Length <= 5)
					{
						if (char.IsLetter(str[0]) && _TextInput.Length != 5)
						{
							_WordleRows[_Attempts][_TextInput.Length].GetComponent<TMP_InputField>().text += str;
							_TextInput += str;
						}
						else if (str[0] == 8 && _TextInput.Length > 0)
						{
							_TextInput = _TextInput[..^1];
							_WordleRows[_Attempts][_TextInput.Length].GetComponent<TMP_InputField>().text = "";
						}
					}
				}
				else
				{
					//TODO: ADD WORDLE FAIL SCREEN
					fail.Play();
					yield break;
				}

				yield return null;
			}

			if (skipButton.isReleased)
			{
				skip.Play();
				yield return new WaitForSeconds((float) skip.duration);
				yield break;
			}

			if (!_SplitStrings.Contains(_TextInput))
			{
				yield return new WaitForEndOfFrame();
				submitButton.isReleased = false;
				continue;
			}

			yield return LetterChecker();

			if (!ConditionMet && _TextInput.Length == 5)
			{
				_Attempts++;
				_TextInput = "";
				submitButton.isReleased = false;
				fail.Play();
				yield return new WaitForSeconds((float) fail.duration);
			}
		}

		complete.Play();
		yield return new WaitForSeconds((float) complete.duration);
	}

	private IEnumerator LetterChecker()
	{
		Char[] charArray = _SelectedWord.ToCharArray();
		for (int index = 0; index < charArray.Length; index++)
		{
			TMP_InputField inputField = _WordleRows[_Attempts][index].GetComponent<TMP_InputField>();
			Image image = _WordleRows[_Attempts][index].GetComponent<Image>();
			if (inputField.text == charArray[index].ToString())
			{
				image.color = new Color(0.3254901961f, 0.5529411765f, 0.3058823529f);
			}
			else if (_SelectedWord.Contains(inputField.text))
			{
				image.color = new Color(0.7098039216f, 0.6235294118f, 0.2313725490f);
			}
			else
			{
				image.color = new Color(0.2274509804f, 0.2274509804f, 0.2352941176f);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	private void ReadString()
	{
		string path = "";
#if UNITY_STANDALONE
		path = Application.persistentDataPath + "/WordleWords.txt";
#endif
#if UNITY_EDITOR
		path = "Assets/Resources/WordleWords.txt";
#endif
		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path);
		string words = reader.ReadToEnd();
		Debug.Log(words);
		reader.Close();

		_SplitStrings = words.Split("\n").ToList();

		_SplitStrings.Shuffle();

		DateTime utcNowDate = DateTime.UtcNow.Date;
		String time = utcNowDate.ToString(CultureInfo.CurrentCulture);
		Int32[] intArray = time.ToIntArray();
		int seed = intArray.Sum();

		Random.InitState(seed);
		Int32 range = Random.Range(0, _SplitStrings.Count);

		_SelectedWord = _SplitStrings[range];
	}
}