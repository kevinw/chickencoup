using UnityEngine;
using System.Collections;

public class VerletMovement : MonoBehaviour
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
    Vector2 velocityVector;
    // User-intended acceleration
    Vector2 movementVector;
    //our last position
    Vector3 lastPosition;

    void Start()
    {
        //we start out with no movement
        velocityVector = Vector2.zero;

        //we start out where we started
        lastPosition = transform.position;
    }

    void Update()
    {
        //init any movement from the player
        movementVector = Vector2.zero;
        lastPosition = transform.position;

        // set the movement to the inputed keys
        //grab the first half step vel
        movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 accleration = GetAccleration(velocityVector);
        Vector3 halfStepVel = new Vector3 (velocityVector.x + 0.5f * Time.deltaTime * accleration.x, velocityVector.y + 0.5f * Time.deltaTime * accleration.y, 0);
        transform.position = transform.position + halfStepVel * Time.deltaTime;

        //grab the second half-step
        accleration = GetAccleration(new Vector2 (halfStepVel.x, halfStepVel.y));
        velocityVector.x = halfStepVel.x + 0.5f * Time.deltaTime * accleration.x;
        velocityVector.y = halfStepVel.y + 0.5f * Time.deltaTime * accleration.y;
        velocityVector = CutXYZ(velocityVector, stopVelocity);
    }

    //get the accleration of an object based on its velocity
    Vector2 GetAccleration(Vector2 velocity)
    {
        //find the force given
        Vector2 force = movementVector * accleration;

        //find the new force
        force = new Vector2(force.x - (friction * mass * velocity.x), force.y - (friction * mass * velocity.y));

        //divide force by mass to get accleration
        return CutXYZ(new Vector2(force.x / mass, force.y / mass), 0.01f);
    }

    //floor a vector2 if it's below a threshold
    Vector2 CutXYZ(Vector2 input, float threshold)
    {
        float x = Mathf.Abs(input.x) > threshold ? input.x : 0f;
        float y = Mathf.Abs(input.y) > threshold ? input.y : 0f;
        return new Vector2(x,y);
    }

}