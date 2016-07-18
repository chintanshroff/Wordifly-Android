using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour
{
	private float SkySpeed;
	// Use this for initialization
	void Start ()
	{
		SkySpeed = GameManager_HMMS.Instance.MaxStarSpeed - 0.8f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager_HMMS.Instance.CurrentState == GameState.Playing) {
			if (transform.position.y < -11.5f) {
				//Destroy (gameObject);
				transform.position = new Vector3 (0, 11.5f, 8);
			} 
			transform.position -= Vector3.up * SkySpeed * Time.deltaTime;
		}
	}
}
