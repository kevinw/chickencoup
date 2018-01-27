using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAfterSeconds : MonoBehaviour {

	public float Seconds;
	void OnEnable()
	{
        StartCoroutine(LateCall());
	}
 
    IEnumerator LateCall()
    {
         yield return new WaitForSeconds(Seconds);
         gameObject.SetActive(false);
    }
}
