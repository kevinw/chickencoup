using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace  ChickenCoup
{
    public class Look : MonoBehaviour {
        public GameObject target;

        void Update () {
            transform.LookAt(target.transform);
        }
    }
}
