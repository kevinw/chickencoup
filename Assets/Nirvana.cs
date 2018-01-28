using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {

public class Nirvana : MonoBehaviour {
	public static void ReachNirvana(GameObject obj)
	{
		var enlightenment = obj.GetComponent<RaiseToSky>();
		if (!enlightenment)
		{
			obj.AddComponent<RaiseToSky>();
			//if (obj.tag != "Player")
			{
				var b = obj.GetComponent<Rigidbody>();
				b.useGravity = false;
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		var recruitable = collider.GetComponent<Recruitable>();
		var coreChicken = collider.GetComponent<CoreChicken>();
		if (recruitable || coreChicken)
			ReachNirvana(collider.gameObject);
	}
}

}