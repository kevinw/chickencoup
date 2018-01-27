using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FMOD.Studio;

namespace ChickenCoup
{
    public class CoreChicken : MonoBehaviour {

        public GameObject BlueSquak;
        public GameObject YellowSquak;
        public GameObject RedSquak;
        public bool TouchingChicken;

        Verlet3D MovementScript;

#region  SOUND

        [FMODUnity.EventRef]
        public string RedSquakSound;
        [FMODUnity.EventRef]
        public string BlueSquakSound;
        [FMODUnity.EventRef]
        public string YellowSquakSound;
        [FMODUnity.EventRef]
        public string GreenSquakSound;
        [FMODUnity.EventRef]
        public string WalkingSoundEvent;
        FMOD.Studio.EventInstance walkingSound;
#endregion

        enum SquakType
        {
            Green,
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

            MovementScript = GetComponent<Verlet3D>();

            //sub to buttons 
            Events.Input.ButtonPressed += OnButtonPressed;

            //link sounds
            walkingSound = FMODUnity.RuntimeManager.CreateInstance(WalkingSoundEvent);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        }

        void Update()
        {
            PLAYBACK_STATE walkingState;
            walkingSound.getPlaybackState(out walkingState);
            if(MovementScript.InMotion)
            {
                if(walkingState != PLAYBACK_STATE.PLAYING)
                {
                   Debug.Log("started walking sound");
                    walkingSound.start();
                }
            } 
            else
            {
                walkingSound.stop(STOP_MODE.IMMEDIATE);
            }
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
                    case ControllerButton.A:
                        Squak(SquakType.Green);
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
                    FMODUnity.RuntimeManager.PlayOneShot(BlueSquakSound, transform.position);
                    break;
                case SquakType.Yellow:
                    YellowSquak.SetActive(true);
                    FMODUnity.RuntimeManager.PlayOneShot(YellowSquakSound, transform.position);
                    break;
                case SquakType.Red:
                    RedSquak.SetActive(true);
                    FMODUnity.RuntimeManager.PlayOneShot(RedSquakSound, transform.position);
                    break;
                case SquakType.Green:
                    //jumps
                    FMODUnity.RuntimeManager.PlayOneShot(GreenSquakSound, transform.position);
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
