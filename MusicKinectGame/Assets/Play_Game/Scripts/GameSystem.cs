using UnityEngine;
using System;//追加.
using System.Collections;
using System.Collections.Generic;//追加.

[Serializable]
public class MusicSetting
{/*譜面の名前やBPMなどの譜面設定情報*/
	public string name;
	public int maxBlock;
	public int BPM;
	public int offset;
	public NoteInformation[] notes;
}
[Serializable]
public class NoteInformation
{/*ノーツ一つ一つの情報*/
	public int LPB;
	public int num;
	public int block;
	public int type;
	public NoteInformation[] notes;
}
public class LongNoteTiming{
	public Timing startTiming;
	public Timing endTiming;

    public float longActiveTime;
	/*LongNoteTiming(Timing startTiming, Timing endTiming){
		this.startTiming = startTiming;
		this.endTiming = endTiming;
	}*/
}

[Serializable]
public class GameSystem : MonoBehaviour {
    public static int score = 0;
	public string loadJsonFileName;
	public NoteLane[] noteLane = new NoteLane[11];
	public LongNoteLane[] longNoteLane = new LongNoteLane[8];
    public int highSpeedLevel = 28;
	public Vector3 startPoint;    public bool[] longFlags;

    void LoadJson(string fileName){
		var textAsset =  Resources.Load ("kanki_Heaven_Hard") as TextAsset;
		var jsonText = textAsset.text;
		MusicSetting item = JsonUtility.FromJson<MusicSetting>(jsonText);

		List<Timing>[] timingList = new List<Timing>[item.maxBlock];
		//List<LongNoteTiming>[] longNoteTimingList = new List<LongNoteTiming>[item.maxBlock-3];
		List<Timing>[] longNoteStartTimingList = new List<Timing>[item.maxBlock-3];
		List<Timing>[] longNoteEndTimingList = new List<Timing>[item.maxBlock-3];
        List<float>[] longActiveTimeList = new List<float>[item.maxBlock - 3];
        longFlags = new bool[item.maxBlock - 3];
		for(int i = 0;i<timingList.Length;i++){
			//リストのリストになっている
			timingList [i] = new List<Timing> ();
		}
		for(int i = 0;i<longNoteStartTimingList.Length;i++){
			longNoteStartTimingList[i] = new List<Timing>();
		}
		for(int i = 0;i<longNoteEndTimingList.Length;i++){
			longNoteEndTimingList[i] = new List<Timing>();
		}
        for (int i = 0;i<longActiveTimeList.Length;i++)
        {
            longActiveTimeList[i] = new List<float>();
        }
		foreach (NoteInformation note in item.notes) {
			int type = note.type;
			if(type == 1){
					Timing tmp = LoadTiming (note.num+32);
					timingList [note.block].Add (tmp);
			}else if(type == 2){
				foreach (NoteInformation longNote in note.notes) {
					LongNoteTiming tmp = new LongNoteTiming();
					tmp.startTiming = LoadTiming (note.num+32);
					tmp.endTiming = LoadTiming (longNote.num+32);
                 
					longNoteStartTimingList [note.block].Add (tmp.startTiming);
					longNoteEndTimingList [note.block].Add (tmp.endTiming);
                    float longTiming = longNote.num - note.num;
                    float longTime = (longTiming * 2 / 25) * 1.31f;
                   // Debug.Log(longTime);
                    longActiveTimeList[note.block].Add(longTime);
                }
			}
		}

		for(int i = 0;i<noteLane.Length;i++){
			/*余分にタイミングを足すことでおかしな終了をしないようにする
			0,0,0は決して最後にならないので良い*/
			timingList [i].Add (new Timing (0, 0, 0));
			noteLane [i].timings = timingList [i].ToArray ();//timingListをリストから配列に変換し、各レーンのタイミングデータにする.
            noteLane[i].highSpeedLevel = highSpeedLevel;
			noteLane [i].updateStartPoint = startPoint;
        }

		for (int i = 0; i < longNoteLane.Length; i++) {
			longNoteStartTimingList [i].Add (new Timing (0, 0, 0));
			longNoteLane [i].startTimings = longNoteStartTimingList [i].ToArray ();
			longNoteEndTimingList [i].Add (new Timing (0, 0, 0));
			longNoteLane [i].endTimings = longNoteEndTimingList [i].ToArray ();
            longNoteLane[i].longActiveTimes = longActiveTimeList[i].ToArray();
            longNoteLane[i].highSpeedLevel = highSpeedLevel;
			longNoteLane [i].updateStartPoint = startPoint;
            longNoteLane[i].longLaneNum = i;
        }
	}

	private Timing LoadTiming(int n){
		int bar = (n / 4) / 4;
		int beat = (n / 4) % 4;
		int unit = n % 4;
		return new Timing (bar, beat, unit);
	}

	void Awake(){
        score = 0;
		LoadJson (loadJsonFileName);
		for (int i = 0; i < noteLane.Length; i++) {
			noteLane [i].enabled = true;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)){
            if (noteLane[4].hit())
            {
                score = score + 10;
            }
			if(longNoteLane[4].hit(true))
            {
                score = score + 10;
            }
        }
		if(Input.GetKeyDown(KeyCode.C)){
			if(noteLane [5].hit ())
            {
                score = score + 10;
            }
            if (longNoteLane[5].hit(true))
            {
                score = score + 10;
            }
        }
		if(Input.GetKeyDown(KeyCode.Z)){
			if(noteLane [6].hit ())
            {
                score = score + 10;
            }
            if(longNoteLane[6].hit(true))
            {
                score = score + 10;
            }
        }
		if(Input.GetKeyDown(KeyCode.X)){
			if(noteLane [7].hit ())
            {
                score = score + 10;
            }
            if(longNoteLane[7].hit(true))
            {
                score = score + 10;
            }
        }
		if(Input.GetKeyDown(KeyCode.V)){
			if(noteLane [9].hit ())
            {
                score = score + 10;
            }
        }
		if(Input.GetKeyDown(KeyCode.Space)){
			if(noteLane [10].hit ())
            {
                score = score + 10;
            }
        }
	}


	public void GetInput(int n)
	{
		if (n == 4)
		{
			noteLane[4].hit();
			longNoteLane[4].hit(true);
		}
		if (n == 7)
		{
			noteLane[5].hit();
			longNoteLane[5].hit(true);
		}
		if (n == 6)
		{
			noteLane[6].hit();
			longNoteLane[6].hit(true);
		}
		if (n == 5)
		{
			noteLane[7].hit();
			longNoteLane[7].hit(true);
		}
		if (n == 9)
		{
			noteLane[10].hit();
		}
		if (n == 10)
		{
			noteLane[8].hit();
		}
	}
}
