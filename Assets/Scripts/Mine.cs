using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

namespace ChickenCoup {
	public class Mine : MonoBehaviour {
		[FMODUnity.EventRef] public string ExplodeSound;
		[FMODUnity.EventRef] public string CountdownSound;		
		public GameObject ExplosionRadiusObject;
		bool explosionStarted;

		void Start()
		{
			ExplosionRadiusObject.GetComponent<MineExplosionRadius>().SetParentMine(this);
			explosionStarted = false;
		}
		void OnTriggerEnter(Collider other) {
			// var rb = other.GetComponent<Rigidbody>();
			if(!explosionStarted)
			{
				ExplosionRadiusObject.GetComponent<MineExplosionRadius>().Explode();
				explosionStarted = true;
			}
		}
		public void PlayExplosionSound()
		{
			if (!string.IsNullOrEmpty(ExplodeSound))
			{
				FMODUnity.RuntimeManager.PlayOneShot(ExplodeSound, transform.position);
			}
		}
		public void PlayCountdownSound()
		{
			if (!string.IsNullOrEmpty(ExplodeSound))
			{
				FMODUnity.RuntimeManager.PlayOneShot(CountdownSound, transform.position);
			}
		}
		
		public void DestroyMine()
		{
			Destroy(gameObject);
		}
	}
}