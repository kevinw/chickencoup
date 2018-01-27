using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class Recruitable : MonoBehaviour {
        public GameObject RecruitmentPrompt;
        bool Activated;
        ControllerButton beginrecruitmentButton;

        void Start()
        {
            Assert.IsNotNull(RecruitmentPrompt);
            Activated = false;
            //figure out how to smartly being recruitment
            //otherwise just assign to a button
            beginrecruitmentButton = ControllerButton.B;
            // RecruitmentPrompt.Setup(beginrecruitmentButton);

            Events.Recruitment.ToggleRecruitmentPrompt += ToggleRecruitmentPrompt;
            Events.Recruitment.TryBeginRecruitment += OnRecruitmentAttempted;
        }

        void ToggleRecruitmentPrompt(GameObject g, Visibility state)
        {
            if(g != this.gameObject){return;}
            switch (state)
            {
                case Visibility.Hidden:
                    RecruitmentPrompt.SetActive(false);
                    Activated = false;
                    break;
                case Visibility.Visible:
                    RecruitmentPrompt.SetActive(true);
                    Activated = true;
                    break;
            }
        }

        void OnRecruitmentAttempted(ControllerButton b)
        {
            if(Activated != true && beginrecruitmentButton != b){return;}
            //activate recruitment
                    
        }
    }
}