using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeWalls : MonoBehaviour {
	public Transform[] wallsToExplode;
	public Transform explodePoint;
	public float explodeRadius = 10f;
	public float explodeForce = 100f;

	public bool ExplodeNextFrame = false;

	void Update()
	{
		if (ExplodeNextFrame)
		{
			ExplodeNextFrame = false;
			DoExplode();
		}
	}

	public void DoExplode()
	{
		foreach (Transform t in wallsToExplode)
		{
			var obj = t.gameObject;
			if (obj.GetComponent<Rigidbody>() == null)
				obj.AddComponent<Rigidbody>();
			var r = obj.GetComponent<Rigidbody>();
			r.AddExplosionForce(explodeForce, explodePoint.transform.position, explodeRadius, 0.5f, ForceMode.Impulse);
		}

	}

}
