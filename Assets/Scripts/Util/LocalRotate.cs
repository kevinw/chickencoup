using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotate : MonoBehaviour {

	public float rotateAmount;
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAroundLocal(Vector3.up, rotateAmount);
	}
}
