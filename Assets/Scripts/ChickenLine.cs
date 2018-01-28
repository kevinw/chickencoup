using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public class ChickenLine : MonoBehaviour {

		void SetupFollowing()
		{
			// each chicken follows the next in line in the hiearchy window
			for (var i = 0; i < transform.childCount; ++i)
			{
				var child = transform.GetChild(i);
				var soldier = child.GetComponent<FollowInLine>();
				if (soldier && i - 1 > -1)
					soldier.following = transform.GetChild(i - 1);
			}
		}

		void Start () {
			// AttachSpringJoints();
			//SetupFollowing();
		}

		Vector3 startPos;
		Vector3 endPos;

		[System.NonSerialized]
		public List<Recruitable> chickensFollowingYou = new List<Recruitable>();

		public void AddFollowingChicken(Recruitable recruitable)
		{
			recruitable.GetComponent<Rigidbody>().isKinematic = false;
			chickensFollowingYou.Add(recruitable);
		}

		public GameObject featherExplosion;

		public void KillChicken(Recruitable recruitable)
		{
			recruitable.DidDie();

			var follow = recruitable.GetComponent<VerletFollow>();
			if (follow)
				follow.enabled = false;
			var v = recruitable.GetComponent<Verlet3D>();
			if (v) {
				v.enabled = false;
			}

			if (featherExplosion)
				Instantiate(featherExplosion, recruitable.transform.position, Quaternion.identity);

			chickensFollowingYou.Remove(recruitable);
		}

		public float FollowDistance = 1.0f;

		void Update()
		{
			var playerChicken = GameObject.FindGameObjectWithTag("Player");
			if (chickensFollowingYou.Count == 0)
				return;

			var lastChicken = chickensFollowingYou[chickensFollowingYou.Count - 1];
			
			startPos = playerChicken.transform.position;
			endPos = lastChicken.transform.position;

			var toLastChicken = endPos - startPos;
			var numChickens = chickensFollowingYou.Count;
			var delta = toLastChicken / ((float)numChickens + 1.0f);

			for (var i = 0; i < numChickens; ++i)
			{
				/* 
				// OLD - linear
				var forward = delta * ((float)i + 1f);
				var p = startPos + forward;
				*/

				// NEW: chickens follow you
				var p = FollowInLine.GetTargetPos(i == 0 ? playerChicken.transform : chickensFollowingYou[i - 1].transform, FollowDistance);

				//var right = Vector3.Cross(forward, Vector3.up);
				//var random = Mathf.PerlinNoise((int)(Time.time * 2.0f + i * 1.0f/(float)numChickens), i);
				//var randomVec = right.normalized * random;
				//p += randomVec;

				var otherChicken = chickensFollowingYou[i];
				var follow = otherChicken.GetComponent<VerletFollow>();
				if (!follow)
				{
					Debug.Log("Adding follow " + otherChicken);
					follow = otherChicken.gameObject.AddComponent<VerletFollow>();
				}

				var v = otherChicken.GetComponent<Verlet3D>();

				if (i == numChickens - 1)
				{
					follow.Follow = false;
					v.CPUMovement = false;
					v.PlayerControlled = true;
					v.joystickSide = ControllerInput.JoystickSide.Right;
				}
				else
				{
					v.PlayerControlled = false;
					follow.FollowPos(p);
				}
			}
			
		}
	}
}
