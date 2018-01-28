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
	[Range(0.0f,1.0f)]
	public float sittingBlend;
	public float sitOffset;
	[Range(0.0f,1.0f)]
	public float flyingBlend;
	public float flyOffset;

	public AnimationSettings () {
		moveY = new AnimationCurve();
		hopHeight = 0.1f;
		hopRate = 2.0f;
		maxVelocity = 0.1f;
		chaos = 0.0f;
		chaosSpeed = 1.0f;
		sittingBlend = 0.0f;
		sitOffset = 0.3f;
		flyingBlend = 0.0f;
	}
}

/* 
public class SittingPose : Pose {
	public Vector3 rootPosition {
		get {
			return new Vector3(0, -1 * Easing.Elastic.InOut(settings.sittingBlend) * settings.sitOffset,0);
		}
	}
	public Vector3 rootScale {
		get {
			float scaleValue = Mathf.Max(0.0f, Easing.Elastic.InOut(settings.sittingBlend) - 1.0f);
			return new Vector3(1.0f + scaleValue, 1.0f - scaleValue, 1.0f + scaleValue);
		}
	}
	public SittingPose(ChickenAnimator _animator, AnimationSettings _settings) : base(_animator, _settings) {}
}
public class FlyingPose : Pose {
	public Vector3 rootPosition {
		get {
			return new Vector3(0, (Easing.Elastic.In(settings.flyingBlend) * settings.flyOffset),0);
		}
	}
	public FlyingPose(ChickenAnimator _animator, AnimationSettings _settings, Quaternion _startRotation) : base(_animator, _settings, _st) {}
}
*/
[System.Serializable]
public class ShadingSettings {

	public Color[] colorSwatches;

}

public class ChickenAnimator : MonoBehaviour {

	public List<GameObject> headJoints = new List<GameObject>();
	public List<GameObject> wingJoints = new List<GameObject>();
	public List<GameObject> legJoints = new List<GameObject>();
	public GameObject rootJoint;

	public Renderer renderer;

	public SpringSettings springSettings;
	public AnimationSettings animationSettings;
	public ShadingSettings shadingSettings;

	private List<Spring> headSprings = new List<Spring>();
	private List<Spring> wingSprings = new List<Spring>();
	private Spring rootSpring;

	private Quaternion rootJointStartRotation;
	private List<Quaternion> neckJointStartRotations = new List<Quaternion>();
	private List<Quaternion> wingJointStartRotations = new List<Quaternion>();
	private List<Quaternion> legJointStartRotations = new List<Quaternion>();

	private Vector3 chaosPoint = Vector3.zero;

	private Vector3 lastPosition;
	[HideInInspector]
	public Vector3 velocity;
	
	private float frame = 0.0f;
	private float hopHeight = 0.0f;
	private float legSpread = 0.0f;

	void Start(){
		
		// Create procedural offsets
		float seed = Random.Range(0,100);

		float colorID = seed / 100.0f * (shadingSettings.colorSwatches.Length - 1);
		int topColorId = Mathf.CeilToInt(colorID);
		int lowerColorId = Mathf.FloorToInt(colorID);
		Color color = Color.Lerp(shadingSettings.colorSwatches[lowerColorId], shadingSettings.colorSwatches[topColorId], colorID - lowerColorId);

		renderer.material.SetColor("_Color", color);

		rootJointStartRotation = rootJoint.transform.localRotation;

		// Create the springs
		rootSpring = new Spring(springSettings, rootJoint.transform.position);
		foreach (GameObject joint in headJoints) {
			headSprings.Add(new Spring(springSettings, joint.transform.position));
			neckJointStartRotations.Add(joint.transform.localRotation);
		}
		foreach (GameObject joint in wingJoints) {
			wingSprings.Add(new Spring(springSettings, joint.transform.position));
			wingJointStartRotations.Add(joint.transform.localRotation);
		}
		foreach (GameObject joint in legJoints) {
			legJointStartRotations.Add(joint.transform.localRotation);
		}

		lastPosition = transform.position;
		velocity = Vector3.zero;		

	}

