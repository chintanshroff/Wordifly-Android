using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.GameCenter;

public delegate void AchievementCompleteEvent ();
public class AchievementHandler : MonoBehaviour
{

	public static event AchievementCompleteEvent OnAchievementComplete;

	public List<Objectives> objective;
	private Objectives _currentObjective;
	public AchievementNotification achievementNotification;
	public TextMesh MissionDescriptionForMainMenu;
	public Text MissionDescriptionForPauseMenu;
	private bool firstTime;


	

	void Awake ()
	{
		AchievementNotification.OnHideNotif += ShowNextNotif;
		Objective.OnAchievementProgress += UpdateProgressText;

	}

	void Start ()
	{
		for (int i = objective.Count - 1; i >= 0; i--) {
						
			if (objective [i].IsAchieved ()) {
				Destroy (objective [i].gameObject);
				objective.RemoveAt (i);
			}
		}
		if (objective.Count > 0) {
			_currentObjective = objective [0];
			_currentObjective.Activate ();
			_currentObjective.wordCount = PlayerPrefs.GetInt ("CurrentCount", 0);
			achievementNotification.UpdateText (_currentObjective.message, _currentObjective.wordCount + "/" + _currentObjective.totalWordCount); // i have added this
			
		} else {
			_currentObjective = null;
			GameManager_HMMS.Instance.CompleteText.gameObject.SetActive (true);
			Console.Log ("All Objectives complete");
		}
		if (achievementNotification == null) {
			Console.LogError ("AchievementNotification is required to display achievements.");
		}
	}

	void Update ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {

			if (GameManager_HMMS.Instance.firstTimeAchievementLoad) {
				
				achievementNotification.UpdateText (_currentObjective.message, _currentObjective.wordCount + "/" + _currentObjective.totalWordCount); // i have added this
				GameManager_HMMS.Instance.firstTimeAchievementLoad = false;
			}

			if (_currentObjective != null) {
				if (!firstTime) {
					firstTime = true;
					achievementNotification.DisplayMission ();
				}

				if (_currentObjective.IsComplete ()) {
					_currentObjective.Complete ();
					
					if (achievementNotification != null) {
						achievementNotification.ShowNotification ();
						if (!GameManager_HMMS.Instance.SoundOff) {
							GameManager_HMMS.Instance.source_AchivementComplete.Play ();
						}
						//reward
						if (OnAchievementComplete != null) {
							OnAchievementComplete ();
						}
						if (_currentObjective.id != null) {
							Console.Log ("_currentObjective.id = " + _currentObjective.id);
							#if UNITY_IPHONE
							GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);
							#endif

							Social.ReportProgress (_currentObjective.id, 100.0f, (bool success) => {
								// handle success or failure
							});

						}

					}
					Destroy (_currentObjective.gameObject);
					objective.RemoveAt (0);
					if (objective.Count > 0) {
						_currentObjective = objective [0];
					} else {
						_currentObjective = null;
					}
				}
			}
		}
	}

	void UpdateProgressText ()
	{
		achievementNotification.UpdateText (_currentObjective.message, _currentObjective.wordCount + "/" + _currentObjective.totalWordCount); // i have added this
		PlayerPrefs.SetInt ("CurrentCount", _currentObjective.wordCount);
		achievementNotification.myAnim.Play ("Notif_Scale");
		StartCoroutine ("StopNofitScaleAnim");
	}

	IEnumerator StopNofitScaleAnim ()
	{
		yield return new WaitForSeconds (0.5f);
		achievementNotification.myAnim.Play ("Notif_Idle");
	}

	void ShowNextNotif ()
	{
		if (_currentObjective != null) {
			_currentObjective.Activate ();
			achievementNotification.UpdateText (_currentObjective.message, _currentObjective.wordCount + "/" + _currentObjective.totalWordCount); // i have added this
			achievementNotification.DisplayMission ();
		} else {
			GameManager_HMMS.Instance.CompleteText.gameObject.SetActive (true);
			Console.Log ("All Objectives complete");

		}
	}

	void OnDestroy ()
	{
		AchievementNotification.OnHideNotif -= ShowNextNotif;
		Objective.OnAchievementProgress -= UpdateProgressText;
	}

}