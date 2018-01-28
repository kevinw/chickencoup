using UnityEngine;
using System.Collections;
using FMOD.Studio;

namespace ChickenCoup
{	
	public class Verlet3D : MonoBehaviour
	{

        [FMODUnity.EventRef]
        FMOD.Studio.EventInstance PlayerFootsteps;

		//accleration per update from player movement
		public float accleration = 200.0f;
		//amount of friction on the ground
		public float friction = 10.0f;
		//how much mass the object has (negative numbers mean infinitely dense)
		public float mass = 1.0f;
		//near-zero point at which any velocity is set to zero
		public float stopVelocity = 0.1f;
		[Range(0,5.0f)]
		public float jumpAmount = 1.0f;
		// Object's velocity in units per second. Derived from acceleration.
		Vector3 velocityVector;
		// User-intended acceleration
		Vector3 movementVector;
		//our last position
		Vector3 lastPosition;

		private Transform cam; // A reference to the main camera in the scenes transform
		bool jumping = false;
		public bool InMotion;		

		void Start()
		{            

			//we start out with no movement
			velocityVector = Vector3.zero;

			//we start out where we started
			lastPosition = transform.position;
			
			//sub to button events
			Events.Input.ButtonPressed += OnButtonPressed;

			//init move state
			InMotion = false;
		}

		bool IsPlayerControlled {
			get {
				return PlayerControlled;
			}
		}

		public void OnButtonPressed(ControllerButton b)
		{
			if(IsPlayerControlled && b == ControllerButton.A)
			{
				jumping = true;
			}
		}

		public bool PlayerControlled = true;
		public ControllerInput.JoystickSide joystickSide;


		internal bool CPUMovement = false;
		internal Vector3 CPUMoveVector;


		void FixedUpdate()
		{
            if (!IsPlayerControlled && !CPUMovement)
				return;
			
			if (ChickenSong.InSong)
				return;

			//init any movement from the player
			movementVector = Vector3.zero;
			lastPosition = transform.position;
			float yVal = lastPosition.y - transform.position.y; //for rigidbody contributions

			// set the movement to the inputed keys
			//grab the first half step vel
			if (CPUMovement)
				movementVector = CPUMoveVector;
			else {
				if (ControllerInput.Instance)
				{
					var stick = ControllerInput.Instance.GetStick(joystickSide);
					movementVector = new Vector3(stick.x, yVal/Time.deltaTime, stick.y);
				}
			}
			
			InMotion = false;
			if(Vector3.Magnitude(movementVector) > 0.1){InMotion = true;}
			if(jumping){movementVector.y += jumpAmount;}
			Vector3 accleration = GetAccleration(velocityVector);
			if(accleration.y > 10.0f){accleration.y = 10.0f;}

           

			Vector3 halfStepVel = new Vector3 (velocityVector.x + 0.5f * Time.deltaTime * accleration.x, 
											velocityVector.y + 0.5f * Time.deltaTime * accleration.y, 
											velocityVector.z + 0.5f * Time.deltaTime * accleration.z);
			transform.position = transform.position + halfStepVel * Time.deltaTime;
			if (transform.position.y < 0)
			{
				velocityVector.y = 0;
				var pos = transform.position;
				pos.y = 0;
				transform.position = pos;
			}

			var forward = Vector3.ProjectOnPlane(halfStepVel.normalized, Vector3.up);
			if (forward.sqrMagnitude > 0f)
			{
				transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * ForwardFaceSpeed);
			}

			//grab the second half-step
			accleration = GetAccleration(new Vector3 (halfStepVel.x, halfStepVel.y, halfStepVel.z));
			velocityVector.x = halfStepVel.x + 0.5f * Time.deltaTime * accleration.x;
			velocityVector.y = halfStepVel.y + 0.5f * Time.deltaTime * accleration.y;
			velocityVector.z = halfStepVel.z + 0.5f * Time.deltaTime * accleration.z;
			velocityVector = CutXYZ(velocityVector, stopVelocity);
			jumping = false;
		}

		public float ForwardFaceSpeed = 14f;

		//get the accleration of an object based on its velocity
		Vector3 GetAccleration(Vector3 velocity)
		{
			//find the force given
			Vector3 force = movementVector * accleration;

			//find the new force
			force = new Vector3(force.x - (friction * mass * velocity.x), 
								force.y - (mass * velocity.y), 
								force.z - (friction * mass * velocity.z));
			//divide force by mass to get accleration
			return CutXYZ(new Vector3(force.x / mass, force.y / mass, force.z / mass), 0.01f);
		}

		//clamp accleration vector3 if it's below a threshold
		Vector3 CutXYZ(Vector3 acclearation, float threshold)
		{
			float x = Mathf.Abs(acclearation.x) > threshold ? acclearation.x : 0f;
			float y = Mathf.Abs(acclearation.y) > threshold ? acclearation.y : 0f;
			float z = Mathf.Abs(acclearation.z) > threshold ? acclearation.z : 0f;
			return new Vector3(x,y,z);
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			if (CPUMovement)
				Gizmos.DrawRay(new Ray(transform.position, CPUMoveVector));
		}

	}
}