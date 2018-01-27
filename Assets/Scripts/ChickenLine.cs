using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLine : MonoBehaviour {

	public void AttachSpringJoints()
	{
		for (var i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i);
			if (i + 1 < transform.childCount)
			{
				var springJoint = child.gameObject.AddComponent<SpringJoint>();
				var nextRigidBody = transform.GetChild(i + 1).GetComponent<Rigidbody>();
				springJoint.connectedBody = nextRigidBody;
			}
		}
	}

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
		SetupFollowing();
	}
}
