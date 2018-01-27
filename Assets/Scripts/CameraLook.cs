using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace  ChickenCoup
{
    public class CameraLook : MonoBehaviour {

        Camera cam;
        public GameObject target;

        void Start()
        {
            cam = Camera.main;
            Assert.IsNotNull(cam);
        }

        void Update () {
            cam.transform.LookAt(target.transform);
        }
    }
}
