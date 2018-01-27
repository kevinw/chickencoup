using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineField : MonoBehaviour {
	public Transform[] mineSpawnAreas;
	public Transform Ground;
	public GameObject MinePrefab;
	public int NumberOfMines = 50;

	void Awake()
	{
		bool first = true;
		Bounds bounds = new Bounds();

		foreach (var t in mineSpawnAreas)
		{
			var box = t.GetComponent<BoxCollider>();
			if (first)
			{
				bounds = box.bounds;
				first = false;
			}
			else
				bounds.Encapsulate(box.bounds);
		}

		for (int i = 0; i < NumberOfMines; ++i)
		{
			var pt = bounds.center + new Vector3(
				bounds.extents.x * (Random.value - 0.5f * 2.0f),
				0,
				bounds.extents.z * (Random.value - 0.5f * 2.0f));
			if (MinePrefab)
			{
				var mine = Instantiate(MinePrefab, pt, Quaternion.identity);
				mine.transform.parent = transform;
			}

		}
	}
}
