using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[Range(0.0f,1.0f)]
	public float headBend;
	[Range(-1.0f,1.0f)]
	public float headTurn;


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
		headBend = 0.0f;
		headTurn = 0.0f;
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
	public ChickenMode mode = ChickenMode.Random;


	public List<GameObject> headJoints = new List<GameObject>();
	public List<GameObject> wingJoints = new List<GameObject>();
	public List<GameObject> legJoints = new List<GameObject>();
	public GameObject rootJoint;

	public Renderer renderer;

	public GameObject hatMesh;
	public List<GameObject> accessories = new List<GameObject>();

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

	[FMODUnity.EventRef]
	public string HoverSoundEvent;
	FMOD.Studio.EventInstance hoverSound;

	[FMODUnity.EventRef]
	public string JumpSound;
	[FMODUnity.EventRef]
	public string LandSound;

	public enum ChickenMode
	{
		Random,
		Player,
		Farmer
	}

	private Vector3 lastPosition;
	[HideInInspector]
	public Vector3 velocity;
	
	private float frame = 0.0f;
	private float hopHeight = 0.0f;
	private float legSpread = 0.0f;

	private bool hovering = false;

	void Start(){
		
		if (mode == ChickenMode.Farmer) {
			if (hatMesh)
				hatMesh.SetActive(true);
			for (int i = 0; i < accessories.Count; i++) {
				accessories[i].SetActive(false);
			}
		} else if (mode == ChickenMode.Player) {
			if (hatMesh)
				hatMesh.SetActive(false);
			for (int i = 0; i < accessories.Count; i++) {
				accessories[i].SetActive(false);
			}

			Color color = Color.white;
			if (shadingSettings.colorSwatches.Length > 0)
			{
				var one = shadingSettings.colorSwatches[Random.Range(0, shadingSettings.colorSwatches.Length-1)];
				var two = shadingSettings.colorSwatches[Random.Range(0, shadingSettings.colorSwatches.Length-1)];
				color = Color.Lerp(one, two, Random.value);
			}

			float neckOffset = -0.088f;
			foreach (GameObject joint in headJoints) {
				joint.transform.localPosition = new Vector3 (joint.transform.localPosition.x + neckOffset, joint.transform.localPosition.y, joint.transform.localPosition.z);
			}
		} else {
			if (hatMesh)
				hatMesh.SetActive(false);

			// Create procedural offsets
			float seed = Random.Range(0,100);

			Color color = Color.white;
			if (shadingSettings.colorSwatches.Length > 0)
			{
				var one = shadingSettings.colorSwatches[Random.Range(0, shadingSettings.colorSwatches.Length-1)];
				var two = shadingSettings.colorSwatches[Random.Range(0, shadingSettings.colorSwatches.Length-1)];
				color = Color.Lerp(one, two, Random.value);
			}

			// Pick hat
			int hatIndex = Random.Range(0,accessories.Count);
			for (int i = 0; i < accessories.Count; i++) {
				if (i != hatIndex) {
					accessories[i].SetActive(false);
				}
			}

			renderer.material.SetColor("_Color", color);

			// Create neck length
			float neckOffset = (0.25f - (seed/100)) * 0.2f;
			foreach (GameObject joint in headJoints) {
				joint.transform.localPosition = new Vector3 (joint.transform.localPosition.x + neckOffset, joint.transform.localPosition.y, joint.transform.localPosition.z);
			}
		}

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
		Vector3 rootJointScale = new Vector3(1,1,1);
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

		// Sitting Offset
		rootJointPosition += new Vector3(0, -1 * Easing.Elastic.InOut(animationSettings.sittingBlend) * 0.25f,0);
		float scaleValue = Mathf.Max(0.0f, Easing.Elastic.InOut(animationSettings.sittingBlend) - 1.0f) * 1.5f;
		rootJointScale += new Vector3(scaleValue, scaleValue, scaleValue);
		for (int i = 0; i < legJoints.Count; i++) {
			float legRotZ = Mathf.Lerp(0.0f, -90.0f, animationSettings.sittingBlend);
			Vector3 newLegAngles = new Vector3(0.0f,0.0f,legRotZ);
			Quaternion localLegRotate = Quaternion.Inverse(legJointStartRotations[i]) * Quaternion.Euler(newLegAngles) * legJointStartRotations[i];
			legJointRotations[i] *= localLegRotate;
		}

		// Head Bend

		for (int i = 0; i < headJoints.Count; i++) {
			float headRotZ = Mathf.Lerp(0.0f, -25.0f, animationSettings.headBend);
			float headRotX = Mathf.Lerp(-25.0f, 25.0f, animationSettings.headTurn/2 + 0.5f);
			Vector3 newHeadAngles = new Vector3(headRotX,0.0f,headRotZ);
			Quaternion localLegRotate = Quaternion.Inverse(neckJointStartRotations[i]) * Quaternion.Euler(newHeadAngles) * neckJointStartRotations[i];
			neckJointRotations[i] *= localLegRotate;
		}
		
		// Wing Flap
		float heightBlend =  -Mathf.Clamp((transform.position.y), 0.0f, 3.0f)/3.0f;
		for (int i = 0; i < wingJoints.Count; i++) {
			float wingRotY = Mathf.Sin(Time.time * 20.0f) * 30.0f * heightBlend;
			Vector3 newHeadAngles = new Vector3(0.0f,0.0f,wingRotY);
			Quaternion localLegRotate = Quaternion.Inverse(neckJointStartRotations[i]) * Quaternion.Euler(newHeadAngles) * neckJointStartRotations[i];
			wingJointRotations[i] *= localLegRotate;
		}
		 

		// Flying offset
		//float blend = -0.5f - transform.position.y;
		//rootJointPosition += new Vector3(0, (Easing.Elastic.In(blend) * animationSettings.flyOffset),0);
		//scaleValue = (1.0f - Mathf.Min(1.0f, Easing.Elastic.In(blend) + 1.0f)) * 1.5f;
		//rootJointScale += new Vector3(scaleValue, scaleValue, scaleValue);

		// Apply hop
		frame += Time.deltaTime * animationSettings.hopRate;
		Vector3 velocity2D = new Vector3(velocity.x, 0, velocity.z);
		if (frame >= 1.0f){
			frame = 0.0f;
			hopHeight = Mathf.Clamp01(velocity2D.magnitude / animationSettings.maxVelocity) ;
		}
		rootJointPosition += animationSettings.moveY.Evaluate(frame) * hopHeight * animationSettings.hopHeight * Vector3.up * (1 + heightBlend);
		rootJointPosition += Mathf.Sin(frame * 6.2f) * animationSettings.hopHeight * 0.2f * Vector3.up * (heightBlend);

		// Rotate legs
		for (int i = 0; i < legJoints.Count; i++) {
			float legRotZ = Mathf.Cos(frame * 6.2f);
			legRotZ = Mathf.Lerp(0.0f, legRotZ, Mathf.Clamp01(velocity2D.magnitude / animationSettings.maxVelocity));
			Vector3 newLegAngles = new Vector3(legRotZ,0.0f,0.0f);
			newLegAngles *= 90;
			newLegAngles *= 1 + heightBlend;
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
		newAngles *= 15.0f;
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
		rootJoint.transform.localScale = rootJointScale;
		
		for (int i = 0; i < neckJointRotations.Count; i++) {
			headJoints[i].transform.localRotation = neckJointRotations[i];
		}
		for (int i = 0; i < wingJointRotations.Count; i++) {
			wingJoints[i].transform.localRotation = wingJointRotations[i];
		} 
		for (int i = 0; i < legJointRotations.Count; i++) {
			legJoints[i].transform.localRotation = legJointRotations[i];
		} 

		/*
		if (SceneManager.GetActiveScene().buildIndex == 2) {
			if (hovering) {
				if (heightBlend <= 1.0f) {
					FMODUnity.RuntimeManager.PlayOneShot(LandSound, transform.position);
					hoverSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					
					hovering = false;
				}
			} else {
				if (heightBlend >= 1.0f) {
					FMODUnity.RuntimeManager.PlayOneShot(JumpSound, transform.position);
					hoverSound.start();
					hovering = true;
				}
			}
		} */


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
