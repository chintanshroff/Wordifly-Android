using UnityEngine;
using System.Collections;

public delegate void TutorialEvent ();
public class Bee_HMMS : MonoBehaviour
{
	public static event TutorialEvent StartTurorial;
	private CircleCollider2D myCollider;
	private Animator myAnimator;



	void Awake ()
	{
		MainMenuHandler.OnPlay += OnGameStart;
		myCollider = GetComponent<CircleCollider2D> ();
		myAnimator = GetComponent<Animator> ();
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Flower") { 
			if (GameManager_HMMS.Instance.PickedLetterList.Count <= 7) {
				other.gameObject.GetComponent<Star> ().OnPlayerCollideWithStar ();
				if (!GameManager_HMMS.Instance.notFirstTime) {
					GameManager_HMMS.Instance.starCollectedForTut++;
					if (GameManager_HMMS.Instance.starCollectedForTut < 3) {
						GameManager_HMMS.Instance.SpawnStar ();
					}
				}
			} else {

				//GameManager_HMMS.Instance.CapacityFull.SetActive (true);
			}
		}
	}



	void OnDestroy ()
	{
		MainMenuHandler.OnPlay -= OnGameStart;
	}

	void OnGameStart ()
	{
		StartCoroutine ("FlySwirl");
	}

	IEnumerator FlySwirl ()
	{
		myAnimator.Play ("Fly_Swirl");
		//length of this sound should be atleast 1 sec
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Play.Play ();
		}
		yield return new WaitForSeconds (1.5f);
		GameManager_HMMS.Instance.CurrentState = GameState.Playing;
		if (!GameManager_HMMS.Instance.StarPool.activeInHierarchy) {
			GameManager_HMMS.Instance.StarPool.SetActive (true);
			GameManager_HMMS.Instance.YelStarCollectPool.SetActive (true);
			GameManager_HMMS.Instance.TapFXPool.SetActive (true);
		}


		myAnimator.Play ("Fly");
		if (GameManager_HMMS.Instance.notFirstTime) {
			GameManager_HMMS.Instance.StartCoroutine ("SpawnStarTimer");
			Console.Log ("Not first time");
		} else if (!GameManager_HMMS.Instance.notFirstTime) {
			//start tutorial
			if (StartTurorial != null) {
				StartTurorial ();
			}
		}
	}

}
