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
        }

        void OnRecruitmentResult(Recruitable r, bool result)
        {
            if(result)
            {
                totalChickens++;
                Counter.text = totalChickens.ToString();
            }
        }
    }
}
