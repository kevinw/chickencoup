using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingDirection : MonoBehaviour {
	Vector3 lastPosition;
	public Vector3 direction;

	void Update () {
		var delta = (transform.position - lastPosition);
		if (delta.magnitude > 0.001f)
		{
			direction = Vector3.ProjectOnPlane(delta.normalized, Vector3.up);
		}
		lastPosition = transform.position;
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Ray(transform.position, direction));
    }
}
