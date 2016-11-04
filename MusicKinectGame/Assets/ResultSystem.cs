using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSystem : AllSystem {
   // public GameObject text;
    public GameObject scoresText;
    public GameObject scoreText;
	// Use this for initialization
	void Start () {
        scoreText.GetComponent<Text>().text = score.ToString();

        string temp = "";
        for (int i =scores.Count-1; i>=0;i--)
        {
            int j = i + 1;
            temp = j + "位 " + scores[i] + "\n" + temp;
        }
        scoresText.GetComponent<Text>().text = temp;
        if (scores.Count>=5)
        {
           // Debug.Log("remove");
            scores.RemoveAt(0);
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
