using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypeText : MonoBehaviour {

	Text paragraph;
	public List<string> lines = new List<string>();
	float timer = 0.0f;
	string text;
		
	public float delayTime = 0.1f;
	public float pauseTime = 0.5f;
	public string nextSceneName;

	private string baseText = string.Empty;
	private int lineIndex = 0;

	[FMODUnity.EventRef]
	public string typeSound;

	// Use this for initialization
	void Start () {
		paragraph = gameObject.GetComponent<Text>();

		SetText();
	}

	public void SetText()
	{
		StartCoroutine(SetTextRoutine(lines[lineIndex]));
	}

	public IEnumerator SetTextRoutine(string text)
	{
		// reset the paragraph text
		paragraph.text = string.Empty;
		paragraph.text += baseText;

		// keep local start and end tag variables 
		string startTag = string.Empty;
		string endTag = string.Empty;

		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];

			// check to see if we're starting a tag
			if (c == '<')
			{
				// make sure we don't already have a starting tag
				// don't check for ending tag because we set these variables at the 
				// same time
				if (string.IsNullOrEmpty(startTag))
				{
					// store the current index 
					int currentIndex = i;

					for (int j = currentIndex; j < text.Length; j++)
					{
						// add to our starting tag
						startTag += text[j].ToString();

						// check to see if we're going to end the tag
						if (text[j] == '>')
						{
							// set our current index to the end of the tag
							currentIndex = j;
							// set our letter starting point to the current index (when we continue this will be currentIndex++)
							i = currentIndex;

							// find the end tag that goes with this tag
							for (int k = currentIndex; k < text.Length; k++)
							{
								char next = text[k];

								// check to see if we've reached our end tags start point
								if (next == '<')
									break;

								// if we have not increment currentindex
								currentIndex++;
							}
							break;
						}
					}

					// we start at current index since this is where our ending tag starts
					for (int j = currentIndex; j < text.Length; j++)
					{
						// add to the ending tag
						endTag += text[j].ToString();

						// once the ending tag is finished we break out
						if (text[j] == '>')
						{
							break;
						}
					}
				}
				else
				{
					// go through the text and move past the ending tag
					for (int j = i; j < text.Length; j++)
					{
						if (text[j] == '>')
						{
							// set i = j so we can start at the position of the next letter
							i = j;
							break;
						}
					}
					// we reset our starting and ending tag
					startTag = string.Empty;
					endTag = string.Empty;
				}

				// continue to get the next character in the sequence
				continue;

			}

			paragraph.text += string.Format("{0}{1}{2}", startTag, c, endTag);
			FMODUnity.RuntimeManager.PlayOneShot(typeSound, transform.position);
			yield return new WaitForSeconds(delayTime);
		}

		yield return new WaitForSeconds(pauseTime);
		lineIndex ++;
		if (lineIndex < lines.Count) {
			baseText = paragraph.text + "\r\n";
			SetText();
		} else {
			yield return new WaitForSeconds(pauseTime * 3.0f);
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
