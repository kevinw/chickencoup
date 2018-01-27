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

            //sub to buttons 
            Events.Input.ButtonPressed += OnButtonPressed;
        }

        public void OnButtonPressed(ControllerButton b)
        {
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

        void Squak(SquakType t)
        {

        }

        void Update () {
        }
    }
}
