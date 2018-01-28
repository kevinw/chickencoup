using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMenu : MonoBehaviour {

	public List<Renderer> renderers = new List<Renderer>();
	public Color baseColor;
	public float amplitude;
	public float frequency;
	Vector3 startPosition;
	public float distance = 0.1f;
	float rand1;
	float rand2;
	float randRot;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		rand1 = Random.value;
		rand2 = Random.value;
		randRot = Random.Range(-135.0f,-225.0f);
		transform.localEulerAngles = new Vector3(0.0f,randRot,0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		transform.localEulerAngles = new Vector3(0.0f,randRot,0.0f);
		
		foreach (Renderer renderer in renderers) {
			float cameraDistance = (Vector3.Distance(transform.position, Camera.main.transform.position) * distance);
			Color newColor = baseColor - new Color(cameraDistance, cameraDistance, cameraDistance);
			renderer.material.SetColor("_Color", newColor);
		}
		

		transform.position = startPosition + new Vector3(Mathf.Sin(amplitude * frequency * Time.time + rand1), 0.0f, amplitude * Mathf.Sin(frequency * Time.time + rand2));
			
	}
}
