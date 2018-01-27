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
		internal List<Recruitable> chickensFollowingYou = new List<Recruitable>();

		public void AddFollowingChicken(Recruitable recruitable)
		{
			chickensFollowingYou.Add(recruitable);
		}

		void Update()
		{
			var playerChicken = GameObject.FindGameObjectWithTag("Player");
			var lastChicken = chickensFollowingYou.Count > 0 ? chickensFollowingYou[chickensFollowingYou.Count - 1] : null;
			if (!lastChicken)
				return;
			
			startPos = playerChicken.transform.position;
			endPos = lastChicken.transform.position;

			var toLastChicken = endPos - startPos;

			var numChickens = chickensFollowingYou.Count;
			var delta = toLastChicken / ((float)numChickens + 1.0f);

			for (var i = 0; i < numChickens; ++i)
			{
				var forward = delta * ((float)i + 1f);
				var p = startPos + forward;

				//var right = Vector3.Cross(forward, Vector3.up);

				//var random = Mathf.PerlinNoise((int)(Time.time * 2.0f + i * 1.0f/(float)numChickens), i);
				//var randomVec = right.normalized * random;
				//p += randomVec;

				var otherChicken = chickensFollowingYou[i];
				var follow = otherChicken.GetComponent<VerletFollow>();
				if (!follow)
					follow = otherChicken.gameObject.AddComponent<VerletFollow>();

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
