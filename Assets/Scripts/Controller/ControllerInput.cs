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

		// Y YELLOW
// X Blue    // B RED
		// A green

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

		public static bool AnyButtonPressed(out ControllerButton button)
		{
			if (PressedThisFrame(ControllerButton.A)) { button = ControllerButton.A; return true; }
			if (PressedThisFrame(ControllerButton.B)) { button = ControllerButton.B; return true; }
			if (PressedThisFrame(ControllerButton.X)) { button = ControllerButton.X; return true; }
			if (PressedThisFrame(ControllerButton.Y)) { button = ControllerButton.Y; return true; }
			button = ControllerButton.A;
			return false;
		}

		public static bool PressedThisFrame(ControllerButton button)
		{
			switch (button)
			{
				case ControllerButton.A: return Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Space);
				case ControllerButton.B: return Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.H);
				case ControllerButton.X: return Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.F);
				case ControllerButton.Y: return Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.G);

				default: throw new System.NotImplementedException();
			}
		}

		// Update is called once per frame
		void Update () {
			//may need to update these for different plats, these are osx bindings for xbox
			if (PressedThisFrame(ControllerButton.A))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.A);}
			}
			if(PressedThisFrame(ControllerButton.B))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.B);}
			}
			if(PressedThisFrame(ControllerButton.X))
			{
				if(Events.Input.ButtonPressed != null){Events.Input.ButtonPressed(ControllerButton.X);}
			}
			if(PressedThisFrame(ControllerButton.Y))
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
