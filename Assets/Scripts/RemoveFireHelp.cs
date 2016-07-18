using UnityEngine;
using System.Collections;

public class RemoveFireHelp : MonoBehaviour
{

	void OnEnable ()
	{
		StartCoroutine (SelfDestroy ());

	}

	IEnumerator SelfDestroy ()
	{
		yield return new WaitForSeconds (6f);
		if (!GameManager_HMMS.Instance.PauseButton.gameObject.activeInHierarchy)
			GameManager_HMMS.Instance.PauseButton.gameObject.SetActive (true);
		this.gameObject.SetActive (false);
	}
}