	void Update(){
		
		// Create new position at bind pose
		Vector3 rootJointPosition = new Vector3();
		Quaternion rootJointRotation = rootJointStartRotation;
		List<Quaternion> neckJointRotations = new List<Quaternion>(neckJointStartRotations);
		List<Quaternion> wingJointRotations = new List<Quaternion>(wingJointStartRotations);
		List<Quaternion> legJointRotations = new List<Quaternion>(legJointStartRotations);

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


		// Apply hop
		frame += Time.deltaTime * animationSettings.hopRate;
		Vector3 velocity2D = new Vector3(velocity.x, 0, velocity.z);
		if (frame >= 1.0f){
			frame = 0.0f;
			hopHeight = Mathf.Clamp01(velocity2D.magnitude / animationSettings.maxVelocity) ;
		}
		rootJointPosition += animationSettings.moveY.Evaluate(frame) * hopHeight * animationSettings.hopHeight * Vector3.up;

		// Rotate legs
		for (int i = 0; i < legJoints.Count; i++) {
			float legRotZ = Mathf.Cos(frame * 6.2f);
			legRotZ = Mathf.Lerp(0.0f, legRotZ, Mathf.Clamp01(velocity2D.magnitude / animationSettings.maxVelocity));
			Vector3 newLegAngles = new Vector3(legRotZ,0.0f,0.0f);
			newLegAngles *= 90;
			Quaternion localLegRotate = Quaternion.Inverse(legJointStartRotations[i]) * Quaternion.Euler(newLegAngles) * legJointStartRotations[i];
			legJointRotations[i] *= localLegRotate;
		}


		// Update springs
		rootSpring.Update(transform.position + chaosPoint);
		for (int i = 0; i < headSprings.Count; i++) {
			headSprings[i].Update(headJoints[i].transform.position + chaosPoint);
		}
		for (int i = 0; i < wingSprings.Count; i++) {
			wingSprings[i].Update(wingJoints[i].transform.position + chaosPoint);
		}
		
		// Rotate root based on spring
		float rotX = (rootSpring.position.z - rootJoint.transform.position.z);
		float rotZ = (rootSpring.position.x - rootJoint.transform.position.x);
		Vector3 newAngles = new Vector3(rotX,0.0f,rotZ);
		newAngles *= 15;
		Quaternion localRotate = Quaternion.Inverse(rootJointStartRotation) * Quaternion.Euler(newAngles) * rootJointStartRotation;
		rootJointRotation *= localRotate;
		
		// Rotate head joints based on spring
		for (int i = 0; i < headSprings.Count; i++) {
			float neckRotY = (headSprings[i].position.x - headJoints[i].transform.position.x);
			float neckRotZ = (headSprings[i].position.z - headJoints[i].transform.position.z);
			Vector3 newNeckAngles = new Vector3(0.0f,neckRotY,neckRotZ);
			newNeckAngles *= 30f;
			Quaternion localNeckRotate = Quaternion.Inverse(neckJointStartRotations[i]) * Quaternion.Euler(newNeckAngles) * neckJointStartRotations[i];
			neckJointRotations[i] *= localNeckRotate;
		}
		
		// Rotate wings based on spring
		for (int i = 0; i < wingSprings.Count; i++) {
			float rotWingY = (wingSprings[i].position.z - wingJoints[i].transform.position.z);
			float rotWingZ = (wingSprings[i].position.y - wingJoints[i].transform.position.y);
			Vector3 newWingAngles = new Vector3(0,rotWingY,rotWingZ);
			newWingAngles *= 30f;
			Quaternion localWingRotate = Quaternion.Inverse(wingJointStartRotations[i]) * Quaternion.Euler(newWingAngles) * wingJointStartRotations[i];
			wingJointRotations[i] *= localWingRotate;
		}

		// Apply new orientations
		rootJoint.transform.localPosition = rootJointPosition;
		rootJoint.transform.localRotation = rootJointRotation;
		
		for (int i = 0; i < neckJointRotations.Count; i++) {
			headJoints[i].transform.localRotation = neckJointRotations[i];
		}
		for (int i = 0; i < wingJointRotations.Count; i++) {
			wingJoints[i].transform.localRotation = wingJointRotations[i];
		} 
		for (int i = 0; i < legJointRotations.Count; i++) {
			legJoints[i].transform.localRotation = legJointRotations[i];
		} 
		
	}

	public void Squawk(){
		if (animationSettings.chaos != 0.0f) {
			headSprings[0].AddForce(chaosPoint / animationSettings.chaos);
		}
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
