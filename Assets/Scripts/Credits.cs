using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
	private Button BackBtn;
	public MainMenuHandler myMainMenu;
	private Animator myAnim;
	private Vector3 myStartPos;


	void Awake ()
	{
		BackBtn = GetComponentInChildren<Button> ();
		myAnim = GetComponent<Animator> ();
	}
	
	void OnEnable ()
	{
		myStartPos = transform.position;
		OpenCreditPopUp ();
		Console.Log (GameManager_HMMS.Instance.screenSizeInWord.y);
	}

	IEnumerator CloseCreditPopUp ()
	{
		BackBtn.interactable = false;
		iTween.MoveTo (this.gameObject, iTween.Hash ("y", myStartPos.y, "easetype", "linear", "time", .5f));
		//myAnim.Play ("Credits_GoBackUp");
		StartCoroutine (myMainMenu.ActivateButtons ());
		yield return new WaitForSeconds (1f);
		this.gameObject.SetActive (false);
	}
	
	void OpenCreditPopUp ()
	{
		GetComponent<Image> ().color = GameManager_HMMS.Instance.startColor;
		BackBtn.gameObject.SetActive (false);
		iTween.MoveTo (this.gameObject, iTween.Hash ("y", 0, "easetype", "linear", "time", .5f));
		//myAnim.Play ("Credits_Fall");
		StartCoroutine ("ActivateBackButton");

	}

	IEnumerator ActivateBackButton ()
	{
		yield return new WaitForSeconds (0.5f);
		BackBtn.gameObject.SetActive (true);
		BackBtn.interactable = true;
	}

	public void Back ()
	{
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Tap.Play ();
		}
		StartCoroutine ("CloseCreditPopUp");
	}


}
