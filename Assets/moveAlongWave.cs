using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAlongWave : MonoBehaviour {

	Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Mathf.Sin((Time.time + startPosition.x) * 5.0f) * 0.25f;
		transform.position = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);
	}
}
