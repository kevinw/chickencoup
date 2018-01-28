using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour {

	[FMODUnity.EventRef]
	public string MenuMusicEvent;
	FMOD.Studio.EventInstance menuMusic;

	FMOD.Studio.ParameterInstance IntroFinishParameter;

	// Use this for initialization
	void Start () {
		menuMusic = FMODUnity.RuntimeManager.CreateInstance(MenuMusicEvent);
		menuMusic.start();
		menuMusic.getParameter("IntroDone", out IntroFinishParameter);
		IntroFinishParameter.setValue(0);
	}

	void Update() {
		if (Input.anyKeyDown) {
			//menuMusic.stop(STOP_MODE.ALLOWFADEOUT);
			IntroFinishParameter.setValue(1);
			//print (IntroFinishParameter.get);
		}
	}

}
