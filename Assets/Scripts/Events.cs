using System;
using UnityEngine;

namespace ChickenCoup
{
    public enum Visibility
    {
        Visible,
        Hidden
    }
    public static class Events
    {
        public class Input
        {
            public static Action<ControllerButton> ButtonPressed;
        }
        public class Recruitment
        {
            public static Action<GameObject, Visibility> ToggleRecruitmentPrompt;
            public static Action<ControllerButton> TryBeginRecruitment;
        }
    }
}