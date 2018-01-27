using UnityEngine;

namespace ChickenCoup {
public class VerletFollow : MonoBehaviour
{
	public Vector3 targetPos;
	public bool Follow = false;
	public float deadZone = 0.3f; // when a chicken thinks its close enough to the target to stop
	Verlet3D verlet;

	void Awake()
	{
		verlet = GetComponent<Verlet3D>();
	}

	public void FollowPos(Vector3 pos)
	{
		targetPos = pos;
		Follow = true;
	}

	void LateUpdate()
	{
		if (!Follow)
		{
			verlet.CPUMovement = false;
			return;
		}
		
		var delta = targetPos - transform.position;
		
		verlet.CPUMovement = true;
		verlet.CPUMoveVector = delta.magnitude > deadZone ? delta.normalized : Vector3.zero;
	}
}
}