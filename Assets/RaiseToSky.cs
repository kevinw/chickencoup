using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {
public class RaiseToSky : MonoBehaviour {

	public float RandomSpeedMin = 4.0f;
	public float RandomSpeedMax = 9.0f;

	float Speed;

	void Start () {
		var v = GetComponent<Verlet3D>();
		if (v) v.enabled = false;

		var follow = GetComponent<VerletFollow>();
		if (follow) follow.enabled = false;

		Speed = Random.Range(RandomSpeedMin, RandomSpeedMax);
	}

	void FixedUpdate()
	{
		var p = transform.position;
		p.y += Speed * Time.deltaTime;
		transform.position = p;
	}
}

}