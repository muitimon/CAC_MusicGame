using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteLane : Lane {
	public Timing[] timings;
	public GameObject[] noteObjects = new GameObject[4];//ノーツオブジェクト
	public Note[] notes = new Note[4];
	void Start(){
		startPoint    = new Vector3 (0.0f,-27.0f,60.0f);
        offScreenPos = 0.0f;
	}
	// Update is called once per frame
	void Update () {
		//ノーツ発生処理
		if(equalTiming(Music.Near, timings[nextTimingNum],-16)){
			NoteData tmp = new NoteData();
			tmp.obj = createNote(noteObjects[nextObjectNum]);
			tmp.timing = timings[nextTimingNum];
			tmp.startTime = Music.MusicalTimeFrom (timings [nextTimingNum]);
//			Debug.Log (activeNotes.Count);
			activeNotes.Add(tmp);
			notes [nextObjectNum].note = tmp;
			notes [nextObjectNum].startPoint = startPoint;
			//noteObjects [nextObjectNum].SendMessage ("");
			nextTimingNum++;
			nextObjectNum = manageObjNum(nextObjectNum,noteObjects.Length);
		}
		//アクティブノーツの移動処理
		//画面外に出たノーツを消去処理
		if(activeNotes.Count>0){
			if(activeNotes[0].obj.transform.localPosition.y >= offScreenPos){
				//Debug.Log (activeNotes.Count);
				activeNotes [0].obj.SetActive (false);
				activeNotes.RemoveAt (0);
			}
		}
	}

}
