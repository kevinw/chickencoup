using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {
public class Cheats : MonoBehaviour {

	Recruitable SpawnChicken() {
		return null;
	}

	void SpawnFollowers(int numberOfFollowers)
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		var line = player.GetComponent<ChickenLine>();
		line.AddFollowingChicken(SpawnChicken());
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			SpawnFollowers(5);

		}
		
	}
}
}