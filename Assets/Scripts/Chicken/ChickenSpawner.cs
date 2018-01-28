using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class ChickenSpawner : MonoBehaviour {

        public GameObject ChickenPrefab;
        public Vector3 offset;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, .3f);
            Gizmos.DrawRay(new Ray(transform.position, transform.forward));
        }

        public GameObject SpawnChicken()
        {
            //spawn the chicken in the location
            GameObject spawnedChicken = Instantiate(ChickenPrefab, transform.position + offset, transform.rotation);
            return spawnedChicken;
        }
    }
}