using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class ChickenSpawner : MonoBehaviour {

        public GameObject ChickenPrefab;

        void Start()
        {
            //spawn the chicken in the location
            Instantiate(ChickenPrefab, transform.position, Quaternion.identity);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, .3f);
        }
    }
}