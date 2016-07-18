using UnityEngine;
using System.Collections.Generic;
public delegate void PlayerDataChangedEvent ();
public sealed class PlayerData
{
	private static PlayerData instance = null;
	private static readonly object padlock = new object ();

	private int _score = 0;
	private int _highScore = 0;

		

	// Event
	public static event PlayerDataChangedEvent SoundSettingChanged;
	PlayerData ()
	{

	}

	public static PlayerData Instance {
		get {
			lock (padlock) {
				if (instance == null) {
					instance = new PlayerData ();
				}
				return instance;
			}
		}
	}

	/// <summary>
	/// Cleans the data.
	/// </summary>
	/// <param name="clean">If set to <c>true</c> clean.</param>
	public void CleanData (bool clean)
	{
		if (clean) {
			PlayerPrefs.DeleteAll ();
		}
	}
	/**
     * read Player Data 
     */
	public void ReadData ()
	{
		bool hasSavedData = false;
		if (PlayerPrefs.HasKey ("score")) {
			_score = PlayerPrefs.GetInt ("score");
			hasSavedData = true;
		}
		if (PlayerPrefs.HasKey ("highScore")) {
			_highScore = PlayerPrefs.GetInt ("highScore");
			hasSavedData = true;
		}

		if (!hasSavedData)
			SaveData ();
	}

	public int CheckAndSaveKey (string p_UniqueKey, int p_value)
	{
		if (PlayerPrefs.HasKey (p_UniqueKey)) {
			p_value = PlayerPrefs.GetInt (p_UniqueKey, p_value);
		} else {
			PlayerPrefs.SetInt (p_UniqueKey, p_value);
		}
		PlayerPrefs.Save ();
		return p_value;
	}
	public void SaveKey (string p_UniqueKey, int p_value)
	{
		PlayerPrefs.SetInt (p_UniqueKey, p_value);
	}

	/**
     * Instantly saves all the player data
     */
	public void SaveData ()
	{
		PlayerPrefs.SetInt ("score", _score);
		PlayerPrefs.SetInt ("highScore", _highScore);
		PlayerPrefs.Save ();
	}
	public int Score {
		get {
			return _score;
		}
		set {
			_score = value;
		}
	}

	public int HighScore {
		get {
			return _highScore;
		}
		set {
			_highScore = value;
			PlayerPrefs.SetInt ("highScore", _highScore);
			PlayerPrefs.Save ();
		}
	}


}