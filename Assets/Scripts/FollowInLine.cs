using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public class FollowInLine : MonoBehaviour {
		public float minSpeed = 0.5f;
		public float maxSpeed = 0.5f;
		public Transform following;
		public float FollowDistance = 0.1f;

		Rigidbody rb;

		void OnEnable()
		{
			rb = GetComponent<Rigidbody>();
		}

		Vector3 targetPos;

		void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(targetPos, 0.2f);
		}

		Vector3 GetTargetPos()
		{
			var forward = following.forward;
			var looking = following.GetComponent<LookingDirection>();
			if (looking)
				forward = looking.direction;

			return following.position - forward * FollowDistance;
		}


		void Update () {
			if (!following)
				return;
			
			targetPos = GetTargetPos();

			var delta = (targetPos - transform.position);
			var distance = delta.magnitude;
			if (distance > FollowDistance)
				rb.MovePosition(transform.position + delta.normalized * minSpeed);
			//rb.AddForce(delta.normalized * force, ForceMode.Force);
		}
	}
}