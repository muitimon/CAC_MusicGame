using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultSystem : AllSystem {
    public GameObject text;
    public GameObject scoresText;
	// Use this for initialization
	void Start () {
        text.GetComponent<TextMesh>().text = score.ToString();
        scores.Add(score);
        scores.Sort();
        string temp = "";
        for (int i =0; i<scores.Count;i++)
        {
            temp = temp + scores[i] + "\n"; 
        }
        scoresText.GetComponent<TextMesh>().text = temp;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SceneManager.LoadScene(0);
        }
	}
}
