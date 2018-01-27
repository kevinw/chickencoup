using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenCoup {


[System.Serializable]
public class Note
{
	public float time;
	public ControllerButton button;
}

[System.Serializable]
public class Song
{
	public List<Note> notes = new List<Note>();
	public float seconds = 4f;
}

[ExecuteInEditMode]
public class ChickenSong : MonoBehaviour {

	public float StaffLength = 2.0f;
	public float StaffLineSpace = 0.3f;

	public LineRenderer[] lines;
	public GameObject eggprefab;
	public Transform eggParent;
	public LineRenderer timeline;

	void Start()
	{
		squawkA = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/CHICKEN_RECRUITABLE/Voice_Chicken_Recruit");

		if (Application.isPlaying)
		{
			var song = GenerateSong(6);
			PlaceEggs(song);
			StartCoroutine(PlaySongPractice(song));
		}

	}

	public ChickenAnimator opponent;

	public float DEBUG_TIME;

	IEnumerator PlaySongPractice(Song song)
	{
		var currentTime = 0f;
		var lastNote = 0;
		var nextNote = song.notes[lastNote];
		yield return null;
		while (true)
		{
			currentTime += Time.deltaTime;
			var currentTimeNormalized = currentTime / song.seconds;
			DEBUG_TIME = currentTimeNormalized;
			SetTime(currentTimeNormalized);
			if (currentTimeNormalized > nextNote.time)
			{
				Debug.Log("note " + nextNote.button + " at time " + nextNote.time);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/CHICKEN_RECRUITABLE/Voice_Chicken_Recruit");

				opponent.Squawk();
				lastNote++;
				if (lastNote < song.notes.Count)
				{
					nextNote = song.notes[lastNote];
				}
			}

			yield return null;
		}
	}

	[FMODUnity.EventRef] FMOD.Studio.EventInstance squawkA;
	[FMODUnity.EventRef] FMOD.Studio.EventInstance squawkB;
	[FMODUnity.EventRef] FMOD.Studio.EventInstance squawkX;
	[FMODUnity.EventRef] FMOD.Studio.EventInstance squawkY;

	public void SetTime(float t)
	{
		var pos = timeline.transform.localPosition;
		pos.x = t * StaffLength;
		timeline.transform.localPosition = pos;
	}

	static Color ColorForButton(ControllerButton button)
	{
		switch (button)
		{
			case ControllerButton.A: return Color.green;
			case ControllerButton.B: return Color.red;
			case ControllerButton.X: return Color.blue;
			case ControllerButton.Y: return Color.yellow;
			default: throw new System.NotImplementedException();
		}

	}

	public static ControllerButton[] ButtonIndexes = new [] {
		ControllerButton.A,
		ControllerButton.X,
		ControllerButton.B,
		ControllerButton.Y
	};

	static int IndexForButton(ControllerButton button)
	{
		return System.Array.IndexOf(ButtonIndexes, button);
	}

	void PlaceEggs(Song song)
	{
		var notes = song.notes;
		foreach (var note in notes)
		{
			var index = IndexForButton(note.button);
			var pos = new Vector3(note.time * StaffLength, index * StaffLineSpace, -0.1f);
			var eggObj = Instantiate(eggprefab, pos, Quaternion.identity);
			eggObj.transform.SetParent(eggParent, false);
			var eggSprite = eggObj.GetComponent<SpriteRenderer>();
			eggSprite.color = ColorForButton(note.button);

		}
	}

	void Update () {
		if (!Application.isPlaying)
		{
			for (var i = 0; i < lines.Length; ++i)
			{
				var line = lines[i];
				line.SetPosition(1, new Vector3(StaffLength, 0, 0));
				var pos = line.transform.localPosition;
				pos.y = StaffLineSpace * (float)i;
				line.transform.localPosition = pos;
			}
		}
	}

	Song GenerateSong(int numNotes)
	{
		var song = new Song();
		for (var i = 0; i < numNotes; ++i)
		{
			var button = (ControllerButton)Random.Range(0, 4);
			song.notes.Add(new Note{time = (float)i/(float)numNotes, button=button});
		}

		return song;

	}
}

}