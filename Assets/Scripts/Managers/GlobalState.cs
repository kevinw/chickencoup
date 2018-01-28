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

		static bool FiredNoiseLimit;

		void Start () {
			FiredNoiseLimit = false;
			Assert.IsNotNull(NoiseMeter);
			TotalNoise = 0;
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
			NoiseLimit = 20;
		}

		public static bool NoiseLimitWasReached {
			get {
				return FiredNoiseLimit;
			}
		}

		bool _noiseLimitWasReached = false;
		
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
