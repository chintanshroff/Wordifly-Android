using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCamera : MonoBehaviour
{
	
	
	private Camera MyCam;
	public List<Color> ColorList = new List<Color> ();
	private int currentColorIndex;
	private int nextColorIndex;
	private float t = 0;
	
	
	// Use this for initialization
	
	void Awake ()
	{
		MyCam = this.gameObject.GetComponent<UnityEngine.Camera> ();
		MainMenuHandler.OnMainMenu += ResetBGColor;
	}

	void Start ()
	{
		CameraFlags ();
		currentColorIndex = Random.Range (0, ColorList.Count);
		nextColorIndex = -1;
		MyCam.backgroundColor = ColorList [currentColorIndex];
		GameManager_HMMS.Instance.startColor = ColorList [currentColorIndex];
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {
			
			if (currentColorIndex == ColorList.Count - 1) {
				if (nextColorIndex == -1) {
					nextColorIndex = currentColorIndex - (ColorList.Count - 1);
				}
				
				if (t < 1) {
					t += Time.deltaTime / 70;
				} else if (t >= 1) {
					currentColorIndex = nextColorIndex;
					nextColorIndex = currentColorIndex + 1;
					Console.Log ("currentColorIndex: " + currentColorIndex);
					Console.Log ("nextColorIndex: " + nextColorIndex);
					t = 0;
				}
			} else if (currentColorIndex < ColorList.Count - 2) {
				if (nextColorIndex == -1) {
					nextColorIndex = currentColorIndex + 1;
				}
				if (t < 1) {
					t += Time.deltaTime / 70;
				} else if (t >= 1) {
					currentColorIndex = nextColorIndex;
					nextColorIndex = currentColorIndex + 1;
					Console.Log ("currentColorIndex: " + currentColorIndex);
					Console.Log ("nextColorIndex: " + nextColorIndex);
					t = 0;
					
				} 
			} else if (currentColorIndex == ColorList.Count - 2) {
				if (nextColorIndex == -1) {
					nextColorIndex = currentColorIndex + 1;
				}
				if (t < 1) {
					t += Time.deltaTime / 70;
				} else if (t >= 1) {
					currentColorIndex = nextColorIndex;
					nextColorIndex = currentColorIndex - (ColorList.Count - 1);
					Console.Log ("currentColorIndex: " + currentColorIndex);
					Console.Log ("nextColorIndex: " + nextColorIndex);
					t = 0;
					
				}
			}
			MyCam.backgroundColor = Color.Lerp (ColorList [currentColorIndex], ColorList [nextColorIndex], t);
			
		}*/
	}

	void CameraFlags ()
	{
		MyCam.clearFlags = CameraClearFlags.SolidColor;
	}

	void ResetBGColor ()
	{
		currentColorIndex = Random.Range (0, ColorList.Count);
		nextColorIndex = -1;
		MyCam.backgroundColor = ColorList [currentColorIndex];
		t = 0;
		GameManager_HMMS.Instance.startColor = MyCam.backgroundColor;
	}

	void OnDestroy ()
	{
		MainMenuHandler.OnMainMenu -= ResetBGColor;
	}
}
