using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultSystem : AllSystem {
    public GameObject text;
    public GameObject scoresText;
	// Use this for initialization
	void Start () {
        text.GetComponent<TextMesh>().text = score.ToString();

        string temp = "";
        for (int i =scores.Count-1; i>=0;i--)
        {
            temp = temp + scores[i] + "\n"; 
        }
        scoresText.GetComponent<TextMesh>().text = temp;
        if (scores.Count>=5)
        {
            scores.Remove(0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SceneManager.LoadScene(0);
        }
	}
}
