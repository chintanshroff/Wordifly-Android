using UnityEngine;
using System.Collections;

public class StarPickUpPS : MonoBehaviour
{

	void OnEnable ()
	{
		StartCoroutine ("RemovePs");
	}



	IEnumerator RemovePs ()
	{
		yield return new WaitForSeconds (0.5f);
		GameObjectPool.GetPool ("YelStarCollectPool").ReleaseInstance (transform);
	}
}
