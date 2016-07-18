using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
	public Transform EmptyStartHolder ; 
	private Vector3 StartPos;





	void Start ()
	{
		StartPos = EmptyStartHolder.position;

	}

	void OnEnable ()
	{
		iTween.MoveTo (EmptyStartHolder.gameObject, iTween.Hash ("position", new Vector3 (0, GameManager_HMMS.Instance.screenSizeInWord.y - 2, -3), "easetype", "spring", "time", .7f));
	}

	public void Pause ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {
			if (!GameManager_HMMS.Instance.SoundOff) {
				GameManager_HMMS.Instance.source_Tap.Play ();
			}
			GameManager_HMMS.Instance.CurrentState = GameState.Paused;
			GameManager_HMMS.Instance.Pause_Panel.gameObject.SetActive (true);
		}
	}
	
	void OnDisable ()
	{
		if (EmptyStartHolder != null) {
			EmptyStartHolder.position = StartPos;
			
		}
	}
}
