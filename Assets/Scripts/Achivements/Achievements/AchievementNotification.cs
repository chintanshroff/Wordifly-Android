using UnityEngine;
using System.Collections;

public delegate void HideNotifEvent ();
public class AchievementNotification : MonoBehaviour
{
	public static event HideNotifEvent OnHideNotif;

	public TextMesh txtDescription;
	public TextMesh txtRemainingCount;
	private Vector3 startPos;
	public Animator myAnim;

	void Awake ()
	{
		//myAnim = GetComponent<Animator> ();
	}

	void Start ()
	{
		startPos = transform.position;
	}

	public void UpdateText (string description, string count)
	{
		txtDescription.text = description;
		txtRemainingCount.text = count;
	}

	public void DisplayMission ()
	{
		myAnim.Play ("Notif_Idle");
		iTween.MoveTo (gameObject, iTween.Hash ("y", (GameManager_HMMS.Instance.screenSizeInWord.y - .5f), "time", 0.7f, "easeType", "spring"));
	}

	public void ShowNotification ()
	{
		Invoke ("HideNotification", 1f);
		myAnim.Play ("Notif_Scale");
	}


	void HideNotification ()
	{
		CancelInvoke ("HideNotification");
		myAnim.Play ("Notif_Yellow");
		iTween.MoveTo (gameObject, iTween.Hash ("x", 8f, "time", 0.5f, "easeType", "easeInOutBack", "oncomplete", "rePlaceGO"));
	}

	void rePlaceGO ()
	{
		this.gameObject.transform.position = startPos;
		if (OnHideNotif != null) {
			OnHideNotif ();
		}


	}



}
