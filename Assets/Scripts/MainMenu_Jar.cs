using UnityEngine;
using System.Collections;

public class MainMenu_Jar : MonoBehaviour
{

	void Awake ()
	{
		MainMenuHandler.OnPlay += OnGameStart;
		MainMenuHandler.OnMainMenu += OnQuit; 
		PauseHandler.OnResume += OnResumeBtnClicked;
	}
	
	// Update is called once per frame
	void OnDestroy ()
	{
		MainMenuHandler.OnPlay -= OnGameStart;
		MainMenuHandler.OnMainMenu -= OnQuit;
		PauseHandler.OnResume -= OnResumeBtnClicked;
	}
	
	void OnGameStart ()
	{
		Invoke ("MoveBelow", 1f);
	}
	
	void MoveBelow ()
	{
		iTween.MoveTo (this.gameObject, iTween.Hash ("y", -7.43f, "time", 1f, "easetype", "linear", "onComplete", "Destroy"));
	}

	void OnQuit ()
	{
		this.gameObject.transform.position = new Vector3 (0.0f, -1.75f, -3.75f);
		this.gameObject.SetActive (true);
	}

	void Destroy ()
	{
		this.gameObject.SetActive (false);
	}

	void OnResumeBtnClicked ()
	{
		this.gameObject.SetActive (false);
	}
}
