using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongNoteLane : Lane {
	public LongNoteTiming[] longNoteTimings;
	public Timing[] startTimings;
	public Timing[] endTimings;

	public GameObject[] longNoteObjects = new GameObject[3];
	public GameObject[] endNoteObjects = new GameObject[3];

	public LongStartNote[] startLongNotes = new LongStartNote[3];
	public Note[] endLongNotes = new Note[3];
	private List<NoteData> activeLongNotes = new List<NoteData> ();
	public bool nowLong = false;

	void Start(){
		startPoint    = new Vector3(0.0f,0.0f,0.0f);
		offScreenPos = -3.0f;
	}
		
	public int hit(bool isLong){
		if (activeNotes.Count > 0) {
			int n = 1;
			int i = 0;
			if(nowLong){
				n = 0;
				i++;
			}
			if (n == 1) {
				return 1;
			}
			StartCoroutine ("hiteffect", n);

			return n;
		}
		return 1;
	}

	private Timing preStartTiming = new Timing(0,0,0);
	void Update(){
		if (equalTiming (Music.Near, startTimings [nextTimingNum], -16)) {
			if (preStartTiming != startTimings [nextTimingNum]) {
				NoteData tmp = new NoteData ();
				tmp.obj = createNote (longNoteObjects [nextObjectNum]);
				tmp.timing = startTimings [nextTimingNum];
				tmp.startTime = Music.MusicalTimeFrom (startTimings [nextTimingNum]);
				activeNotes.Add (tmp);
				startLongNotes [nextObjectNum].note = tmp;
				startLongNotes [nextObjectNum].startPoint = startPoint;
				preStartTiming = startTimings [nextTimingNum];
				nowLong = true;
			}
		}
		else if (equalTiming (Music.Near, endTimings [nextTimingNum], -16)) {
			NoteData tmp = new NoteData ();
			tmp.obj = createNote (endNoteObjects[nextObjectNum]);
			tmp.timing = endTimings [nextTimingNum];
			tmp.startTime = Music.MusicalTimeFrom (endTimings [nextTimingNum]);
			activeLongNotes.Add (tmp);
			endLongNotes[nextObjectNum].note = tmp;
			endLongNotes [nextObjectNum].startPoint = startPoint;
			nextTimingNum++;
			nextObjectNum = manageObjNum(nextObjectNum,longNoteObjects.Length);
		}

		if (activeLongNotes.Count > 0) {
			if (activeLongNotes [0].obj.transform.position.y <= offScreenPos) {
				if (activeNotes.Count > 0) {
					activeNotes [0].obj.SetActive (false);
					activeNotes.RemoveAt (0);
				}
				activeLongNotes [0].obj.SetActive (false);
				activeLongNotes.RemoveAt (0);
				nowLong = false;
			}
		}
	}
}
