using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ChickenCoup
{
	public class ChickenCounter : MonoBehaviour {
        Text Counter;
        int totalChickens;
        void Start()
        {
            Counter = GetComponent<Text>();
            totalChickens = 0;
            Events.Recruitment.RecruitmentResult += OnRecruitmentResult;
            Events.Recruitment.LostChicken += OnChickenLost;
        }

        void OnRecruitmentResult(Recruitable r, bool result)
        {
            if(result)
            {
                totalChickens++;
                Counter.text = totalChickens.ToString();
            }
        }

        void OnChickenLost(Recruitable r)
        {
            totalChickens = totalChickens = 1;
            Counter.text = totalChickens.ToString();
        }
    }
}
