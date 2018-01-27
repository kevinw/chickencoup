using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoup
{
	public enum ControllerButton
	{
		A,
		B,
		X,
		Y
	}

	public class ControllerInput : MonoBehaviour {
		public static ControllerInput Instance;
		void Awake()
		{
			Instance = this;
		}
		void Start()
		{
			DontDestroyOnLoad(this);
		}

		public enum JoystickSide { Left, Right };

		public Vector2 GetStick(JoystickSide side)
		{
			if (side == JoystickSide.Left)
				return GetLeftStick();
			else
				return GetRightStick();
		}

		public Vector2 GetLeftStick()
		{
			return new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical")
			);
		}

		public Vector2 GetRightStick()
		{
			Vector2 axes = new Vector2(
				Input.GetAxis("Horizontal2"), // j, l
				Input.GetAxis("Vertical2") // i, k
			);

			#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			// right stick is different on OSX
			axes += new Vector2(
				Input.GetAxis("Axis3"),
				-Input.GetAxis("Axis4")
			);
			#else
			axes += new Vector2(
				Input.GetAxis("Axis4"),
				-Input.GetAxis("Axis5")
			);
			#endif
			return axes;
		}

		// Update is called once per frame
		void Update () {
			//may need to update these for different plats, these are osx bindings for xbox
			if(Input.GetKeyDown(KeyCode.JoystickButton1))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.A);}
			}
			if(Input.GetKey(KeyCode.JoystickButton2))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.B);}
			}
			if(Input.GetKey(KeyCode.JoystickButton3))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.X);}
			}
			if(Input.GetKey(KeyCode.JoystickButton4))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.Y);}
			}

			// Debug.Log("0" + Input.GetKey(KeyCode.JoystickButton0));
			// Debug.Log("1" + Input.GetKey(KeyCode.JoystickButton1));
			// Debug.Log("2" + Input.GetKey(KeyCode.JoystickButton2));
			// Debug.Log("3" + Input.GetKey(KeyCode.JoystickButton3));
			// Debug.Log("4" + Input.GetKey(KeyCode.JoystickButton4));
			// Debug.Log("5" + Input.GetKey(KeyCode.JoystickButton5));
			// Debug.Log("6" + Input.GetKey(KeyCode.JoystickButton6));
			// Debug.Log("7" + Input.GetKey(KeyCode.JoystickButton7));
			// Debug.Log("8" + Input.GetKey(KeyCode.JoystickButton8));
			// Debug.Log("9" + Input.GetKey(KeyCode.JoystickButton9));
			// Debug.Log("10" + Input.GetKey(KeyCode.JoystickButton10));
			// Debug.Log("11" + Input.GetKey(KeyCode.JoystickButton11));
			// Debug.Log("12" + Input.GetKey(KeyCode.JoystickButton12));
			// Debug.Log("13" + Input.GetKey(KeyCode.JoystickButton13));
			// Debug.Log("14" + Input.GetKey(KeyCode.JoystickButton14));
			// Debug.Log("15" + Input.GetKey(KeyCode.JoystickButton15));
			// Debug.Log("16" + Input.GetKey(KeyCode.JoystickButton16));
			// Debug.Log("17" + Input.GetKey(KeyCode.JoystickButton17));
			// Debug.Log("18" + Input.GetKey(KeyCode.JoystickButton18));
			// Debug.Log("19" + Input.GetKey(KeyCode.JoystickButton19));
		}
	}
}
