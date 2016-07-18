using UnityEngine;
using System.Collections;

public class dummy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GOMovement ();
	}

	void GOMovement()
	{
		if (Input.GetKey(KeyCode.A))
		{
			gameObject.transform.Translate(Vector3.left * 4 * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			gameObject.transform.Translate(Vector3.right * 4 * Time.deltaTime);
			
		}
		
	}
}
