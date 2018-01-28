using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class GlobalState : MonoBehaviour {
		public int TotalNoise { get; private set;}
		public int NoiseLimit;
		public Slider NoiseMeter;
		void Start () {
			Assert.IsNotNull(NoiseMeter);
			TotalNoise = 0;
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
		}
		
		void OnNoiseIncreased(int amount)
		{
			TotalNoise += amount;
			NoiseMeter.value = (float)TotalNoise / (float)NoiseLimit;
			if(TotalNoise >= NoiseLimit)
			{
				if(Events.Noise.NoiseLimitReached != null){Events.Noise.NoiseLimitReached();}
				Debug.Log("noise limit reached");
			}
		}
	}
}
