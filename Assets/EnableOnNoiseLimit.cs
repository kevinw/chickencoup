using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{

public class EnableOnNoiseLimit : MonoBehaviour {
	Renderer toEnable;

	void OnEnable () {
		toEnable = GetComponentInChildren<Renderer>();
		Events.Noise.NoiseLimitReached += _onNoiseLimit;
	}

	void OnDisable() {
		Events.Noise.NoiseLimitReached -= _onNoiseLimit;
	}

	void _onNoiseLimit()
	{
		if (toEnable)
			toEnable.enabled = true;

	}
	
}

}