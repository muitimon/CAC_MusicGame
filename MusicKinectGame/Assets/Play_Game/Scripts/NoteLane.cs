using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteLane : Lane {
	public Timing[] timings;
	public GameObject[] noteObjects = new GameObject[4];//ノーツオブジェクト
	public Note[] notes;

	void Start()
    {
        //gameObject.GetComponent<ParticleSystem>().Stop();
        notes = new Note[noteObjects.Length];
        for (int i=0;i<notes.Length;i++)
        {
            notes[i] = noteObjects[i].GetComponent<Note>();
        }
        startPoint    = updateStartPoint;
        offScreenPos = -10.0f;
	}
	// Update is called once per frame
	void Update () {
		//ノーツ発生処理
		if(equalTiming(Music.Near, timings[nextTimingNum],-1*highSpeedLevel)){
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
			if(activeNotes[0].obj.transform.localPosition.z <= offScreenPos){
				//Debug.Log (activeNotes.Count);
				activeNotes [0].obj.SetActive (false);
				activeNotes.RemoveAt (0);
			}
		}
	}

}
