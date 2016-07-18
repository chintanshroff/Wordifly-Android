using UnityEngine;
using System.Collections;


public delegate void LetterPickEvent ();

public class Star : MonoBehaviour
{
	public static event LetterPickEvent OnLetterPicked;
	public static event LetterPickEvent OnBlankLetterPicked;

	public string letter;
	public int randomIndex;
	public TextMesh LetterText;
	public int letterType;
	private float thisStarSpeed;
	private Transform myPooledPS_Yel;
	private StarPickUpPS PS_Yel_Obj;
	public bool picked;
	private Star myStarScript;
	public Animator myAnim;
	private Color startFontColor;
	public bool BlankStar;

	public void OnEnable ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {
			this.thisStarSpeed = GameManager_HMMS.Instance.starSpeed;


			if (GameManager_HMMS.Instance.notFirstTime) {
				this.getLetter ();
			} else {
				if (GameManager_HMMS.Instance.starCollectedForTut == 0) {
					this.letter = "C";
					this.LetterText.text = this.letter; 
				} else if (GameManager_HMMS.Instance.starCollectedForTut == 1) {
					this.letter = "A";
					this.LetterText.text = this.letter; 
				} else if (GameManager_HMMS.Instance.starCollectedForTut == 2) {
					this.letter = "T";
					this.LetterText.text = this.letter; 
				}
			}
		}
	}

	void Awake ()
	{
		MainMenuHandler.OnMainMenu += OnQuit;
		myStarScript = GetComponent<Star> ();
		myAnim = GetComponent<Animator> ();
		startFontColor = LetterText.color;
	}


	void Update ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {
			StarHandler ();
		} else if (GameManager_HMMS.Instance.CurrentState == GameState.GameOver) {
			RemoveStar ();
		}
	}

	void getLetter ()
	{
		
		int RandomBlank = Random.Range (0, 17);
		if (RandomBlank == 0 && GameManager_HMMS.Instance.WordCount > 1) {
			this.letter = null;
			this.LetterText.text = ""; 
			myAnim.Play ("Star_Blank");
			BlankStar = true;
		} else {
			this.letterType = Random.Range (1, 6);

			if (this.letterType <= 2) {
				this.randomIndex = Random.Range (0, GameManager_HMMS.Instance.VowelList.Count);
				this.letter = GameManager_HMMS.Instance.VowelList [this.randomIndex];
				this.LetterText.text = this.letter;
				BlankStar = false;

			} else if (this.letterType > 2 && this.letterType <= 5) {
				this.randomIndex = Random.Range (0, 21);
				this.letter = GameManager_HMMS.Instance.ConsonantList [this.randomIndex];
				this.LetterText.text = this.letter;
				BlankStar = false;
			} 
				
		}
	}

	void  StarHandler ()
	{
		if (!picked) {
			thisStarSpeed = GameManager_HMMS.Instance.starSpeed;
			if (GameManager_HMMS.Instance.notFirstTime) {
				transform.position -= Vector3.up * thisStarSpeed * Time.deltaTime;
				if (transform.position.y <= -7f) {
					RemoveStar ();
				}
			} else {
				if (transform.position.y >= GameManager_HMMS.Instance.FireflyKaBaap.transform.position.y) {
					transform.position -= Vector3.up * thisStarSpeed * Time.deltaTime;
				}
			}
		} 
	}

	public void RemoveStar ()
	{
		if (this.transform.position.y >= -5 && this.transform.position.y <= 0.6f) {
			SpawnPickUp_PS ();

			
		} else if (transform.position.y > 0.6f) {
			SpawnDrop_PS ();

			
		}
		if (LetterText.color != startFontColor) {
			LetterText.color = startFontColor;
			myAnim.Play ("Star_Idle");
		}

		
		GameObjectPool.GetPool ("Star_Pool").ReleaseInstance (transform);
	}

	public void SpawnPickUp_PS ()
	{
		myPooledPS_Yel = GameObjectPool.GetPool ("YelStarCollectPool").GetInstance ();
		PS_Yel_Obj = myPooledPS_Yel.GetComponent<StarPickUpPS> ();
		PS_Yel_Obj.transform.position = this.transform.position;
	}

	public void SpawnDrop_PS ()
	{
		myPooledPS_Yel = GameObjectPool.GetPool ("YelStarCollectPool").GetInstance ();
		PS_Yel_Obj = myPooledPS_Yel.GetComponent<StarPickUpPS> ();
		PS_Yel_Obj.transform.position = this.transform.position;
	}

	public void OnPlayerCollideWithStar ()
	{
		picked = true;
		SpawnPickUp_PS ();
		if (!GameManager_HMMS.Instance.SoundOff) {
			GameManager_HMMS.Instance.source_Pick.Play ();
		}
		if (!BlankStar) {
			GameManager_HMMS.Instance.PickedLetterList.Add (myStarScript);
			if (OnLetterPicked != null) {
				OnLetterPicked ();
			}
			float x = GameManager_HMMS.Instance.emptyStarHUDList [GameManager_HMMS.Instance.PickedLetterList.Count - 1].position.x;
			float y = GameManager_HMMS.Instance.emptyStarHUDList [GameManager_HMMS.Instance.PickedLetterList.Count - 1].position.y;
			transform.position = new Vector3 (x, y, -3f);
		} else {
			if (OnBlankLetterPicked != null) {
				OnBlankLetterPicked ();
			}
			RemoveStar ();
		}
	}

	void OnDestroy ()
	{
		MainMenuHandler.OnMainMenu -= OnQuit;
	}


	void OnQuit ()
	{
		if (this.gameObject.activeInHierarchy) {
			RemoveStar ();
		}
	}
}

