using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteData{
	public GameObject obj;
	public Timing timing;
	public float startTime;
}

public class Lane : MonoBehaviour {
	public Vector3 updateStartPoint;
	protected    Vector3 startPoint    =  new Vector3(0.0f, -28.0f, 60.0f);
    public int     nextTimingNum = 0;
	protected int     nextObjectNum = 0;
	public    float   offScreenPos  = -10.0f;
	//public 	  GameObject[] 	 ranks       = new GameObject[2];
	protected List<NoteData> activeNotes = new List<NoteData> ();
    public int highSpeedLevel;



    //protected struct 
    public int manageObjNum(int n, int maxNum){
		int num = n;
		if (num < maxNum-1) {
			num++;
		} else{
			num = 0;
		}
		return num;
	}
	public bool hit(){
		if (activeNotes.Count > 0) {
			int n = 1;
			int i = 0;
			foreach (NoteData item in activeNotes) {
				n = getGrade (item.timing);
				if (n != 1) {
					break;
				}
				i++;
			}
			if (n == 1) {
				return false;
			}
            StartCoroutine ("hiteffect", n);
			activeNotes [i].obj.SetActive (false);
			activeNotes.RemoveAt (i);
			return true;
		}
		return false;

	}


	IEnumerator hiteffect(int rank){
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<AudioSource>().Play();
        for (float f = 0.3f; f < 1.0f; f += 0.05f)
        {
            yield return null;
        }
        gameObject.GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<AudioSource>().Stop();
        /*GameObject obj = createRank(ranks [rank]);
		obj.transform.localPosition = new Vector2 (-8.0f, 0.0f);
		for (float f = 0.3f; f < 1.0f; f += 0.05f) {
			obj.transform.localScale = new Vector2 (f,f);
			Color c = obj.GetComponent<SpriteRenderer> ().color;
			c.a = 1 - f;
			obj.GetComponent<SpriteRenderer> ().color = c;
			yield return null;
		}
		obj.SetActive (false);*/
    }

	protected int getGrade(Timing item){
		float time = Music.MusicalTimeFrom (item);
		//time = Mathf.Abs (time);
		if (time < 1.8f&&time > -0.5f) {
			return 0;
		}
		return 1;
	}

	public bool equalTiming(Timing a, Timing b, int n){
		int num = b.Bar * 16 + b.Beat * 4 + b.Unit;
		num = num + n;
		int bar = (num / 4) / 4;
		int beat = (num / 4) % 4;
		int unit = num % 4;
		Timing c = new Timing (bar, beat, unit);
		return equalTiming (a,c);
	}

	public GameObject createNote(GameObject obj){
		obj.transform.position = startPoint;
        obj.GetComponent<Note> ().enabled = true;
        obj.SetActive(true);
        return obj;
	}
	public GameObject createRank(GameObject obj){
		obj.transform.position = startPoint;
        obj.SetActive(true);
        return obj;
	}

	bool equalTiming(Timing a,Timing b){

		//Debug.Log ("b bar beat unit" + b.Bar + " "+ b.Beat + " "+ b.Unit);
		if (a.Bar == b.Bar && a.Beat == b.Beat && a.Unit == b.Unit) {
			return true;
		}
		return false;
	}
}
