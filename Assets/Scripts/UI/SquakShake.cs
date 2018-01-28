using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public class SquakShake : MonoBehaviour {

		[Range(0,100)]
		public int shakeFactor;
		[Range(0,100)]
		public int amplitude;
		// Use this for initialization
		public bool shake;
		void Start () {
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
		}
		
		void OnNoiseIncreased(int val)
		{

		}

		void Update()
		{
			if(shake)
			{
				float val = amplitude * Mathf.Sin(Time.time * shakeFactor);
				transform.Rotate(Vector3.forward, val);
			}
		}
	}
}
