using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


namespace ChickenCoup
{
	public class ExplodeWalls : MonoBehaviour {
		public Transform[] wallsToExplode;
		public Transform explodePoint;
		public float explodeRadius = 10f;
		public float explodeForce = 100f;

		public bool ExplodeNextFrame = false;

		[FMODUnity.EventRef]
        public string ExplodeSound;

		void Update()
		{
			if (ExplodeNextFrame)
			{
				ExplodeNextFrame = false;
				DoExplode();
			}
		}

		void Start()
		{
			Events.Noise.NoiseLimitReached += DoExplode;
		}

		public void DoExplode()
		{
			Events.Noise.NoiseLimitReached -= DoExplode;
			FMODUnity.RuntimeManager.PlayOneShot(ExplodeSound, transform.position);
			foreach (var g in wallsToExplode)
			{
				var obj = g.gameObject;
				if (obj.GetComponent<Rigidbody>() == null)
					obj.AddComponent<Rigidbody>();
				var r = obj.GetComponent<Rigidbody>();
				r.AddExplosionForce(explodeForce, explodePoint.transform.position, explodeRadius, 0.5f, ForceMode.Impulse);
			}
		}

	}
}