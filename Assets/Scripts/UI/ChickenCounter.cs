using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class ChickenCounter : MonoBehaviour {
        Text Counter;
        public GameObject Player;
        int totalChickens;
        ChickenLine line;
        void Start()
        {
            line = Player.GetComponent<ChickenLine>();
            Assert.IsNotNull(Player);
            Counter = GetComponent<Text>();
            totalChickens = 0;
        }

        void Update()
        {
            Counter.text = line.chickensFollowingYou.Count.ToString();
        }
    }
}
