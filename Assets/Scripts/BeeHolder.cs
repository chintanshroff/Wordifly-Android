using UnityEngine;
using System.Collections;

public delegate void FlyDieEvent ();
public class BeeHolder : MonoBehaviour
{
	public static event FlyDieEvent OnGameOver;

	public float BeeSpeed_Keyboard;
	public float BeeSpeed_Touch;
	private const float ROTATE_AMOUNT = 15;
	private float currentTailScore;
	private Animator BeeAnimator;
	//public Transform Hurry;
	private bool AchieveComplete;
	//public GameObject myShield;
	private float cachedY;



	void Awake ()
	{
		wordCheck.IncreaseTrail += OnIncreaseTrail;
		BeeAnimator = GetComponentInChildren<Animator> ();
		AchievementHandler.OnAchievementComplete += IncreaseTrailLength;
		Star.OnBlankLetterPicked += OnBlankPickedHandler;

	}

	void Start ()
	{
		cachedY = transform.position.y;
	}

	void Update ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing)
		{
			BeeMovement ();
			BeeMovementMobile ();
			//	HandleBeeMovementSpeed ();



			if (!GameManager_HMMS.Instance.WordMade)
			{
				DecreaseTrail ();
			}
			else
			{
				currentTailScore -= Time.deltaTime;
				if (currentTailScore <= 0)
				{
					currentTailScore = 0;
					GameManager_HMMS.Instance.WordMade = false;
				}
			}
		}
	}

	void OnIncreaseTrail ()
	{
		Console.Log ("Score before word = " + currentTailScore);
		currentTailScore += GameManager_HMMS.Instance.tailScore;
		Console.Log ("Score on word = " + currentTailScore);
		if (GameManager_HMMS.Instance.HurryText.activeInHierarchy)
			GameManager_HMMS.Instance.HurryText.SetActive (false);
		
		GameManager_HMMS.Instance.WordMade = true;
		BeeAnimator.Play ("Fly");
		if (!AchieveComplete)
		{
			GameManager_HMMS.Instance.Trail_ps.startLifetime = 1.5f;
		}
		else
		{
			GameManager_HMMS.Instance.Trail_ps.startLifetime = 2f;
			Console.Log ("Achievementcomplete");
			AchieveComplete = false;
		}
		GameManager_HMMS.Instance.Trail_ps.startSize = .32f;
		GameManager_HMMS.Instance.Trail_ps.startColor = new Color (1, 1, 0, 1); //tail color is yellow
		GameManager_HMMS.Instance.BeeShield.SetActive (true);
	}

	
	
	void DecreaseTrail ()
	{
		if (GameManager_HMMS.Instance.WordCount > 0)
		{
			GameManager_HMMS.Instance.Trail_ps.startLifetime -= GameManager_HMMS.Instance.startLifetimeReducer;
			GameManager_HMMS.Instance.BeeShield.SetActive (false);
			if (GameManager_HMMS.Instance.Trail_ps.startLifetime >= 1f)
			{
				GameManager_HMMS.Instance.Trail_ps.startColor = new Color (1, .6f, 0, 1); //tail color is orange
			}
			else if (GameManager_HMMS.Instance.Trail_ps.startLifetime < 1f)
			{
				
				GameManager_HMMS.Instance.Trail_ps.startColor = new Color (1, .3f, 0, 1); //tail color is red
				BeeAnimator.Play ("Fly_Dying");
				if (!GameManager_HMMS.Instance.HurryText.activeInHierarchy)
					GameManager_HMMS.Instance.HurryText.SetActive (true);
			
				
				if (GameManager_HMMS.Instance.Trail_ps.startLifetime <= 0.05f)
				{
					//GameManager_HMMS.Instance.CurrentState = GameState.SaveMe;

				
					GameManager_HMMS.Instance.Trail_ps.Stop ();
					if (GameManager_HMMS.Instance.HurryText.activeInHierarchy)
						GameManager_HMMS.Instance.HurryText.SetActive (false);
						

					GameManager_HMMS.Instance.CurrentState = GameState.GameOver;
					StartCoroutine ("FlyDie");
				}
			}
		}
	}

	void HandleBeeMovementSpeed ()
	{
		if (GameManager_HMMS.Instance.WordCount < 5)
		{
			BeeSpeed_Keyboard = 4;
			BeeSpeed_Touch = 0.4f;
		}
		else if (GameManager_HMMS.Instance.WordCount >= 5 && GameManager_HMMS.Instance.WordCount < 15)
		{
			BeeSpeed_Keyboard = 4.5f;
			BeeSpeed_Touch = 0.5f;
		}
		else if (GameManager_HMMS.Instance.WordCount >= 15 && GameManager_HMMS.Instance.WordCount < 25)
		{
			BeeSpeed_Keyboard = 5.5f;
			BeeSpeed_Touch = 0.6f;
		}
		else if (GameManager_HMMS.Instance.WordCount >= 25)
		{
			BeeSpeed_Keyboard = 6;
			BeeSpeed_Touch = 0.7f;
		}
	}

	
	float GetTiltValue ()
	{
		const float TILT_MIN = 0.005f;
		const float TILT_MAX = 0.2f;
		
		// Work out magnitude of tilt
		float tilt = Mathf.Abs (Input.acceleration.x);
		
		// If not really tilted don't change anything
		if (tilt < TILT_MIN)
		{
			return 0;
		}
		float tiltScale = (tilt - TILT_MIN) / (TILT_MAX - TILT_MIN);
		
		// Change scale to be negative if accel was negative
		if (Input.acceleration.x < 0)
		{
			return tiltScale;
		}
		else
		{
			return -tiltScale;
		}
	}

	void BeeMovementMobile ()
	{
		float tiltValue = GetTiltValue ();
		Vector3 oldAngles = this.transform.eulerAngles;
		this.transform.eulerAngles = new Vector3 (oldAngles.x, oldAngles.y, oldAngles.z + (tiltValue * ROTATE_AMOUNT));
		
		float delta = Input.acceleration.x;
		transform.position += new Vector3 (Mathf.Clamp (delta * BeeSpeed_Touch, -1f, 1.0f), 0, 0);
		
		//Clamp angles
		float rotZ = ClampAngle (transform.eulerAngles.z, -30, 30);
		transform.eulerAngles = new Vector3 (0, 0, rotZ);
		
		//side teleportation

		if (transform.position.x > GameManager_HMMS.Instance.screenSizeInWord.x)
		{ //-3.1
			transform.position = new Vector2 (-GameManager_HMMS.Instance.screenSizeInWord.x, cachedY);
		}
		else if (transform.position.x < -GameManager_HMMS.Instance.screenSizeInWord.x)
		{
			transform.position = new Vector2 (GameManager_HMMS.Instance.screenSizeInWord.x, cachedY);
		}
	}

	void BeeMovement ()
	{
		if (Input.GetKey (KeyCode.A))
		{
			gameObject.transform.Translate (Vector3.left * BeeSpeed_Keyboard * Time.deltaTime);
		}
		else if (Input.GetKey (KeyCode.D))
		{
			gameObject.transform.Translate (Vector3.right * BeeSpeed_Keyboard * Time.deltaTime);
		}
	}

	private float ClampAngle (float angle, float min, float max)
	{
		
		if (angle < 90 || angle > 270)
		{
			if (angle > 180)
				angle -= 360;
			if (max > 180)
				max -= 360;
			if (min > 180)
				min -= 360;
		} 
		
		angle = Mathf.Clamp (angle, min, max);
		
		if (angle < 0)
			angle += 360;
		
		return angle;
	}

	IEnumerator FlyDie ()
	{
		BeeAnimator.Play ("Fly_Die");
		if (!GameManager_HMMS.Instance.SoundOff)
		{
			GameManager_HMMS.Instance.source_Dead.Play ();
		}
		GameManager_HMMS.Instance.FireflyKaBaap.rotation = Quaternion.Euler (0, 0, 0);
		GameManager_HMMS.Instance.HUD_Panel.gameObject.SetActive (false);
		GameManager_HMMS.Instance.AchievementNotification.gameObject.SetActive (false);
		if (GameManager_HMMS.Instance.CompleteText.gameObject.activeInHierarchy)
		{
			GameManager_HMMS.Instance.CompleteText.gameObject.SetActive (false);
			
		}

		yield return new WaitForSeconds (1.4f);
		if (OnGameOver != null)
		{
			OnGameOver ();
		}
		this.gameObject.SetActive (false);
	}

	void IncreaseTrailLength ()
	{
		AchieveComplete = true;
	}

	void OnBlankPickedHandler ()
	{
		currentTailScore += 1f;
		if (GameManager_HMMS.Instance.Trail_ps.startLifetime >= 1f)
		{
			GameManager_HMMS.Instance.Trail_ps.startLifetime = 1.5f;

		}
		else
		{
			GameManager_HMMS.Instance.Trail_ps.startLifetime += 1f;
		}
		if (GameManager_HMMS.Instance.HurryText.activeInHierarchy)
			GameManager_HMMS.Instance.HurryText.SetActive (false);
		BeeAnimator.Play ("Fly");
	}


	void OnDestroy ()
	{
		wordCheck.IncreaseTrail -= OnIncreaseTrail;
		AchievementHandler.OnAchievementComplete -= IncreaseTrailLength;
		Star.OnBlankLetterPicked -= OnBlankPickedHandler;

	}

}
