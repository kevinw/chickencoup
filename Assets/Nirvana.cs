using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup {

public class Nirvana : MonoBehaviour {
	static float AfterTriggerEndGameSeconds = 5.0f;
	static List<Recruitable> saved;
	void ReachNirvana(GameObject obj)
	{
		var enlightenment = obj.GetComponent<RaiseToSky>();
		if (!enlightenment)
		{
			if(obj.GetComponent<Recruitable>() != null)
			{
				saved.Add(obj.GetComponent<Recruitable>());
				if(Events.Recruitment.Nirvana != null){Events.Recruitment.Nirvana(obj.GetComponent<Recruitable>());}
			}
			obj.AddComponent<RaiseToSky>();
			{
				var b = obj.GetComponent<Rigidbody>();
				b.useGravity = false;
			}
		}
	}

	static bool endGameTriggered;

	void Start()
	{
		saved = new List<Recruitable>();
	}

	void EndGame()
	{
		// int totPoints = 0;
		// foreach (Recruitable item in saved)
		// {
		// 	totPoints = totPoints + item.GetComponent<ChickenProperties>().Points;			
		// }
		// GlobalState.TotalPointsWon = totPoints;
		// GlobalState.ChickensSaved = saved.Count;
		UnityEngine.SceneManagement.SceneManager.LoadScene("results");

	}

	void OnTriggerEnter(Collider collider)
	{
		var recruitable = collider.GetComponent<Recruitable>();
		var coreChicken = collider.GetComponent<CoreChicken>();
		if (recruitable || coreChicken)
		{
			ReachNirvana(collider.gameObject);
			if (coreChicken && !endGameTriggered)
			{
				endGameTriggered = true;
				Invoke("EndGame", AfterTriggerEndGameSeconds);
			}
		}
	}
}

}