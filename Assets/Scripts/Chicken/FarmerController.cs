using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FMOD.Studio;

namespace ChickenCoup
{
	public class FarmerController : MonoBehaviour {

		public GameObject PlayerChicken;
		// Use this for initialization
		public float maxDelta = .1f;
		bool Activated;
		void Start () {
			Assert.IsNotNull(PlayerChicken);
			Events.Noise.NoiseLimitReached += OnNoiseLimitReached;
			Activated = false;
		}

		void OnNoiseLimitReached()
		{
			Activated = true;
		}

		void Update()
		{
			if(Activated)
			{
				transform.position = Vector3.MoveTowards(transform.position, PlayerChicken.transform.position, maxDelta);
			}
		}

		void OnTriggerEnter(Collider other) 
		{
			if(other.GetComponent<Recruitable>() != null) //it's a recruit
			{
				Recruitable chicken = other.GetComponent<Recruitable>();
				if(Events.Recruitment.KillChicken != null){Events.Recruitment.KillChicken(chicken);}
			}
		}
	}
}