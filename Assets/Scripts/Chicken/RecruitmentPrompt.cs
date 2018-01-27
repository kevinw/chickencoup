using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class RecruitmentPrompt : MonoBehaviour {
        public GameObject BluePrompt;
        public GameObject RedPrompt;
        public GameObject YellowPrompt;
        GameObject assignedPrompt;

        void Start()
        {
            Assert.IsNotNull(BluePrompt);
            Assert.IsNotNull(RedPrompt);
            Assert.IsNotNull(YellowPrompt);
            BluePrompt.SetActive(false);
            YellowPrompt.SetActive(false);
            RedPrompt.SetActive(false);
        }
        public void Setup(ControllerButton b)
        {  
            switch (b)
            {
                case ControllerButton.B:
                    assignedPrompt = RedPrompt;
                    break;
                case ControllerButton.X:
                    assignedPrompt = BluePrompt;
                    break;
                case ControllerButton.Y:
                    assignedPrompt = YellowPrompt;
                    break;
            }
        }

        public void Activate()
        {
            assignedPrompt.SetActive(true);
        }

        public void Deactivate()
        {
            assignedPrompt.SetActive(false);
        }
    }
}
