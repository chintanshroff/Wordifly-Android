using UnityEngine;
using System.Collections;

public class TapFX : MonoBehaviour
{
	void OnEnable ()
	{
		StartCoroutine ("RemovePs");
	}
	
	IEnumerator RemovePs ()
	{
		yield return new WaitForSeconds (2f);
		GameObjectPool.GetPool ("TapFXPool").ReleaseInstance (transform);
	}
}
