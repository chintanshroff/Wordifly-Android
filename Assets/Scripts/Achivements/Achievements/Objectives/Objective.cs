using UnityEngine;
using System.Collections;


public delegate void AchievementProgress ();
public class Objective : Objectives
{
	public static event AchievementProgress OnAchievementProgress;

	private int achieved = 0;
	private bool _isActive = false;

	public enum Types
	{
		None,
		Collect,
		CompleteWord,
		StartsWith,
		TotalWords,
		EndsWith,
		MatchWord,
		Contains}
	;

	public Types primaryType = Types.None;
	public Types SecondaryType = Types.None;

	public bool resetOnGameOver = false;

	public int letterCount = 0;
	public int totalLetterCount = 1;

	public string startsWithLetter = "";
	public string endsWithLetter = "";
	public string containsLetter = "";

	//public int wordCount = 0;
	//public int totalWordCount = 1;

	public string WordToMatch;

	void OnEnable ()
	{
		if (resetOnGameOver) {
			GameManager_HMMS.GameOverShown += OnGameOverShown;
		}
	}

	void OnGameOverShown ()
	{
		if (resetOnGameOver) {
						
			if (primaryType == Types.Collect || SecondaryType == Types.Collect) {
				letterCount = 0;

			}
			if (primaryType == Types.CompleteWord || SecondaryType == Types.CompleteWord) {
				letterCount = 0;
				wordCount = 0;
			}
			if (primaryType == Types.StartsWith || SecondaryType == Types.StartsWith) {
				wordCount = 0;
			}
			if (primaryType == Types.TotalWords || SecondaryType == Types.TotalWords) {
				wordCount = 0;
			}
			if (primaryType == Types.EndsWith || SecondaryType == Types.EndsWith) {
				wordCount = 0;
			}
			if (primaryType == Types.MatchWord || SecondaryType == Types.MatchWord) {
				wordCount = 0;
			}
			if (primaryType == Types.Contains || SecondaryType == Types.Contains) {
				wordCount = 0;
			}
		}
	}

	void Awake ()
	{
		achieved = PlayerData.Instance.CheckAndSaveKey (key, achieved);
	}

	public override bool IsComplete ()
	{
		bool _unlocked = false;
		switch (primaryType) {
		case Types.Collect:
			_unlocked = letterCount >= totalLetterCount;
			break;
		case Types.CompleteWord:
			_unlocked = wordCount >= totalWordCount;
			break;
		case Types.StartsWith:
			_unlocked = wordCount >= totalWordCount;
			break;
		case Types.TotalWords:
			_unlocked = wordCount >= totalWordCount;
			break;
		case Types.EndsWith:
			_unlocked = wordCount >= totalWordCount;
			break;
		case Types.MatchWord:
			_unlocked = wordCount >= totalWordCount;
			break;
		case Types.Contains:
			_unlocked = wordCount >= totalWordCount;
			break;
		}

		return _unlocked;
	}

	public override void Complete ()
	{
		Console.Log ("achivement complete = " + message);
		achieved = 1;
		PlayerData.Instance.SaveKey (key, achieved);
		GoalCompleted (this);
	}

	public override bool IsAchieved ()
	{
		if (achieved == 0) {
			return false;
		}
		return true;
	}

	void OnWordChecked ()
	{
		letterCount += 1;
		wordCount += 1;
		OnAchievementProgress ();
	}

	void OnValidWord (string word)
	{
		if (totalLetterCount == word.Length) {
			wordCount += 1;
			OnAchievementProgress ();
		}
	}

	void OnStartsWithWord (string word)
	{
		Console.Log ("word.StartsWith (startsWithLetter) = " + word.StartsWith (startsWithLetter));
		if (startsWithLetter != "" && (word.StartsWith (startsWithLetter))) {
			wordCount += 1;
			OnAchievementProgress ();
			
		}
	}

	void OnEndsWithWord (string word)
	{
		Console.Log ("word.EndsWith (endsWithLetter) = " + word.EndsWith (endsWithLetter));
		if (endsWithLetter != "" && (word.EndsWith (endsWithLetter))) {
			wordCount += 1;
			OnAchievementProgress ();
			
		}
	}


	void OnTotalWords (string word)
	{
		wordCount += 1;
		OnAchievementProgress ();
		
	}

	void OnMatchWord (string word)
	{
		if (WordToMatch != "" && (word.Equals (WordToMatch))) {
			wordCount += 1;
			OnAchievementProgress ();
			
		}
	}

	void OnContainsWord (string word)
	{
		if (containsLetter != "" && (word.Contains (containsLetter))) {
			wordCount += 1;
			OnAchievementProgress ();
			
		}
	}

	public override void Activate ()
	{
		Console.Log ("activating achievement = " + message);
		if (primaryType == Types.Collect || SecondaryType == Types.Collect) {
			Star.OnLetterPicked += OnWordChecked;
		}
		if (primaryType == Types.CompleteWord || SecondaryType == Types.CompleteWord) {
			wordCheck.ValidWord += OnValidWord;
		}
		if (primaryType == Types.StartsWith || SecondaryType == Types.StartsWith) {
			wordCheck.ValidWord += OnStartsWithWord;
		}
		if (primaryType == Types.TotalWords || SecondaryType == Types.TotalWords) {
			wordCheck.ValidWord += OnTotalWords;
		}
		if (primaryType == Types.EndsWith || SecondaryType == Types.EndsWith) {
			wordCheck.ValidWord += OnEndsWithWord;
		}
		if (primaryType == Types.MatchWord || SecondaryType == Types.MatchWord) {
			wordCheck.ValidWord += OnMatchWord;
		}
		if (primaryType == Types.Contains || SecondaryType == Types.Contains) {
			wordCheck.ValidWord += OnContainsWord;
		}
		_isActive = true;
				
	}

	public bool IsActive {
		get {
			return _isActive;
		}
	}

	public void OnDisable ()
	{
				
		if (primaryType == Types.Collect || SecondaryType == Types.Collect) {
			Star.OnLetterPicked -= OnWordChecked;
		}
		if (primaryType == Types.CompleteWord || SecondaryType == Types.CompleteWord) {
			wordCheck.ValidWord -= OnValidWord;
		}
		if (primaryType == Types.StartsWith || SecondaryType == Types.StartsWith) {
			wordCheck.ValidWord -= OnStartsWithWord;
		}
		if (primaryType == Types.TotalWords || SecondaryType == Types.TotalWords) {
			wordCheck.ValidWord -= OnTotalWords;
		}
		if (primaryType == Types.EndsWith || SecondaryType == Types.EndsWith) {
			wordCheck.ValidWord -= OnEndsWithWord;
		}
		if (primaryType == Types.MatchWord || SecondaryType == Types.MatchWord) {
			wordCheck.ValidWord -= OnMatchWord;
		}

		if (primaryType == Types.Contains || SecondaryType == Types.Contains) {
			wordCheck.ValidWord -= OnContainsWord;
		}


	}

	void OnApplicationQuit ()
	{
		Console.Log ("Quit");
		if (resetOnGameOver) {
			wordCount = 0;
			PlayerPrefs.SetInt ("CurrentCount", wordCount);
		} else {
			PlayerPrefs.SetInt ("CurrentCount", wordCount);

		}

	}

}
