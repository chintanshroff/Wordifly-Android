using UnityEngine;
using System.Collections;

public delegate void AchievementComplete (Objectives obj);
public abstract class Objectives : MonoBehaviour
{
	public abstract bool IsAchieved ();
	public abstract bool IsComplete ();
	public abstract void Complete ();
	public abstract void Activate ();
	public string key = "unique_key(text)";
	public string id = null;// should be unique.
	public string message = "default description!.";
	public string count = "";
	public int wordCount = 0;
	public int totalWordCount = 1;
	

	public event AchievementComplete AchievementCompleted;
	protected void GoalCompleted (Objectives obj)
	{
		if (AchievementCompleted != null) {
			AchievementCompleted (obj);
		}
	}
		
}
