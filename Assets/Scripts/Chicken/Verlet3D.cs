using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class Verlet3D : MonoBehaviour
{
    //accleration per update from player movement
    public float accleration = 200.0f;
    //amount of friction on the ground
    public float friction = 10.0f;
    //how much mass the object has (negative numbers mean infinitely dense)
    public float mass = 1.0f;
    //near-zero point at which any velocity is set to zero
    public float stopVelocity = 0.1f;
	public float jumpAmount = 1.0f;
    // Object's velocity in units per second. Derived from acceleration.
    Vector3 velocityVector;
    // User-intended acceleration
    Vector3 movementVector;
    //our last position
    Vector3 lastPosition;

	private Transform cam; // A reference to the main camera in the scenes transform
	bool jumping;

    void Start()
    {
        //we start out with no movement
        velocityVector = Vector3.zero;

        //we start out where we started
        lastPosition = transform.position;
		
		jumping = false;
    }



    void Update()
    {
		//grab all input values from player
		// bool jump = CrossPlatformInputManager.GetButton("Jump");
		// jumping = Input.GetKeyDown(KeyCode.Space);
		jumping = Input.GetKey(KeyCode.JoystickButton1);
		// Debug.Log("0" + Input.GetKey(KeyCode.JoystickButton0));
		// Debug.Log("1" + Input.GetKey(KeyCode.JoystickButton1));
		// Debug.Log("2" + Input.GetKey(KeyCode.JoystickButton2));
		// Debug.Log("3" + Input.GetKey(KeyCode.JoystickButton3));
		// Debug.Log("4" + Input.GetKey(KeyCode.JoystickButton4));
		// Debug.Log("5" + Input.GetKey(KeyCode.JoystickButton5));
		// Debug.Log("6" + Input.GetKey(KeyCode.JoystickButton6));
		// Debug.Log("7" + Input.GetKey(KeyCode.JoystickButton7));
		// Debug.Log("8" + Input.GetKey(KeyCode.JoystickButton8));
		// Debug.Log("9" + Input.GetKey(KeyCode.JoystickButton9));
		// Debug.Log("10" + Input.GetKey(KeyCode.JoystickButton10));
		// Debug.Log("11" + Input.GetKey(KeyCode.JoystickButton11));
		// Debug.Log("12" + Input.GetKey(KeyCode.JoystickButton12));
		// Debug.Log("13" + Input.GetKey(KeyCode.JoystickButton13));
		// Debug.Log("14" + Input.GetKey(KeyCode.JoystickButton14));
		// Debug.Log("15" + Input.GetKey(KeyCode.JoystickButton15));
		// Debug.Log("16" + Input.GetKey(KeyCode.JoystickButton16));
		// Debug.Log("17" + Input.GetKey(KeyCode.JoystickButton17));
		// Debug.Log("18" + Input.GetKey(KeyCode.JoystickButton18));
		// Debug.Log("19" + Input.GetKey(KeyCode.JoystickButton19));
        //init any movement from the player
        movementVector = Vector3.zero;
        lastPosition = transform.position;

        // set the movement to the inputed keys
        //grab the first half step vel
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if(jumping){movementVector.y += jumpAmount;}
        Vector3 accleration = GetAccleration(velocityVector);
        Vector3 halfStepVel = new Vector3 (velocityVector.x + 0.5f * Time.deltaTime * accleration.x, 
										   velocityVector.y + 0.5f * Time.deltaTime * accleration.y, 
										   velocityVector.z + 0.5f * Time.deltaTime * accleration.z);
        transform.position = transform.position + halfStepVel * Time.deltaTime;

        //grab the second half-step
        accleration = GetAccleration(new Vector3 (halfStepVel.x, halfStepVel.y, halfStepVel.z));
        velocityVector.x = halfStepVel.x + 0.5f * Time.deltaTime * accleration.x;
        velocityVector.y = halfStepVel.y + 0.5f * Time.deltaTime * accleration.y;
        velocityVector.z = halfStepVel.z + 0.5f * Time.deltaTime * accleration.z;
        velocityVector = CutXYZ(velocityVector, stopVelocity);
    }

    //get the accleration of an object based on its velocity
    Vector3 GetAccleration(Vector3 velocity)
    {
        //find the force given
        Vector3 force = movementVector * accleration;

        //find the new force
        force = new Vector3(force.x - (friction * mass * velocity.x), 
						    force.y - (friction * mass * velocity.y), 
							force.z - (friction * mass * velocity.z));
        //divide force by mass to get accleration
        return CutXYZ(new Vector3(force.x / mass, force.y / mass, force.z / mass), 0.01f);
    }

    //floor a vector2 if it's below a threshold
    Vector3 CutXYZ(Vector3 input, float threshold)
    {
        float x = Mathf.Abs(input.x) > threshold ? input.x : 0f;
        float y = Mathf.Abs(input.y) > threshold ? input.y : 0f;
        float z = Mathf.Abs(input.z) > threshold ? input.z : 0f;
        return new Vector3(x,y,z);
    }

}