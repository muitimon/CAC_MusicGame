using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SaveData : AllSystem{

	public string saveDataPath = "Assets/score.txt";
	private FileStream f;

	void Start ()
	{
		if (File.Exists(saveDataPath))
		{
			RoadText(saveDataPath);
		}
		scores.Add(10);
		scores.Add(200);
		scores.Sort();
		WriteText(saveDataPath);
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
		f = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
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



	void Update () {
	
	}
}
