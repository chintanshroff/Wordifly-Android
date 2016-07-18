using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

	public Transform TutorialScreen;
	public Animator LoadAnim;
	// Use this for initialization
	public GameObject PlayBtn;
	private AsyncOperation async;




	void Start ()
	{
		//Camera.main.aspect = 10f / 16f;
		//FadeIn (this.gameObject);
		Invoke ("FadeOut", 4);
	}



	public void FadeOut ()
	{
		iTween.ValueTo (this.gameObject, iTween.Hash ("from", 1.0f, "to", 0.0f, "time", 1f, "easetype", "linear", "onupdate", "setAlpha", "oncomplete", "Complete"));
	}

	private void Complete ()
	{
		TutorialScreen.gameObject.SetActive (true);
		async = Application.LoadLevelAsync ("MainGame_HMMS");
		//async.allowSceneActivation = false;
		StartCoroutine (StartLoading ());
	}

	IEnumerator StartLoading ()
	{
		while (async.progress < 0.9f)
		{
			Console.Log (async.progress);
			LoadAnim.Play ("Loading");
			yield return new WaitForSeconds (0.1f);
		}
		//LoadAnim.gameObject.SetActive (false);
		//async.allowSceneActivation = true;
	}

	public void Play ()
	{
		
	}


	public void FadeIn ()
	{
		iTween.ValueTo (gameObject, iTween.Hash ("from", 0f, "to", 1f, "time", 1f, "easetype", "linear", "onupdate", "setAlpha"));
	}

	public void setAlpha (float newAlpha)
	{
		foreach (Material mObj in GetComponent<Renderer>().materials)
		{
			mObj.color = new Color (mObj.color.r, mObj.color.g, mObj.color.b, newAlpha);
		}
	}
}
