using System;

public static class Events
{
    public class Input
    {
        public static Action<ControllerButton> ButtonPressed;
    }
}