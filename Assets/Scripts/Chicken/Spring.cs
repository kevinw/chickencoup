using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpringSettings
{
    [Range(0.001f, 1.0f)]
    public float stiffness;
    [Range(0.001f, 5.0f)]
    public float mass;
    [Range(0.0f, 1.0f)]
    public float damping;

    public SpringSettings(float _stiffness, float _mass, float _damping)
    {
        stiffness = _stiffness;
        mass = _mass;
        damping = _damping;
    }
}

public class Force {

    private Vector3 force;
    private float timer;

    public Vector3 position {
        get {
            timer -= Time.deltaTime;
            if (timer <= 0.0f){
                timer = 0.0f;
            }

            return force * timer;
        }
    }

    public Force (Vector3 _force) {
        force = _force;
        timer = 1.0f;
    }


}

public class Spring
{
    private SpringSettings settings;
    public Vector3 position;
    private Vector3 velocity = Vector3.zero;
    private Force force;

    public void Update(Vector3 target_position)
    {
        Vector3 springForce = (settings.stiffness * (target_position - position)) - (settings.damping * velocity);
        Vector3 acceleration = springForce / settings.mass;
        Vector3 current_velocity = acceleration + velocity;
        position = current_velocity + position;
        velocity = current_velocity;

        if (force != null) {
            Vector3 forceOffset = force.position;
            if (position.magnitude <= 0.001) {
                force = null;
            } else {
                position += forceOffset;
            }
        }
    }

    public void AddForce(Vector3 _force) {
        force = new Force(_force);
    }

    public Spring(SpringSettings _settings, Vector3 _startPosition)
    {
        settings = _settings;
        position = _startPosition;
    }
}

