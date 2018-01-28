using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FMODUnity;
namespace ChickenCoup {

public enum NoteState { None, Hit, Missed };

[System.Serializable]
public class Note
{
	public float time;
	public ControllerButton button;

	public NoteState state = NoteState.None;
	public float missAmount = 0f;

	public override string ToString()
	{
		return "<Note t:" + time + " b:" + button + " s:" + state + ">";
	}
}

[System.Serializable]
public class Song
{
	public List<Note> notes = new List<Note>();
	public float seconds = 3f;
}

[ExecuteInEditMode]
public class ChickenSong : MonoBehaviour {
	string failFX = "event:/SFX/UI/Communication_Fail";
	string successFX1 = "event:/SFX/CHICKEN_RECRUITABLE/Voice_Chicken_Recruit";
	string successFX2 = "event:/SFX/UI/Chicken_Recruit";

	public float StaffLength = 2.0f;
	public float StaffLineSpace = 0.3f;

	public LineRenderer[] lines;
	public GameObject eggprefab;
	public GameObject friedEggPrefab;
	public GameObject chickPrefab;
	public Transform eggParent;
	public LineRenderer timeline;
	public UnityEngine.UI.Text text;

	Recruitable recruitable;

	public void GenerateAndPlaySong(Recruitable recruitable)
	{
		int numberOfNotes = Random.Range(1, 6);
		float seconds = Random.Range(2, 5);
		float restChance = Random.Range(0.05f, 0.15f);

		this.recruitable = recruitable;

		var song = GenerateSong(numberOfNotes, seconds, restChance);

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

	public float DEBUG_TIME;

	public float MissTime = 0.2f;

	public static bool InSong = false;

	IEnumerator PlaySong(Song song, bool practice=true)
	{
		InSong = true;
		var player = GameObject.FindGameObjectWithTag("Player");

		if (text)
			text.text = practice ? "practice" : "sing!";

		yield return new WaitForSeconds(1.0f);

		var currentTime = 0f;
		var nextNoteIndex = 0;
		var nextNote = song.notes[nextNoteIndex];
		var missTimeNormalized = MissTime / song.seconds;
		yield return null;
		bool finished = false;
		float currentTimeNormalized = 0f;
		while (currentTimeNormalized <= 1f)
		{
			currentTime += Time.deltaTime;
			currentTimeNormalized = currentTime / song.seconds;
			DEBUG_TIME = currentTimeNormalized;
			SetTime(currentTimeNormalized);

			if (!finished)
			{
				ControllerButton button;
				if (ControllerInput.AnyButtonPressed(out button))
				{
					if (!practice)
					{
						var prevNote = nextNoteIndex > 0 ? song.notes[nextNoteIndex - 1] : null;
						int testIndex = nextNoteIndex;

						Note note = nextNote;
						if (prevNote != null && prevNote.state == NoteState.None &&
							(nextNote == null ||
								(Mathf.Abs(nextNote.time - currentTimeNormalized) > Mathf.Abs(prevNote.time - currentTimeNormalized))))
						{
							note = prevNote;
							testIndex = nextNoteIndex - 1;
						}


						if (note != null && note.state == NoteState.None)
						{
							var delta = currentTimeNormalized - note.time;
							bool hit = Mathf.Abs(delta) < missTimeNormalized && button == note.button;
							note.state = hit ? NoteState.Hit : NoteState.Missed;
							if (!hit)
								if (text) text.text = "oops!";
							EggHit(testIndex, note, hit);
						}
					}
					else
					{
						var fakeNote = new Note();
						fakeNote.time = currentTimeNormalized;
						fakeNote.button = button;
						var fakeEgg = InstantiateNote(eggprefab, fakeNote, true);
						fakeEgg.transform.SetParent(transform, false);
						LeanTween.scale(fakeEgg, Vector3.zero, 0.5f).setDelay(0.3f).setOnComplete(() => {
							Destroy(fakeEgg);
						});

					}
				}
			}

			if (nextNote != null && currentTimeNormalized > nextNote.time)
			{
				if (!finished)
				{
					var pos = recruitable.transform.position;
					FMODUnity.RuntimeManager.PlayOneShot(SquawkSoundForNote(nextNote.button), pos);
					if (recruitable)
						recruitable.Squawk();
					if (practice)
						PulseEgg(nextNoteIndex);
					nextNoteIndex++;

				}

				if (nextNoteIndex < song.notes.Count)
				{
					nextNote = song.notes[nextNoteIndex];
				}
				else
				{
					nextNote = null;
				}
			}

			if (!practice && nextNoteIndex - 1 >= 0)
			{
				var lastNote = song.notes[nextNoteIndex - 1];
				if (lastNote.state == NoteState.None)
				{
					var delta = Mathf.Abs(currentTimeNormalized - lastNote.time);
					if (delta > missTimeNormalized)
					{
						lastNote.state = NoteState.Missed;
						if (text) text.text = "oops!";
						EggHit(nextNoteIndex - 1, lastNote, false);
					}
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
				var success = song.notes.All(n => n.state == NoteState.Hit);
				if (success) {
					FMODUnity.RuntimeManager.PlayOneShot(successFX1);
					FMODUnity.RuntimeManager.PlayOneShot(successFX2);
				}
				Events.Recruitment.RecruitmentResult.Invoke(recruitable, success);
				var cheats = FindObjectOfType<Cheats>();
				if (cheats)
					cheats.SpawnFollowers(success ? Random.Range(6, 12) : Random.Range(1, 4));
			}

		}

		InSong = false;
	}

	[FMODUnity.EventRef] public string squawkA;
	[FMODUnity.EventRef] public string squawkB;
	[FMODUnity.EventRef] public string squawkX;
	[FMODUnity.EventRef] public string squawkY;

	public void EggHit(int noteIndex, Note note, bool hit)
	{
		if (!eggParent)
		{
			Debug.LogWarning("no egg parent");
			return;
		}

		if (noteIndex >= eggParent.childCount)
		{
			Debug.LogWarning("invalid index " + noteIndex);
			return;
		}

		var egg = eggParent.GetChild(noteIndex);
		egg.GetComponent<SpriteRenderer>().enabled = false;
		var obj = InstantiateNote(hit ? chickPrefab : friedEggPrefab, note);
		obj.transform.SetParent(transform, false);

		if (!hit)
		{
			FMODUnity.RuntimeManager.PlayOneShot(failFX);

		}
	}

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
			var eggObj = InstantiateNote(eggprefab, note, true);
			eggObj.transform.SetParent(eggParent, false);
		}
	}

	GameObject InstantiateNote(GameObject prefab, Note note, bool color=false)
	{
		var index = IndexForButton(note.button);
		var pos = new Vector3(note.time * StaffLength, index * StaffLineSpace, -0.1f);
		var eggObj = Instantiate(prefab, pos, Quaternion.identity);
		if (color)
		{
			var eggSprite = eggObj.GetComponent<SpriteRenderer>();
			eggSprite.color = ColorForButton(note.button);
		}
		return eggObj;
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

	Song GenerateSong(int numNotes, float seconds = 3.0f, float restChance = 0.1f)
	{
		Debug.Log("generate song with " + numNotes);
		var song = new Song();
		song.seconds = seconds;

		for (var i = 0; i < numNotes; ++i)
		{
			var button = (ControllerButton)Random.Range(0, 4);
			var time = (float)i/(float)numNotes;
			time += 1.0f/numNotes/2.0f;
			if (Random.value > restChance || song.notes.Count == 0)
				song.notes.Add(new Note{time = time, button=button});
		}

		return song;

	}
}

}