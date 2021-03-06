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
        ChickenAnimator chickenAnimator;
        public bool Recruited { get { return _recruited; } }
        bool _recruited;

        public MeshRenderer cookedChickenRenderer;
        bool didDie;

        public void DidDie()
        {
            if (didDie)
                return;
            didDie = true;
            if (cookedChickenRenderer)
                cookedChickenRenderer.enabled = true;
            if (chickenAnimator)
            {
                if(Events.Recruitment.LostChicken != null){Events.Recruitment.LostChicken(this);}
                chickenAnimator.gameObject.SetActive(false);
            }
        }

        public void Squawk()
        {
            if (chickenAnimator)
                chickenAnimator.Squawk();
        }

        void Start()
        {
            chickenAnimator = GetComponentInChildren<ChickenAnimator>();

            //spawn a new recruitment prefab
            Assert.IsNotNull(RecruitmentPromptPrefab);
            spawnedPrompt = Instantiate(RecruitmentPromptPrefab, this.transform.position, Quaternion.identity, this.transform).GetComponent<RecruitmentPrompt>();
            List<ControllerButton> buttons = new List<ControllerButton>()
            {
                ControllerButton.B,
                ControllerButton.X,
                ControllerButton.Y
            };
            beginrecruitmentButton = buttons[Random.Range(0,buttons.Count)];
            spawnedPrompt.Setup(beginrecruitmentButton);

            Activated = false;
            // RecruitmentPrompt.Setup(beginrecruitmentButton);

        }

        void OnEnable()
        {
            Events.Recruitment.ToggleRecruitmentPrompt += ToggleRecruitmentPrompt;
            Events.Recruitment.TryBeginRecruitment += OnRecruitmentAttempted;
        }

        void OnDisable()
        {
            Events.Recruitment.ToggleRecruitmentPrompt -= ToggleRecruitmentPrompt;
            Events.Recruitment.TryBeginRecruitment -= OnRecruitmentAttempted;
        }

        void ToggleRecruitmentPrompt(GameObject g, Visibility state)
        {
            if(g != this.gameObject){return;}
            if (attempted || Recruited || didDie)
            {
                spawnedPrompt.Deactivate();
                Activated = false;
                return;
            }
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

        bool attempted = false;

        void OnRecruitmentAttempted(ControllerButton b)
        {
            if (attempted)
                return;

            if (Activated && beginrecruitmentButton == b)
            {
                attempted = true;
                if (Events.Recruitment.BeginRecruitment != null)
                {
                    Events.Recruitment.BeginRecruitment.Invoke(this);
                    var lookAtPlayer = GetComponent<LookAtPlayer>();
                    if (lookAtPlayer)
                        lookAtPlayer.enabled = false;
                }
            }
        }

        public void SetRecruited()
        {
            _recruited = true;
        }


    }
}