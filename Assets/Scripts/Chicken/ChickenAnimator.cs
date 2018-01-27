using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationSettings {
	public AnimationCurve moveY;
	public float hopHeight;
	public float hopRate;
	public float maxVelocity;
	public float chaos;
	public float chaosSpeed;

	public AnimationSettings () {
		moveY = new AnimationCurve();
		hopHeight = 0.1f;
		hopRate = 2.0f;
		maxVelocity = 0.1f;
		chaos = 0.0f;
		chaosSpeed = 1.0f;
	}
}

public class ChickenAnimator : MonoBehaviour {


	public List<GameObject> headJoints = new List<GameObject>();
	public List<GameObject> wingJoints = new List<GameObject>();
	public GameObject rootJoint;

	public SpringSettings springSettings;
	public AnimationSettings animationSettings;

	private List<Spring> headSprings = new List<Spring>();
	private List<Spring> wingSprings = new List<Spring>();
	private Spring rootSpring;

	private Vector3 chaosPoint = Vector3.zero;

	private Vector3 lastPosition;
	private Vector3 velocity;

	private float frame = 0.0f;
	private float hopHeight = 0.0f;

	void Start(){

		// Create the springs
		rootSpring = new Spring(springSettings, rootJoint.transform.position);
		foreach (GameObject joint in headJoints) {
			headSprings.Add(new Spring(springSettings, joint.transform.position));
		}
		foreach (GameObject joint in wingJoints) {
			wingSprings.Add(new Spring(springSettings, joint.transform.position));
		}

		lastPosition = transform.position;
		velocity = Vector3.zero;		

	}

	void Update(){
		
		// Update velocity
		velocity = transform.position - lastPosition;
		lastPosition = transform.position;

		// Add noise to head
		chaosPoint = new Vector3(
			Mathf.PerlinNoise(Time.time * animationSettings.chaosSpeed, 0.0f) - 0.5f,
			Mathf.PerlinNoise(Time.time * animationSettings.chaosSpeed, 302.3f) - 0.5f,
			Mathf.PerlinNoise(Time.time * animationSettings.chaosSpeed, 54.4f) - 0.5f
		);
		chaosPoint *= animationSettings.chaos;

		// Update springs
		rootSpring.Update(transform.position + chaosPoint);
		//Vector3 target = rootSpring.position;
		for (int i = 0; i < headSprings.Count; i++) {
			headSprings[i].Update(headJoints[i].transform.position + chaosPoint);
		}

		// Rotate head joints based on spring
		rotateToSpring(rootJoint, rootSpring, 15.0f);
		for (int i = 0; i < headSprings.Count; i++) {
			rotateToSpring(headJoints[i], headSprings[i]);
		}

		// Rotate wings based on spring
		for (int i = 0; i < wingSprings.Count; i++) {
			float rotZ = (wingSprings[i].position.y - wingJoints[i].transform.position.y);
			Vector3 newAngles = new Vector3(0,0,(rotZ * 45.0f) + 90);
			wingJoints[i].transform.localEulerAngles = newAngles;
			wingSprings[i].Update(wingJoints[i].transform.position);
		}
		
		// Apply hop
		frame += Time.deltaTime * animationSettings.hopRate;
		Vector3 velocity2D = new Vector3(velocity.x, 0, velocity.z);
		if (frame >= 1.0f){
			frame = 0.0f;
			hopHeight = Mathf.Clamp01(velocity2D.magnitude / animationSettings.maxVelocity) * -1;
		}
		rootJoint.transform.localPosition = new Vector3 (
			animationSettings.moveY.Evaluate(frame) * hopHeight * animationSettings.hopHeight,
			rootJoint.transform.localPosition.y,
			rootJoint.transform.localPosition.z
		);	

	}

	public void Squawk(){
		if (animationSettings.chaos != 0.0f) {
			headSprings[0].AddForce(chaosPoint / animationSettings.chaos);
		}
	}

	void rotateToSpring(GameObject target, Spring spring, float amplitude=45.0f, float maxRotation=45.0f) {

		float rotX = (spring.position.z - target.transform.position.z);
		float rotZ = (spring.position.x - target.transform.position.x);

		Vector3 newAngles = new Vector3(0,rotX,rotZ);
		newAngles *= amplitude;

		target.transform.localEulerAngles = newAngles;

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (rootSpring != null) {
			Gizmos.DrawSphere(rootSpring.position, 0.01f);
			foreach (Spring spring in headSprings) {
				Gizmos.DrawSphere(spring.position, 0.01f);
			}
			foreach (Spring spring in wingSprings) {
				Gizmos.DrawSphere(spring.position, 0.01f);
			}
		}
	}

}
