using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var psys = GetComponent<ParticleSystem>();
		var main = psys.emission;
		main.enabled = false;
	}
}
