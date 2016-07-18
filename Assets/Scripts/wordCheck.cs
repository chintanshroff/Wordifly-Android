using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public delegate void WordCheck (string word);
public delegate void TrailIncreaseEvent ();

public class wordCheck : MonoBehaviour
{

	public static event WordCheck ValidWord;
	public static event TrailIncreaseEvent IncreaseTrail;

	public string defaultLanguage = "en";
	private static SortedList<string, bool> wordList = null;
	public ParticleSystem WordTrueTap;
	public string WordToCheck = null;
	public Transform myBee;
	private CircleCollider2D myBeeCollider;
	private Transform myPooledTapFX;
	private TapFX TapFXObj;
	public Animator WordAnim;

	public Animator FlyingText;

	void Awake ()
	{
		Star.OnLetterPicked += OnPlayerPickLetterHandler;
		Star.OnBlankLetterPicked += OnBlankPickHandler;
		myBeeCollider = myBee.GetComponent<CircleCollider2D> ();
		MainMenuHandler.OnMainMenu += HandleOnMainMenu;

	}

	void HandleOnMainMenu ()
	{
		if (wordList == null)
		{
			setLanguage (defaultLanguage);
		}
	}


	void Update ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing)
		{

			if (GameManager_HMMS.Instance.PickedLetterList.Count >= 1)
			{
				if (GameManager_HMMS.Instance.PickedLetterList.Count < 8)
				{
					if (GameManager_HMMS.Instance.CapacityFull.activeInHierarchy)
					{
						GameManager_HMMS.Instance.CapacityFull.SetActive (false);
						if (!myBeeCollider.isActiveAndEnabled)
							myBeeCollider.enabled = true;
					}

				}
				else if (GameManager_HMMS.Instance.PickedLetterList.Count == 8)
				{
					if (!GameManager_HMMS.Instance.CapacityFull.activeInHierarchy)
					{
						GameManager_HMMS.Instance.CapacityFull.SetActive (true);
						myBeeCollider.enabled = false;
					}
				}
				if (Input.GetKeyDown (KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) && (Input.GetTouch (0).position.y < ((Screen.height / 3) * 2)))
				{
		
					//check if word is valid
					if ((isWord (WordToCheck)) && GameManager_HMMS.Instance.PickedLetterList.Count > 2)
					{
						if (!GameManager_HMMS.Instance.notFirstTime)
						{
							GameManager_HMMS.Instance.TapImage.SetActive (false);
							GameManager_HMMS.Instance.FireHelp.SetActive (true);
							StartCoroutine (GameManager_HMMS.Instance.SpawnStarTimer ());
							GameManager_HMMS.Instance.notFirstTime = true;
							PlayerPrefs.SetInt ("FirstTime", (GameManager_HMMS.Instance.notFirstTime ? 1 : 0));
						}


						GameManager_HMMS.Instance.tailScore = Mathf.Pow (GameManager_HMMS.Instance.PickedLetterList.Count, GameManager_HMMS.Instance.TailScorePower);
						if (IncreaseTrail != null)
						{
							IncreaseTrail ();
						}
						//for achievement system
						if (ValidWord != null)
						{
							ValidWord (WordToCheck);
						}
						if (GameManager_HMMS.Instance.CapacityFull.activeInHierarchy)
						{
							GameManager_HMMS.Instance.CapacityFull.SetActive (false);
						}
						if (!myBeeCollider.isActiveAndEnabled)
							myBeeCollider.enabled = true;
						SpawnTapFX ();
						Console.Log ("TapFX");
						StartCoroutine ("AddScore");
						GameManager_HMMS.Instance.IncreaseStarSpeed ();
						for (int i = 0; i < GameManager_HMMS.Instance.PickedLetterList.Count; i++)
						{
							GameManager_HMMS.Instance.PickedLetterList [i].picked = false;
							GameManager_HMMS.Instance.PickedLetterList [i].RemoveStar ();
						}
						GameManager_HMMS.Instance.PickedLetterList.RemoveRange (0, GameManager_HMMS.Instance.PickedLetterList.Count);
						WordToCheck = WordToCheck.Remove (0);
					}
				//else if word is invalid, remove a letter
				else
					{
						if (GameManager_HMMS.Instance.notFirstTime)
						{
							if (!GameManager_HMMS.Instance.SoundOff)
							{
								GameManager_HMMS.Instance.source_Drop.Play ();
							}
							//GameManager_HMMS.Instance.PickedLetterList [GameManager_HMMS.Instance.PickedLetterList.Count - 1].picked = false;
							//GameManager_HMMS.Instance.PickedLetterList [GameManager_HMMS.Instance.PickedLetterList.Count - 1].SpawnPickUp_PS ();
							GameManager_HMMS.Instance.PickedLetterList [GameManager_HMMS.Instance.PickedLetterList.Count - 1].RemoveStar ();
							GameManager_HMMS.Instance.PickedLetterList.RemoveAt (GameManager_HMMS.Instance.PickedLetterList.Count - 1);
							WordToCheck = WordToCheck.Remove (GameManager_HMMS.Instance.PickedLetterList.Count);

						}
					}
				}
			}
		}
	}



	//functions to check word validity
	public static bool isWord (string word)
	{
		return wordList.ContainsKey (word.ToLower ());
	}

	public static void setLanguage (string res)
	{
		string[] words = (Resources.Load (res) as TextAsset).text.Split ("\n" [0]);
		wordList = new SortedList<string,bool> ();
		foreach (string word in words)
			wordList.Add (word.Trim (), true);
	}

	void OnPlayerPickLetterHandler ()
	{
		
		//add to word string
		for (int i = GameManager_HMMS.Instance.PickedLetterList.Count - 1; i < GameManager_HMMS.Instance.PickedLetterList.Count; i++)
		{
			WordToCheck += GameManager_HMMS.Instance.PickedLetterList [i].letter;
		}

		if (GameManager_HMMS.Instance.PickedLetterList.Count > 2)
		{
			//check if word is valid
			if (isWord (WordToCheck))
			{
				if (!GameManager_HMMS.Instance.SoundOff)
				{
					GameManager_HMMS.Instance.source_WordReady.Play ();
				}
				if (!GameManager_HMMS.Instance.notFirstTime)
				{
					GameManager_HMMS.Instance.TiltImage.SetActive (false);
					GameManager_HMMS.Instance.TapImage.SetActive (true);

				}
				for (int i = 0; i < GameManager_HMMS.Instance.PickedLetterList.Count; i++)
				{
					GameManager_HMMS.Instance.PickedLetterList [i].LetterText.color = new Color32 (255, 240, 0, 255);// Color.yellow;
					GameManager_HMMS.Instance.PickedLetterList [i].myAnim.Play ("Star_Shine");

				}
			}
		}
	}

	void OnBlankPickHandler ()
	{
		StartCoroutine ("AddBlankScore");
		SpawnTapFX ();
	}


	void OnDestroy ()
	{
		Star.OnLetterPicked -= OnPlayerPickLetterHandler;
		MainMenuHandler.OnMainMenu -= HandleOnMainMenu;
		Star.OnBlankLetterPicked -= OnBlankPickHandler;

	}


	void SpawnTapFX ()
	{
		
		myPooledTapFX = GameObjectPool.GetPool ("TapFXPool").GetInstance ();
		TapFXObj = myPooledTapFX.GetComponent<TapFX> ();
		TapFXObj.transform.position = myBee.position;
		TapFXObj.transform.SetParent (GameManager_HMMS.Instance.FireflyKaBaap);
		TapFXObj.transform.rotation = Quaternion.identity;
	}

	IEnumerator AddScore ()
	{
		GameManager_HMMS.Instance.wordCount++;
		Analytics.CustomEvent ("wordMade", new Dictionary<string, object> {
			{ "WordLength", WordToCheck.Length },
			//{ "FirstLetterOfWord", WordToCheck [0].ToString () },
		});

		for (int i = 0; i < WordToCheck.Length; i++)
		{
			Analytics.CustomEvent ("wordMade", new Dictionary<string, object> {
				{ "letter" + i, WordToCheck [i].ToString () },
			});
		}

		if (!GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.source_CorrectWord.Play ();
		}
		GameManager_HMMS.Instance.WordCount += WordToCheck.Length;
		
		if (GameManager_HMMS.Instance.WordCount_Text != null)
			GameManager_HMMS.Instance.WordCount_Text.text = "Score:" + GameManager_HMMS.Instance.WordCount.ToString ("000"); 


		FlyingText.gameObject.SetActive (true);
		FlyingText.gameObject.GetComponent<TextMesh> ().text = "+" + WordToCheck.Length.ToString ();
		FlyingText.Play ("FlyingText_Fly");
		WordAnim.Play ("WordScore_Enlarge");
		yield return new WaitForSeconds (1f);
		WordAnim.Play ("WordScore_Idle");
		FlyingText.Play ("FlyingText_Idle");
		FlyingText.gameObject.SetActive (false);
	}


	IEnumerator AddBlankScore ()
	{
		GameManager_HMMS.Instance.WordCount++;
		if (GameManager_HMMS.Instance.WordCount_Text != null)
			GameManager_HMMS.Instance.WordCount_Text.text = "Score:" + GameManager_HMMS.Instance.WordCount.ToString ("000"); 


		FlyingText.gameObject.SetActive (true);
		FlyingText.gameObject.GetComponent<TextMesh> ().text = "+1";
		FlyingText.Play ("FlyingText_Fly");
		WordAnim.Play ("WordScore_Enlarge");
		yield return new WaitForSeconds (1f);
		WordAnim.Play ("WordScore_Idle");
		FlyingText.Play ("FlyingText_Idle");
		FlyingText.gameObject.SetActive (false);
	}
	                  
	


}