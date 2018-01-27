using System;

namespace ChickenCoup
{
    public static class Events
    {
        public class Input
        {
            public static Action<ControllerButton> ButtonPressed;
        }
    }
}