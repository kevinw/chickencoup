using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiSineScale : MonoBehaviour {

    private RectTransform rect;
    float startScale;
    public float frequency = 2.0f;
    public float amplitude = 0.1f;
    public float offset = 0.0f;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
        startScale = rect.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        float value = (Mathf.Sin(Time.time * frequency + offset) * amplitude) + startScale;
        rect.localScale = new Vector3(value, value, value);
	}
}
