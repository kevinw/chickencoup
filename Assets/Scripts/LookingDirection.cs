using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingDirection : MonoBehaviour {
	Vector3 lastPosition;
	public Vector3 direction;

	void Update () {
		direction = Vector3.ProjectOnPlane(transform.position - lastPosition.normalized, Vector3.one);
	}
}
