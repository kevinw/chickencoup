using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
public class ExtendDolly : MonoBehaviour {

	public Vector3 ExtraPoint;

	void Start () {
		Events.Noise.NoiseLimitReached += OnNoiseLimit;
	}

	void OnNoiseLimit()
	{
		var path = GetComponent<Cinemachine.CinemachineSmoothPath>();
		System.Array.Resize(ref path.m_Waypoints, path.m_Waypoints.Length + 1);
		path.m_Waypoints[path.m_Waypoints.Length - 1].position = ExtraPoint;
		path.m_Waypoints[path.m_Waypoints.Length - 1].roll = 0;
	}
	
}

}