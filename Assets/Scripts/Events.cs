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
            public static Action<Recruitable> BeginRecruitment;
            public static Action<Recruitable, bool> RecruitmentResult;
            public static Action<Recruitable> KillChicken;
            public static Action<Recruitable> LostChicken;
            public static Action<Recruitable> Nirvana;
        }
        public class Noise
        {
            public static Action<int> IncreaseNoise;
            public static Action NoiseLimitReached;
        }
    }
}