using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class GlobalState : MonoBehaviour {
		public static int TotalNoise { get; private set;}
		public static int NoiseLimit { get; private set;}
		public Slider NoiseMeter;

		bool FiredNoiseLimit;

		void Start () {
			Assert.IsNotNull(NoiseMeter);
			TotalNoise = 0;
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
			NoiseLimit = 20;
		}
		
		void OnNoiseIncreased(int amount)
		{
			TotalNoise += amount;
			NoiseMeter.value = (float)TotalNoise / (float)NoiseLimit;
			if(!FiredNoiseLimit && TotalNoise >= NoiseLimit)
			{
				FiredNoiseLimit = true;
				if(Events.Noise.NoiseLimitReached != null){Events.Noise.NoiseLimitReached();}
				Debug.Log("noise limit reached");
			}
		}
	}
}
