using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraLook : MonoBehaviour {

    Camera camera;
    public GameObject target;

    void Start()
    {
        camera = Camera.main;
        Assert.IsNotNull(camera);
    }

	void Update () {
		camera.transform.LookAt(target.transform);
	}
}
