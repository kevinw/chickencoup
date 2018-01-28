using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {

public class Nirvana : MonoBehaviour {
	static float AfterTriggerEndGameSeconds = 5.0f;
	public static void ReachNirvana(GameObject obj)
	{
		var enlightenment = obj.GetComponent<RaiseToSky>();
		if (!enlightenment)
		{
			obj.AddComponent<RaiseToSky>();
			{
				var b = obj.GetComponent<Rigidbody>();
				b.useGravity = false;
			}
		}
	}

	static bool endGameTriggered;

	void EndGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("results");

	}

	void OnTriggerEnter(Collider collider)
	{
		var recruitable = collider.GetComponent<Recruitable>();
		var coreChicken = collider.GetComponent<CoreChicken>();
		if (recruitable || coreChicken)
		{
			ReachNirvana(collider.gameObject);
			if (coreChicken && !endGameTriggered)
			{
				endGameTriggered = true;
				Invoke("EndGame", AfterTriggerEndGameSeconds);
			}
		}
	}
}

}