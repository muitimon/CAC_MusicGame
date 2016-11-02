using UnityEngine;using System.Collections;using System.Collections.Generic;public class LongNoteLane : Lane {    public int longLaneNum;    public GameObject gameSystem;    public GameSystem gameSystemScript;	public LongNoteTiming[] longNoteTimings;	public Timing[] startTimings;	public Timing[] endTimings;    public float[] longActiveTimes;	public GameObject[] longNoteObjects = new GameObject[3];	public GameObject[] endNoteObjects = new GameObject[3];	public LongStartNote[] startLongNotes = new LongStartNote[3];	public Note[] endLongNotes = new Note[3];	private List<NoteData> activeLongNotes = new List<NoteData> ();	public bool nowLong = false;	void Start()
    {
        gameSystem = GameObject.Find("GameSystem");
        gameSystemScript = gameSystem.GetComponent<GameSystem>();
       // gameObject.GetComponent<ParticleSystem>().Stop();        startPoint = updateStartPoint;        offScreenPos = -10.0f;	}			public bool hit(bool isLong){		if (activeNotes.Count > 0) {			int n = 1;			int i = 0;			if(nowLong){				n = 0;				i++;			}			if (n == 1) {				return false;			}			StartCoroutine ("hiteffect", n);			return true;		}		return false;	}	private Timing preStartTiming = new Timing(0,0,0);	void Update(){		if (equalTiming (Music.Near, startTimings [nextTimingNum], -1*highSpeedLevel)) {			if (preStartTiming != startTimings [nextTimingNum]) {				NoteData tmp = new NoteData ();				tmp.obj = createNote (longNoteObjects [nextObjectNum]);				tmp.timing = startTimings [nextTimingNum];				tmp.startTime = Music.MusicalTimeFrom (startTimings [nextTimingNum]);
                //GetComponent<TrailRenderer>().time = Music.MusicalTimeFrom(endTimings[nextTimingNum]) - tmp.startTime;
                longNoteObjects[nextObjectNum].GetComponent<TrailRenderer>().time = longActiveTimes[nextTimingNum];
                /*Debug.Log(tmp.startTime);
                Debug.Log(Music.MusicalTimeFrom(endTimings[nextTimingNum]));*/
                activeNotes.Add (tmp);				startLongNotes [nextObjectNum].note = tmp;				startLongNotes [nextObjectNum].startPoint = startPoint;
                preStartTiming = startTimings [nextTimingNum];				nowLong = true;			}		}
        else if (equalTiming(Music.Near, endTimings[nextTimingNum], -1 * highSpeedLevel))
        {
            NoteData tmp = new NoteData();
            tmp.obj = createNote(endNoteObjects[nextObjectNum]);
            tmp.timing = endTimings[nextTimingNum];
            tmp.startTime = Music.MusicalTimeFrom(endTimings[nextTimingNum]);
            activeLongNotes.Add(tmp);
            endLongNotes[nextObjectNum].note = tmp;
            endLongNotes[nextObjectNum].startPoint = startPoint;
            nextObjectNum = manageObjNum(nextObjectNum, longNoteObjects.Length);
        }

        if (Music.IsJustChangedAt(startTimings[nextTimingNum]))
        {
            gameSystemScript.longFlags[longLaneNum] = true;
        }else if (Music.IsJustChangedAt(endTimings[nextTimingNum]))
        {
            gameSystemScript.longFlags[longLaneNum] = false;
        }
        
        if (activeLongNotes.Count > 0) {			if (activeLongNotes [0].obj.transform.localPosition.z <= offScreenPos) {				if (activeNotes.Count > 0) {					activeNotes [0].obj.SetActive (false);					activeNotes.RemoveAt (0);				}				activeLongNotes [0].obj.SetActive (false);				activeLongNotes.RemoveAt (0);				nowLong = false;
                nextTimingNum++;
            }		}	}}