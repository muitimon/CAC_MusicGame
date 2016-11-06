using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class ResultSystem : AllSystem {
   // public GameObject text;
    public GameObject scoresText;
    public GameObject scoreText;
	public string saveDataPath = @"score.txt";

	// Use this for initialization
	void Start () {
        scoreText.GetComponent<Text>().text = "スコア : " + score.ToString();


		scores.Clear();
		scores.Add(score);
		if (File.Exists(saveDataPath))
		{
			RoadText(saveDataPath);
		}
		scores.Sort();
		WriteText(saveDataPath);


		string temp = "";
        for (int i = 0; i<scores.Count; i++)
        {
            int j = scores.Count - i;
			if (scores[i] == score)
			{
				temp = j + "位 " + scores[i] + "\t←\n" + temp;
			}
			else {
				temp = j + "位 " + scores[i] + "\n" + temp;
			}
        }
        scoresText.GetComponent<Text>().text = temp;
		/*
        if (scores.Count>=5)
        {
           // Debug.Log("remove");
            scores.RemoveAt(0);
        }
		*/
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SceneManager.LoadScene(0);
        }
	}

	public void WriteText(string _filePath)
	{
		//f = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
		//BinaryWriter writer = new BinaryWriter(f);
		StreamWriter sw;
		sw = new StreamWriter(_filePath);
		for (int i = 0; i < scores.Count; i++)
		{
			sw.WriteLine(scores[i]);
			//writer.Write(scores[i]);
		}
		sw.Close();
		//writer.Close();
	}

	public void RoadText(string _filePath)
	{
		FileStream f = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
		StreamReader reader = new StreamReader(f);
		if (reader != null)
		{
			while (!reader.EndOfStream)
			{
				string str = reader.ReadLine();
				scores.Add(int.Parse(str));
				//print(str);
			}
			reader.Close();
		}
	}
}
