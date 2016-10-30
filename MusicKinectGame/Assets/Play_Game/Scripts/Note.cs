using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {

	public NoteData note;
	public Vector2 startPoint;
	// Use this for initialization
	void Start () {
		startPoint    = new Vector2(5.0f,12.0f);
		//move = true;
	}
	
	// Update is called once per frame
	void Update () {
		float time = Music.MusicalTimeFrom (note.timing);
		float pos = time / note.startTime;
		note.obj.transform.localPosition = startPoint * pos;

	}
}
