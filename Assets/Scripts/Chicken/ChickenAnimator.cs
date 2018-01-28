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
public abstract class Pose {
	protected AnimationSettings settings;
	protected ChickenAnimator animator;
	protected Quaternion startRotation;
	public Vector3 rootPosition {
		get {
			return Vector3.zero;
		}
	}
	public Vector3 rootScale {
		get {
			return new Vector3(1,1,1);
		}
	}
	public Vector3 neckRotation {
		get {
			return Vector3.zero;
		}
	}
	public Pose (ChickenAnimator _animator, AnimationSettings _settings, Quaternion _startRotation) {
		settings = _settings;
		animator = animator;
		startRotation = startRotation;
	}
}
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
public class MovingPose : Pose {

	private float frame = 0.0f;
	private float hopHeight = 0.0f;

	public Vector3 rootPosition {
		get {
			// Apply hop
			if (animator != null) {
				frame += Time.deltaTime * settings.hopRate;
				Vector3 velocity2D = new Vector3(animator.velocity.x, 0, animator.velocity.z);
				if (frame >= 1.0f){
					frame = 0.0f;
					hopHeight = Mathf.Clamp01(velocity2D.magnitude / settings.maxVelocity) * -1;
				}
				return new Vector3 (
					0,
					settings.moveY.Evaluate(frame) * hopHeight * settings.hopHeight,
					0
				);
			} else {
				return Vector3.zero;
			}
			
		}
	}
	public MovingPose(ChickenAnimator _animator, AnimationSettings _settings) : base(_animator, _settings) {}
}
public class SpringPose : Pose {

	public SpringPose(ChickenAnimator _animator, AnimationSettings _settings) : base(_animator, _settings) {}
}
*/


public class ChickenAnimator : MonoBehaviour {

	public List<GameObject> headJoints = new List<GameObject>();
	public List<GameObject> wingJoints = new List<GameObject>();
	public GameObject rootJoint;

	public SpringSettings springSettings;
	public AnimationSettings animationSettings;

	private List<Spring> headSprings = new List<Spring>();
	private List<Spring> wingSprings = new List<Spring>();
	private Spring rootSpring;

	private Quaternion rootJointStartRotation;
	private List<Quaternion> neckJointStartRotations = new List<Quaternion>();
	private List<Quaternion> wingJointStartRotations = new List<Quaternion>();

	private Vector3 chaosPoint = Vector3.zero;

	private Vector3 lastPosition;
	[HideInInspector]
	public Vector3 velocity;
	
	private float frame = 0.0f;
	private float hopHeight = 0.0f;

	void Start(){

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

		lastPosition = transform.position;
		velocity = Vector3.zero;		

	}

	void Update(){
		
		// Create new position at bind pose
		Vector3 rootJointPosition = new Vector3();
		Quaternion rootJointRotation = rootJointStartRotation;
		List<Quaternion> neckJointRotations = new List<Quaternion>(neckJointStartRotations);
		List<Quaternion> wingJointRotations = new List<Quaternion>(wingJointStartRotations);

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


		// Update springs
		rootSpring.Update(transform.position + chaosPoint);
		for (int i = 0; i < headSprings.Count; i++) {
			headSprings[i].Update(headJoints[i].transform.position + chaosPoint);
		}
		for (int i = 0; i < wingSprings.Count; i++) {
			wingSprings[i].Update(wingJoints[i].transform.position + chaosPoint);
		}
		
		// Rotate head joints based on spring
		float rotX = (rootSpring.position.z - rootJoint.transform.position.z);
		float rotZ = (rootSpring.position.x - rootJoint.transform.position.x);
		Vector3 newAngles = new Vector3(rotX,0.0f,rotZ);
		newAngles *= 15;
		Quaternion localRotate = Quaternion.Inverse(rootJointStartRotation) * Quaternion.Euler(newAngles) * rootJointStartRotation;
		rootJointRotation *= localRotate;
		
		for (int i = 0; i < headSprings.Count; i++) {
			float neckRotY = (headSprings[i].position.x - headJoints[i].transform.position.x);
			float neckRotZ = (headSprings[i].position.z - headJoints[i].transform.position.z);
			Vector3 newNeckAngles = new Vector3(0.0f,neckRotY,neckRotZ);
			newNeckAngles *= 45f;
			//headJoints[i].transform.localEulerAngles = newAngles;
			Quaternion localNeckRotate = Quaternion.Inverse(neckJointStartRotations[i]) * Quaternion.Euler(newNeckAngles) * neckJointStartRotations[i];
			neckJointRotations[i] *= localNeckRotate;
		}

		
		// Rotate wings based on spring
		for (int i = 0; i < wingSprings.Count; i++) {
			float rotWingY = (wingSprings[i].position.z - wingJoints[i].transform.position.z);
			float rotWingZ = (wingSprings[i].position.y - wingJoints[i].transform.position.y);
			Vector3 newWingAngles = new Vector3(0,rotWingY,rotWingZ);
			newWingAngles *= 45f;
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
