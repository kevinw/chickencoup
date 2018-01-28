using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {
public class Cheats : MonoBehaviour {

	public GameObject followerPrefab;

	Recruitable SpawnChicken(Vector3 pos) {
		return Instantiate(followerPrefab, pos, Quaternion.identity).GetComponent<Recruitable>();
	}

	void SpawnFollowers(int numberOfFollowers)
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		var line = player.GetComponent<ChickenLine>();

		var xz = Random.insideUnitCircle * Random.value * 3f;
		line.AddFollowingChicken(SpawnChicken(player.transform.position + new Vector3(xz.x, 0, xz.y)));
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			SpawnFollowers(5);
		}
	}
}
}