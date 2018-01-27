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

	Recruitable recruitable;

	public void GenerateAndPlaySong(Recruitable recruitable)
	{
		this.recruitable = recruitable;
		var song = GenerateSong(6);
		PlaceEggs(song);
		coro = StartCoroutine(PlaySong(song));
	}

	Coroutine coro;

	void OnDisable()
	{
		if (coro != null)
		{
			StopCoroutine(coro);
		}
	}

	public ChickenAnimator opponent;

	public float DEBUG_TIME;

	IEnumerator PlaySong(Song song, bool practice=true)
	{
		var currentTime = 0f;
		var lastNote = 0;
		var nextNote = song.notes[lastNote];
		yield return null;
		bool finished = false;
		float currentTimeNormalized = 0f;
		while (currentTimeNormalized <= 1f)
		{
			currentTime += Time.deltaTime;
			currentTimeNormalized = currentTime / song.seconds;
			DEBUG_TIME = currentTimeNormalized;
			SetTime(currentTimeNormalized);
			if (currentTimeNormalized > nextNote.time)
			{
				if (!finished)
				{
					var pos = opponent ? opponent.transform.position : transform.position;
					FMODUnity.RuntimeManager.PlayOneShot(SquawkSoundForNote(nextNote.button), pos);
					if (opponent)
						opponent.Squawk();
					if (practice)
						PulseEgg(lastNote);
					else
					{

					}
					lastNote++;
				}

				if (lastNote < song.notes.Count)
				{
					nextNote = song.notes[lastNote];
				}
				else
				{
					finished = true;
				}
			}

			yield return null;
		}

		coro = null;
		if (practice)
			StartCoroutine(PlaySong(song, false));
		else
		{
			if (Events.Recruitment.RecruitmentResult != null)
			{
				var success = true;
				Events.Recruitment.RecruitmentResult.Invoke(recruitable, success);
			}

		}
	}

	[FMODUnity.EventRef] public string squawkA;
	[FMODUnity.EventRef] public string squawkB;
	[FMODUnity.EventRef] public string squawkX;
	[FMODUnity.EventRef] public string squawkY;

	public void PulseEgg(int noteIndex)
	{
		if (eggParent && noteIndex < eggParent.childCount)
		{
			var egg = eggParent.GetChild(noteIndex);
			var obj = egg.gameObject;
			var s = egg.localScale;
			LeanTween.scale(obj, s * 1.4f, 0.08f);
			LeanTween.scale(obj, s, 0.1f).setDelay(0.08f);
		}
	}

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
		ControllerButton.B,
		ControllerButton.X,
		ControllerButton.Y
	};

	string SquawkSoundForNote(ControllerButton button)
	{
		switch (button)
		{
			case ControllerButton.A: return squawkA;
			case ControllerButton.B: return squawkB;
			case ControllerButton.X: return squawkX;
			case ControllerButton.Y: return squawkY;
			default: throw new System.NotImplementedException();
		}
	}

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
			var time = (float)i/(float)numNotes;
			time += 1.0f/numNotes/2.0f;
			song.notes.Add(new Note{time = time, button=button});
		}

		return song;

	}
}

}