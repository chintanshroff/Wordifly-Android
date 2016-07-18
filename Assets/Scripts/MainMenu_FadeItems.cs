using UnityEngine;
using System.Collections;

public class MainMenu_FadeItems : MonoBehaviour
{

	void Awake ()
	{
		MainMenuHandler.OnPlay += OnGameStart;
		MainMenuHandler.OnMainMenu += OnQuit;
	
	}
	
	// Update is called once per frame
	void OnDestroy ()
	{
		MainMenuHandler.OnPlay -= OnGameStart;
		MainMenuHandler.OnMainMenu -= OnQuit;
	}

	void OnGameStart ()
	{
		FadeOut ();
	}

	public void FadeOut ()
	{
		iTween.ValueTo (gameObject, iTween.Hash ("from", 1.0f, "to", 0.0f, "time", 1f, "easetype", "linear", "onupdate", "setAlpha", "oncomplete", "Complete"));
	}

	public void setAlpha (float newAlpha)
	{
		foreach (Material mObj in GetComponent<Renderer>().materials) {
			mObj.color = new Color (mObj.color.r, mObj.color.g, mObj.color.b, newAlpha);
		}
	}

	void OnQuit ()
	{
		FadeIn ();
	}

	public void FadeIn ()
	{
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f, "easetype", "linear", "onupdate", "setAlpha"));
	}
}
