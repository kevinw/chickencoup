using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {
	public float Seconds = 5f;

	IEnumerator Start () {
		yield return new WaitForSeconds(Seconds);
		Destroy(gameObject);
	}
}
