using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public class GroundChangeOnNoiseLimit : MonoBehaviour {

		public Material MatToStartWith;
		public Material MatToChangeTo;
		// Use this for initialization
		void Start () {
			GetComponent<MeshRenderer>().sharedMaterial = MatToStartWith;
			Events.Noise.NoiseLimitReached += OnNoiseLimitReached;
		}
		
		void OnNoiseLimitReached()
		{
			GetComponent<MeshRenderer>().sharedMaterial = MatToChangeTo;
		}
	}
}