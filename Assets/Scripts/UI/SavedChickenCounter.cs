using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class SavedChickenCounter : MonoBehaviour {
        public Text SavedText;
        public Text SavedCount;
        void Start()
        {
            SavedText.enabled = false;
            SavedCount.enabled = false;
            Events.Noise.NoiseLimitReached += OnLimitReached;
            // Events.Recruitment.Nirvana += OnNirvanaReached;
        }

        void OnLimitReached()
        {
            SavedText.enabled = true;
            SavedCount.enabled = true;
        }

        void OnNirvanaReached()
        {
            //increase number of saved chickens
        }

        void Update()
        {
            // Counter.text = line.chickensFollowingYou.Count.ToString();
        }
    }
}
