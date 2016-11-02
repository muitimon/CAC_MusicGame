using UnityEngine;using System.Collections;using System.Collections.Generic;public class LongNoteLane : Lane {    public int longLaneNum;    public GameObject gameSystem;    public GameSystem gameSystemScript;	public LongNoteTiming[] longNoteTimings;	public Timing[] startTimings;	public Timing[] endTimings;    public float[] longActiveTimes;    public int longFlagNextTimingNum = 0;	public GameObject[] longNoteObjects;	public GameObject[] endNoteObjects;	public LongStartNote[] startLongNotes;	public Note[] endLongNotes;	private List<NoteData> activeLongNotes = new List<NoteData> ();	public bool nowLong = false;	void Start()
    {
        gameSystem = GameObject.Find("GameSystem");
        gameSystemScript = gameSystem.GetComponent<GameSystem>();        startLongNotes = new LongStartNote[longNoteObjects.Length];        endLongNotes = new Note[endNoteObjects.Length];        for (int i=0; i< startLongNotes.Length;i++)
        {
            startLongNotes[i] = longNoteObjects[i].GetComponent<LongStartNote>();
            endLongNotes[i] = endNoteObjects[i].GetComponent<Note>();
        }        startPoint = updateStartPoint;        offScreenPos = 0.0f;	}			public bool hit(bool isLong){		if (activeNotes.Count > 0) {			int n = 1;			int i = 0;			if(nowLong){				n = 0;				i++;			}			if (n == 1) {				return false;			}			StartCoroutine ("hiteffect", n);			return true;		}		return false;	}	public Timing preStartTiming = new Timing(0,0,0);	void Update(){       // Debug.Log(preStartTiming);
        if (equalTiming(Music.Near, startTimings[nextTimingNum], -1 * highSpeedLevel)) {
            if (preStartTiming != startTimings[nextTimingNum])
            {
                nowLong = true;
                NoteData tmp = new NoteData();
                tmp.obj = createNote(longNoteObjects[nextObjectNum]);
                tmp.timing = startTimings[nextTimingNum];
                tmp.startTime = Music.MusicalTimeFrom(startTimings[nextTimingNum]);
                //GetComponent<TrailRenderer>().time = Music.MusicalTimeFrom(endTimings[nextTimingNum]) - tmp.startTime;
                longNoteObjects[nextObjectNum].GetComponent<TrailRenderer>().time = longActiveTimes[nextTimingNum];
                /*Debug.Log(tmp.startTime);
                Debug.Log(Music.MusicalTimeFrom(endTimings[nextTimingNum]));*/
                activeNotes.Add(tmp);
                startLongNotes[nextObjectNum].note = tmp;
                startLongNotes[nextObjectNum].startPoint = startPoint;
                preStartTiming = startTimings[nextTimingNum];
            }		}
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
            nextTimingNum++;

        }
        if (!Music.IsJustChangedAt(new Timing(0, 0, 0)))
        {

            // if (startTimings[0].Bar!=0&& startTimings[0].Beat != 0&& startTimings[0].Unit != 0) {
            if (Music.IsJustChangedAt(startTimings[longFlagNextTimingNum]))
            {
                gameSystemScript.longFlags[longLaneNum] = true;
            }
        }/* else if (Music.IsJustChangedAt(endTimings[longFlagNextTimingNum]))
            {
            }*/
       // }
        if (activeLongNotes.Count > 0) {			if (activeLongNotes [0].obj.transform.localPosition.z <= offScreenPos) {				if (activeNotes.Count > 0) {					activeNotes [0].obj.SetActive (false);					activeNotes.RemoveAt (0);				}				activeLongNotes [0].obj.SetActive (false);				activeLongNotes.RemoveAt (0);				nowLong = false;
                gameSystemScript.longFlags[longLaneNum] = nowLong;

                longFlagNextTimingNum++;
            }		}	}}