using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyManager : MonoBehaviour
{


	public List<Sky>SkyList = new List<Sky> (); 
	private Sky skyObj;
	private Sky currentSkyObj;
	private int skyNum = 0;



	// Use this for initialization
	void Start ()
	{
		CreateFirstSky ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		CreateRestSkys ();
	}
	
	void CreateFirstSky ()
	{
		skyObj = Instantiate (SkyList [skyNum], new Vector3 (0, 0, 8), Quaternion.identity) as Sky;
		currentSkyObj = skyObj;
		skyNum++;

	}

	void CreateRestSkys ()
	{
		if (skyNum < SkyList.Count) {
			if (currentSkyObj.transform.position.y < 0f) {
				skyObj = Instantiate (SkyList [skyNum], new Vector3 (0, 11.5f, 8), Quaternion.identity) as Sky;
				currentSkyObj = skyObj;
				skyNum++;
			}
		} //else if (skyNum == SkyList.Count) {
		//skyNum = 0;
		//}
	}
}
