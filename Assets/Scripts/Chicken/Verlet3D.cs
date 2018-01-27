using UnityEngine;
using System.Collections;

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
    // Object's velocity in units per second. Derived from acceleration.
    Vector3 velocityVector;
    // User-intended acceleration
    Vector3 movementVector;
    //our last position
    Vector3 lastPosition;

    void Start()
    {
        //we start out with no movement
        velocityVector = Vector3.zero;

        //we start out where we started
        lastPosition = transform.position;
    }

    void Update()
    {
        //init any movement from the player
        movementVector = Vector3.zero;
        lastPosition = transform.position;

        // set the movement to the inputed keys
        //grab the first half step vel
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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