using UnityEngine;
using System.Collections;

public class LongStartNote :Note {

	// Use this for initialization
	void Start () {
		//startPoint    = new Vector2(5.0f,12.0f);
	}

	void Update () {
		float time = Music.MusicalTimeFrom (note.timing);
		//Debug.Log ("move!!!");
		if (time <= 0.0f) {
			float pos = time / note.startTime;
			note.obj.transform.localPosition = startPoint * pos;

		}
	}
}
