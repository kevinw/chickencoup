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

		[FMODUnity.EventRef]
        public string RecruitingSoundEvent;
        FMOD.Studio.EventInstance recruitingSound;
		[FMODUnity.EventRef]
        public string EscapeSoundEvent;
        FMOD.Studio.EventInstance escapeSound;

		public static int TotalPointsWon;
		public static int ChickensSaved;
		static int DEFAULT_NOISE_LIMIT = 65;

		void Start () {
			TotalPointsWon = 0;
			ChickensSaved = 0;
			DontDestroyOnLoad(this);
			FiredNoiseLimit = false;
			Assert.IsNotNull(NoiseMeter);
			TotalNoise = 0;
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
			NoiseLimit = DEFAULT_NOISE_LIMIT;
			Events.Noise.NoiseLimitReached += OnNoiseLimitReached;

			recruitingSound = FMODUnity.RuntimeManager.CreateInstance(RecruitingSoundEvent);

			escapeSound = FMODUnity.RuntimeManager.CreateInstance(EscapeSoundEvent);
			recruitingSound.start();

			Events.Recruitment.Nirvana += OnNir;
		}

		public static bool NoiseLimitWasReached {
			get {
				return FiredNoiseLimit;
			}
		}

		void OnNir(Recruitable r)
		{
			ChickensSaved++;
			TotalPointsWon +=  r.GetComponent<ChickenProperties>().Points;
			Debug.Log("chickens: " + ChickensSaved + "  points total:" + TotalPointsWon);
		}

		void OnNoiseLimitReached() {
			recruitingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);	
			escapeSound.start();
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
