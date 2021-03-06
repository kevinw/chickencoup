using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FMOD.Studio;

namespace ChickenCoup
{
    public class CoreChicken : MonoBehaviour {
        public GameObject songPrefab;

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

        IEnumerator Start()
        {
            Assert.IsNotNull(BlueSquak);
            Assert.IsNotNull(YellowSquak);
            Assert.IsNotNull(RedSquak);
            DisableAllSquaks();

            MovementScript = GetComponent<Verlet3D>();

            //sub to events
            Events.Input.ButtonPressed += OnButtonPressed;
            Events.Recruitment.BeginRecruitment += StartSong;
            Events.Recruitment.RecruitmentResult += OnRecruitmentResult;

            //link sounds
            walkingSound = FMODUnity.RuntimeManager.CreateInstance(WalkingSoundEvent);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingSound, GetComponent<Transform>(), GetComponent<Rigidbody>());


            yield return null;

        }

        void Update()
        {
            PLAYBACK_STATE walkingState;
            walkingSound.getPlaybackState(out walkingState);
            if(MovementScript.InMotion)
            {
                if(walkingState != PLAYBACK_STATE.PLAYING)
                {
                    walkingSound.start();
                }
            } 
            else
            {
                walkingSound.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }

        public void OnButtonPressed(ControllerButton b)
        {
            //if(!TouchingChicken)
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
            if(TouchingChicken && !ChickenSong.InSong && !GlobalState.NoiseLimitWasReached)
            {
                if (Events.Recruitment.TryBeginRecruitment != null){
                    Events.Recruitment.TryBeginRecruitment(b);
                }
            }
        }

        public Vector3 songPosition = new Vector3(-1.2f, 0, 3.07f);

        public void StartSong(Recruitable recruitable)
        {
            if (ChickenSong.InSong)
            {
                Debug.Log("can't start song, in song");
                return;
            }
            if (songObj)
            {
                Debug.Log("can't start song, already one");
                return;
            }

            songObj = Instantiate(songPrefab, songPosition, Quaternion.identity);
            songObj.transform.SetParent(Camera.main.transform, false);
            songObj.GetComponent<ChickenSong>().GenerateAndPlaySong(recruitable);
        }

        GameObject songObj;

        void OnRecruitmentResult(Recruitable recruitable, bool result)
        {
            Debug.Log("OnRecruitmentResult " + recruitable + " " + result);
            if (songObj)
                LeanTween.scale(songObj, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuint).setOnComplete(() => {
                    Destroy(songObj);
                    songObj = null;
                });

            if (result)
            {
                GetComponent<ChickenLine>().AddFollowingChicken(recruitable);
                recruitable.SetRecruited();
            }

        }

        void PlaySoundHere(string soundID)
        {
            if (!string.IsNullOrEmpty(soundID))
                FMODUnity.RuntimeManager.PlayOneShot(soundID, transform.position);
        }

        void Squak(SquakType t)
        {
            switch (t)
            {
                case SquakType.Blue:
                    BlueSquak.SetActive(true);
                    PlaySoundHere(BlueSquakSound);
                    if(Events.Noise.IncreaseNoise != null){Events.Noise.IncreaseNoise(2);}
                    break;
                case SquakType.Yellow:
                    YellowSquak.SetActive(true);
				    PlaySoundHere(YellowSquakSound);
                    if(Events.Noise.IncreaseNoise != null){Events.Noise.IncreaseNoise(2);}
                    break;
                case SquakType.Red:
                    RedSquak.SetActive(true);
                    PlaySoundHere(RedSquakSound);
                    if(Events.Noise.IncreaseNoise != null){Events.Noise.IncreaseNoise(2);}
                    break;
                case SquakType.Green:
                    //jumps
                    if(Events.Noise.IncreaseNoise != null){Events.Noise.IncreaseNoise(1);}
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
