using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

namespace ChickenCoup {

public class Mine : MonoBehaviour {
	[FMODUnity.EventRef] public string ExplodeSound;
	public float ExplosiveForce = 100f;
	public float ExplosionRadius = 2f;
	public float ExplosionUpwards = 3f;
    void OnTriggerEnter(Collider other) {
		var rb = other.GetComponent<Rigidbody>();

		if (!string.IsNullOrEmpty(ExplodeSound))
			FMODUnity.RuntimeManager.PlayOneShot(ExplodeSound, transform.position);

		if (rb)
			rb.AddExplosionForce(ExplosiveForce, transform.position, ExplosionRadius, ExplosionUpwards);

		var rec = other.GetComponent<Recruitable>();
		if (rec)
			FindObjectOfType<ChickenLine>().KillChicken(rec);

		Destroy(gameObject);
    }
}

}