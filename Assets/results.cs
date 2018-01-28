using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class results : MonoBehaviour {

		public List<ChickenProperties> chickenProperties = new List<ChickenProperties>();
		public Text scoreText;
		public Text savedText;
		public float spacing = 0.5f;
		public float chickenWidth = 0.5f;
		public GameObject chickenPrefab;
		public int chickenCount = 20;

		void Awake () {
			int score = 0;
			int index = 0;
			chickenCount = chickenProperties.Count ;
			float baseLength = Mathf.Clamp(chickenCount, 0.0f, 10.0f);
			float leftPosition = (-1 * baseLength / 2.0f * chickenWidth) + (-1 * baseLength/2.0f * spacing);
			foreach (ChickenProperties properties in chickenProperties) {
				score += properties.Points;
				float new_index = index;

				while (new_index > 10.0f) {
					new_index -= 10.0f;
				}

				float z_position = -1 * Mathf.Floor(index / 10.0f) * 2.0f;
				float position = leftPosition + new_index + spacing + (z_position / 4.0f);

				GameObject chicken = Instantiate(chickenPrefab, new Vector3(position, 20.0f, z_position), Quaternion.identity) as GameObject;
				chicken.AddComponent<moveAlongWave>();
				index ++;
			}
			scoreText.text = score.ToString();
		}
	}
}