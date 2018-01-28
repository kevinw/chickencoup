using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public class MineExplosionRadius : MonoBehaviour {
		List<GameObject> collidingObjects;
		Mine ParentMine;
		public float ExplosiveForce = 100f;
		public float ExplosionRadius = 2f;
		public float ExplosionUpwards = 3f;
		void Start()
		{
			collidingObjects = new List<GameObject>();
		}
		public void Explode()
		{
			StartCoroutine(ExplodeWithDelay());
		}

		void OnTriggerEnter(Collider c)
		{
			Debug.Log("added collider");
			collidingObjects.Add(c.gameObject);	
		}
		void OnTriggerExit(Collider c)
		{
			Debug.Log("removed collider");
			collidingObjects.Remove(c.gameObject);	
		}

		public void SetParentMine(Mine m)
		{
			ParentMine = m;
		}

		IEnumerator ExplodeWithDelay()
		{
			ParentMine.PlayCountdownSound();
			yield return new WaitForSeconds(2.0f);
			ParentMine.PlayExplosionSound();

			//grab all the chickens colliding
			foreach (GameObject g in collidingObjects)
			{
				if(g.GetComponent<Recruitable>() != null)
				{
					//tis a chicken!
					Recruitable chicken = g.GetComponent<Recruitable>();
					if(g.GetComponent<Rigidbody>() != null)
					{
						g.GetComponent<Rigidbody>().AddExplosionForce(ExplosiveForce, transform.position, ExplosionRadius, ExplosionUpwards);
					}
					Events.Recruitment.KillChicken(chicken);
				}
			}
			
			ParentMine.DestroyMine();
		}
	}
}