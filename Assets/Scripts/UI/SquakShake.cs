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
		public bool shakeMax;
		void Start () {
			Events.Noise.IncreaseNoise += OnNoiseIncreased;
			Events.Noise.NoiseLimitReached += OnNoiseLimitReached;
			amplitude = 5;

			//final movement
			rect = GetComponent<RectTransform>();
			target = rect.localPosition + new Vector3(0,200,0);
		}

		RectTransform rect;
		Vector3 target;
		
		void OnNoiseIncreased(int val)
		{
			//
			//amp = noise amount * ampMax / noise limit
			int ampAmount = 5 + (GlobalState.TotalNoise * 20) / GlobalState.NoiseLimit;
			StartCoroutine(ShakeMeter(ampAmount));
		}

		void OnNoiseLimitReached()
		{
			shakeMax = true;
			amplitude = 20;
			Events.Noise.IncreaseNoise -= OnNoiseIncreased;
		}

		IEnumerator ShakeMeter(int ampAmount)
		{
			shake = true;
			amplitude = ampAmount;
			yield return new WaitForSeconds(0.6f);
			shake = false;
			transform.rotation = Quaternion.identity;
		}

		void Update()
		{
			if(shake || shakeMax)
			{
				float val = amplitude * Mathf.Sin(Time.time * shakeFactor);
				transform.Rotate(Vector3.forward, val);
			}
			if(shakeMax)
			{
				rect.localPosition = Vector3.MoveTowards(rect.localPosition, target, 0.2f);
			}
		}
	}
}
