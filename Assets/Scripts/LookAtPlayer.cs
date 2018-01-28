using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
	Transform player;
	void OnEnable()
	{
		var obj = GameObject.FindGameObjectWithTag("Player");
		if (obj) {
			player = obj.transform;
		}
	}

	void Update () {
		transform.LookAt(player, Vector3.up);
	}
}
