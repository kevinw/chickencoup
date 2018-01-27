using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class Recruitable : MonoBehaviour {
        public GameObject RecruitmentPromptPrefab;
        RecruitmentPrompt spawnedPrompt;
        bool Activated;
        ControllerButton beginrecruitmentButton;
        //assigned recruitment prompt
        GameObject recruitPrompt;

        void Start()
        {
            //spawn a new recruitment prefab
            Assert.IsNotNull(RecruitmentPromptPrefab);
            spawnedPrompt = Instantiate(RecruitmentPromptPrefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<RecruitmentPrompt>();
            //figure out how to smartly being recruitment
            //otherwise just assign to a button
            beginrecruitmentButton = ControllerButton.B;
            spawnedPrompt.Setup(ControllerButton.B);

            Activated = false;
            // RecruitmentPrompt.Setup(beginrecruitmentButton);

            Events.Recruitment.ToggleRecruitmentPrompt += ToggleRecruitmentPrompt;
            Events.Recruitment.TryBeginRecruitment += OnRecruitmentAttempted;
        }

        void ToggleRecruitmentPrompt(GameObject g, Visibility state)
        {
            if(g != this.gameObject){return;}
            switch (state)
            {
                case Visibility.Visible:
                    spawnedPrompt.Activate();
                    Activated = true;
                    break;
                case Visibility.Hidden:
                    spawnedPrompt.Deactivate();
                    Activated = false;
                    break;
            }
        }

        void OnRecruitmentAttempted(ControllerButton b)
        {
            if(Activated != true && beginrecruitmentButton != b){return;}
            Debug.Log("ACTIVATED RECRUITMENT");
            //activate recruitment
                    
        }
    }
}