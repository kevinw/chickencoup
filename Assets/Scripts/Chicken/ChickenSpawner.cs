using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class ChickenSpawner : MonoBehaviour {

        public GameObject ChickenPrefab;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, .3f);
        }

        public GameObject SpawnChicken()
        {
            //spawn the chicken in the location
            GameObject spawnedChicken = Instantiate(ChickenPrefab, transform.position, Quaternion.identity);
            return spawnedChicken;
        }
    }
}