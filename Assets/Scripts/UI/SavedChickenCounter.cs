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
        int savedChickens = 0;
        void Start()
        {
            savedChickens = 0;
            SavedText.enabled = false;
            SavedCount.enabled = false;
            Events.Noise.NoiseLimitReached += OnLimitReached;
            Events.Recruitment.Nirvana += OnNirvanaReached;
        }

        void OnLimitReached()
        {
            SavedText.enabled = true;
            SavedCount.enabled = true;
        }

        void OnNirvanaReached()
        {
            savedChickens++;
            SavedCount.text = savedChickens.ToString();
            //increase number of saved chickens
        }
    }
}
