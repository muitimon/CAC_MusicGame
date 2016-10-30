using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour {

	public NoteData note;
	public Vector3 startPoint= new Vector3 (0.0f, -27.0f, 30.0f);
    public float highSpeedLevel;
    // Use this for initialization
   /* void Start () {
		startPoint = new Vector3 (0.0f, -27.0f, 30.0f);
		//move = true;
	}*/

    // Update is called once per frame
    void Update () {
		float time = Music.MusicalTimeFrom (note.timing)*highSpeedLevel;
		float pos = time / note.startTime;
		note.obj.transform.localPosition = startPoint * pos;

	}
}
