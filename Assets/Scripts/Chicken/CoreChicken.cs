using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class CoreChicken : MonoBehaviour {

        public GameObject BlueSquak;
        public GameObject YellowSquak;
        public GameObject RedSquak;
        public bool TouchingChicken;

        enum SquakType
        {
            Red,
            Blue,
            Yellow
        }

        void Start()
        {
            Assert.IsNotNull(BlueSquak);
            Assert.IsNotNull(YellowSquak);
            Assert.IsNotNull(RedSquak);
            DisableAllSquaks();

            //sub to buttons 
            Events.Input.ButtonPressed += OnButtonPressed;
        }

        public void OnButtonPressed(ControllerButton b)
        {
            if(!TouchingChicken)
            {
                //let people squak all the time
                switch (b)
                {
                    case ControllerButton.B:
                        Squak(SquakType.Red);
                        break;
                    case ControllerButton.Y:
                        Squak(SquakType.Yellow);
                        break;
                    case ControllerButton.X:
                        Squak(SquakType.Blue);
                        break;
                }
            }
            if(TouchingChicken)
            {
                if(Events.Recruitment.TryBeginRecruitment != null){Events.Recruitment.TryBeginRecruitment(b);}                
            }
        }

        void Squak(SquakType t)
        {
            switch (t)
            {
                case SquakType.Blue:
                    BlueSquak.SetActive(true);
                    break;
                case SquakType.Yellow:
                    YellowSquak.SetActive(true);
                    break;
                case SquakType.Red:
                    RedSquak.SetActive(true);
                    break;
            }
        }

        void DisableAllSquaks()
        {
            BlueSquak.SetActive(false);
            YellowSquak.SetActive(false);
            RedSquak.SetActive(false);
        }

        void OnTriggerEnter(Collider other) {
            //prompt to recruit (random squak to consent)
            GameObject c = other.gameObject;
            if(Events.Recruitment.ToggleRecruitmentPrompt != null){Events.Recruitment.ToggleRecruitmentPrompt(c, Visibility.Visible);}
            TouchingChicken = true;
        }

        void OnTriggerExit(Collider other)
        {
            GameObject c = other.gameObject;
            if(Events.Recruitment.ToggleRecruitmentPrompt != null){Events.Recruitment.ToggleRecruitmentPrompt(c, Visibility.Hidden);}
            TouchingChicken = false;
        }
    }
}
