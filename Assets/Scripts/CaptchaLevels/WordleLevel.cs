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

	public TMP_InputField textInput;

	public InputAcceptMode mode;
	public bool caseDependant;
	[SerializeField] private String _SelectedWord;

	[SerializeField] private GameObject[] _WordleRow1;
	[SerializeField] private GameObject[] _WordleRow2;
	[SerializeField] private GameObject[] _WordleRow3;
	[SerializeField] private GameObject[] _WordleRow4;
	[SerializeField] private GameObject[] _WordleRow5;
	[SerializeField] private GameObject[] _WordleRow6;

	private readonly GameObject[][] WordleRows = new GameObject[6][];
	private string _TypedWord;
	private Int32 _Attempts;

	public bool ConditionMet
	{
		get
		{
			String str = textInput.text;
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

	private void Start()
	{
		ReadString();
		WordleRows[0] = _WordleRow1;
		WordleRows[1] = _WordleRow2;
		WordleRows[2] = _WordleRow3;
		WordleRows[3] = _WordleRow4;
		WordleRows[4] = _WordleRow5;
		WordleRows[5] = _WordleRow6;

		foreach (GameObject[] wordleRow in WordleRows)
		{
			foreach (GameObject square in wordleRow)
			{
				TMP_InputField inputField = square.GetComponentInChildren<TMP_InputField>();
				inputField.text = "";
			}
		}
	}

	public override IEnumerator LevelRoutine()
	{
		appear.Play();

		while (!skipButton.isReleased && !ConditionMet)
		{
			while (!submitButton.isReleased && !skipButton.isReleased && !Input.GetKeyDown(KeyCode.Return))
			{
				String str = Input.inputString;
				if (str.Length > 0 && _TypedWord.Length <= 5)
				{
					if (char.IsLetter(str[0]))
					{
						WordleRows[_Attempts][_TypedWord.Length].GetComponentInChildren<TMP_InputField>().text += str;
						_TypedWord += str;
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
				yield return new WaitForSeconds((float) skip.duration);
				yield break;
			}

			if (!ConditionMet && _TypedWord.Length == 5)
			{
				_Attempts++;
				submitButton.isReleased = false;
				fail.Play();
				yield return new WaitForSeconds((float) fail.duration);
			}
		}

		complete.Play();
		yield return new WaitForSeconds((float) complete.duration);
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

		List<String> splitStrings = words.Split("\n").ToList();

		splitStrings.Shuffle();

		DateTime utcNowDate = DateTime.UtcNow.Date;
		String time = utcNowDate.ToString(CultureInfo.CurrentCulture);
		Int32[] intArray = time.ToIntArray();
		int seed = intArray.Sum();

		Random.InitState(seed);
		Int32 range = Random.Range(0, splitStrings.Count);

		_SelectedWord = splitStrings[range];
	}
}