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

		public Transform otherChickens;

		void Update()
		{
			var playerChicken = GameObject.FindGameObjectWithTag("Player");
			var lastChicken = GameObject.FindGameObjectWithTag("LastChicken");
			if (!lastChicken)
				return;
			
			startPos = playerChicken.transform.position;
			endPos = lastChicken.transform.position;

			var numChickens = otherChickens.childCount;
			var delta = (endPos - startPos) / ((float)numChickens + 1.0f);

			for (var i = 0; i < numChickens; ++i)
			{
				var forward = delta * ((float)i + 1f);
				var p = startPos + forward;

				var right = Vector3.Cross(forward, Vector3.up);

				//var random = Mathf.PerlinNoise((int)(Time.time * 2.0f + i * 1.0f/(float)numChickens), i);
				//randomVec = right.normalized * random;
				//p += randomVec;

				var otherChicken = otherChickens.GetChild(i);
				var follow = otherChicken.GetComponent<VerletFollow>();
				if (!follow)
					follow = otherChicken.gameObject.AddComponent<VerletFollow>();
				follow.FollowPos(p);
			}
			
		}

		public Vector3 randomVec;
	}
}
