using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {
public class DestroyOnNoiseLimitReached : MonoBehaviour {
	void OnEnable () {
		Events.Noise.NoiseLimitReached += OnNoiseLimitReached;
	}

	void OnDisable() {
		Events.Noise.NoiseLimitReached -= OnNoiseLimitReached;
	}

	void OnNoiseLimitReached()
	{
		Destroy(gameObject);
	}
}
}