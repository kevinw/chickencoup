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
		if (Ground == null)
		{
			var planeObj = GameObject.Find("Plane");
			if (planeObj)
				Ground = planeObj.transform;
		}

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

		if (!MinePrefab)
			return;

		for (int i = 0; i < NumberOfMines; ++i)
		{
			var pt = bounds.center + new Vector3(bounds.size.x * Random.value - bounds.extents.x, 0, bounds.size.z * Random.value - bounds.extents.z);

			var mine = Instantiate(MinePrefab);
			mine.transform.SetParent(transform, false);

			var minPos = mine.transform.position;
			minPos.x = pt.x;
			minPos.z = pt.z;
			mine.transform.position = minPos;
		}
	}
}
