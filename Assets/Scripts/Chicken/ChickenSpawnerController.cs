using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChickenCoup
{
	public class ChickenSpawnerController : MonoBehaviour {

		List<ChickenSpawner> spawners;
		List<GameObject> spawnedChickens;
		// Use this for initialization
		void Start () {
			Events.Noise.NoiseLimitReached += OnNoiseLimitReached;
			spawnedChickens = new List<GameObject>();
			spawners = GetComponentsInChildren<ChickenSpawner>().ToList();
			foreach (ChickenSpawner spawner in spawners)
			{
				spawnedChickens.Add(spawner.SpawnChicken());
			}
		}

		void OnNoiseLimitReached()
		{
			foreach (GameObject chicken in spawnedChickens)
			{
				if(!chicken.GetComponent<Recruitable>().Recruited)
				{
					chicken.gameObject.SetActive(false);
				}
			}
		}
	}
}
