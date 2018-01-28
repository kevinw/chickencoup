using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {

public class Mine : MonoBehaviour {
	public float ExplosiveForce = 100f;
	public float ExplosionRadius = 2f;
	public float ExplosionUpwards = 3f;
    void OnTriggerEnter(Collider other) {
		var rb = other.GetComponent<Rigidbody>();
		if (rb)
			rb.AddExplosionForce(ExplosiveForce, transform.position, ExplosionRadius, ExplosionUpwards);

		var rec = other.GetComponent<Recruitable>();
		if (rec)
			FindObjectOfType<ChickenLine>().KillChicken(rec);

		Destroy(gameObject);
    }
}

}