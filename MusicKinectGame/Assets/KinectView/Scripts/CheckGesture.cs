﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckGesture : MonoBehaviour {
	float dx, dy;
	List<float> xListR = new List<float>();
	List<float> yListR = new List<float>();
	List<float> xListL = new List<float>();
	List<float> yListL = new List<float>();
	public float threshold = 1.5f;            // その方向に動いたと判定する閾値
	public string controlPoint = "HandTip";
	public string controlAxisText = "SpineMid";
	private int direction = -1;
	public bool active = true;
	private GameObject controlRight;
	private GameObject controlLeft;
	private GameObject controlAxis;
	public int previousAngleLeft = 0;
	public int previousAngleRight = 0;

	private GameObject WristRight;
	private GameObject WristLeft;
	private GameObject HandTipRight;
	private GameObject HandTipLeft;
	private float angleRightHand;
	private float angleLeftHand;

	private const int UP = 0;
	private const int DOWN = 1;
	private const int LEFT = 2;
	private const int RIGHT = 3;
	public int delayCount = 50;
	public bool[] leftHandDir = new bool[4];  //up, down, left, right
	public bool[] rightHandDir = new bool[4];  //up, down, left, right
	public int[] leftHandDirCount = new int[4];   //up, down, left, right
	public int[] rightHandDirCount = new int[4];   //up, down, left, right

	public GameObject gamesystem;

	// Use this for initialization
	void Start () {
		WristRight = gameObject.transform.FindChild("WristRight").gameObject;
		WristLeft = gameObject.transform.FindChild("WristLeft").gameObject;
		HandTipRight = gameObject.transform.FindChild("HandTipRight").gameObject;
		HandTipLeft = gameObject.transform.FindChild("HandTipLeft").gameObject;
		controlLeft = gameObject.transform.FindChild(controlPoint + "Left").gameObject;
		controlRight = gameObject.transform.FindChild(controlPoint + "Right").gameObject;
		controlAxis = gameObject.transform.FindChild(controlAxisText).gameObject;

		gamesystem = GameObject.Find("GameSystem");
	}
	
	// Update is called once per frame
	void Update () {
		// 通常の上下左右を認識させる処理
		checkNormal("Right", controlRight.transform.position);
		checkNormal("Left", controlLeft.transform.position);
		//print(xList.ToString() + ", " + yList.ToString());

		// 回転を認識させる処理
		angleLeftHand = getRadian(WristLeft.transform.position, HandTipLeft.transform.position);
		angleRightHand = getRadian(WristRight.transform.position, HandTipRight.transform.position);
		setDir("Left", angleLeftHand);
		setDir("Right", angleRightHand);
		checkRoll();
	}

	private void setDir(string RorL, float angle)
	{
		int dir;
		float rad = Mathf.PI / 4;

		if (rad <= angle && angle < rad * 3)
		{
			dir = UP;
		}
		else if (angle >= rad * 3 || angle <= -rad * 3)
		{
			dir = LEFT;
		}
		else if (-rad * 3 < angle && angle < -rad)
		{
			dir = DOWN;
		}
		else {
			dir = RIGHT;
		}
		switch (RorL)
		{
			case "Right":
				if (rightHandDir[dir] == false)
				{
					for (int i = 0; i < 4; i++)
					{
						if (i != dir)
						{
							//rightHandDir[i] = false;
						}
						else {
							rightHandDir[i] = true;
							rightHandDirCount[i] = delayCount;
						}
					}
				}
				break;
			case "Left":
				if (leftHandDir[dir] == false)
				{
					for (int i = 0; i < 4; i++)
					{
						if (i != dir)
						{
							//leftHandDir[i] = false;
						}
						else {
							leftHandDir[i] = true;
							leftHandDirCount[i] = delayCount;
						}
					}
				}
				break;
		}
	}

	private void checkRoll()
	{
		bool leftFlag = true;
		bool rightFlag = true;
		for (int i = 0; i < 4; i++)
		{
			if (leftHandDir[i] == false)
			{
				leftFlag = false;
			}
			if (rightHandDir[i] == false)
			{
				rightFlag = false;
			}
		}

		if (leftFlag == true)
		{
			print("LEFT ROLLING!");
			sentMessage(9);
			for (int i = 0; i < 4; i++)
			{
				leftHandDirCount[i] = 0;
				leftHandDir[i] = false;
			}
		}
		if (rightFlag == true)
		{
			print("RIGHT ROLLING!");
			sentMessage(8);
			for (int i = 0; i < 4; i++)
			{
				rightHandDirCount[i] = 0;
				rightHandDir[i] = false;
			}
		}

		for (int i = 0; i < 4; i++)
		{
			if (rightHandDirCount[i] > 0)
			{
				rightHandDirCount[i]--;
				if (rightHandDirCount[i] == 0)
				{
					rightHandDir[i] = false;
				}
			}
			if (leftHandDirCount[i] > 0)
			{
				leftHandDirCount[i]--;
				if (leftHandDirCount[i] == 0)
				{
					leftHandDir[i] = false;
				}
			}
		}
	}

	private void checkNormal(string RorL, Vector3 jointPos)
	{
		if (RorL == "Right")
		{
			xListR.Add(jointPos.x - controlAxis.transform.position.x);
			yListR.Add(jointPos.y - controlAxis.transform.position.y);

			dx = jointPos.x - controlAxis.transform.position.x - xListR[0];
			dy = jointPos.y - controlAxis.transform.position.y - yListR[0];
		}
		else {
			xListL.Add(jointPos.x - controlAxis.transform.position.x);
			yListL.Add(jointPos.y - controlAxis.transform.position.y);

			dx = jointPos.x - controlAxis.transform.position.x - xListL[0];
			dy = jointPos.y - controlAxis.transform.position.y - yListL[0];
		}
		if (xListR.Count >= 11 || xListL.Count >= 11)
		{
			if (RorL == "Right")
			{
				//print("dx = " + dx + "\t dy = " + dy);
				xListR.RemoveAt(0);
				yListR.RemoveAt(0);
			}
			else {
				xListL.RemoveAt(0);
				yListL.RemoveAt(0);
			}

			//print("dx = " + dx + "\t dy = " + dy);
			if (dx > threshold)
			{
				if (RorL == "Right")
				{
					xListR.Clear();
					yListR.Clear();
				}
				else
				{
					xListL.Clear();
					yListL.Clear();
				}
				print(RorL + " →");
				direction = RIGHT;
			}
			else if (dx < -threshold)
			{
				if (RorL == "Right")
				{
					xListR.Clear();
					yListR.Clear();
				}
				else
				{
					xListL.Clear();
					yListL.Clear();
				}
				print(RorL + " ←");
				direction = LEFT;
			}
			else if (dy > threshold)
			{
				if (RorL == "Right")
				{
					xListR.Clear();
					yListR.Clear();
				}
				else
				{
					xListL.Clear();
					yListL.Clear();
				}
				print(RorL + " ↑");
				direction = UP;
			}
			else if (dy < -threshold)
			{
				if (RorL == "Right")
				{
					xListR.Clear();
					yListR.Clear();
				}
				else
				{
					xListL.Clear();
					yListL.Clear();
				}
				print(RorL + " ↓");
				direction = DOWN;
			}

			if (RorL == "Right")
			{
				previousAngleRight = direction;
				sentMessage(direction);
			}
			else {
				previousAngleLeft = direction;
				sentMessage(direction + 4);
			}
		}
	}

	public int getPreviousRight()
	{
		return previousAngleRight;
	}
	public int getPreviousLeft()
	{
		return previousAngleLeft + 4;
	}

	// 同じオブジェクトにあるGameSystem.cs内のGetInput関数を引数を入れて呼び出す。
	private void sentMessage(int ans)
	{
		//gameObject.SendMessage("GetInput", ans);
		gamesystem.GetComponent<GameSystem>().GetInput(ans);
	}

	protected float getRadian(Vector3 pos1, Vector3 pos2)
	{
		float x = pos1.x;
		float y = pos1.y;
		float x2 = pos2.x;
		float y2 = pos2.y;
		float radian = Mathf.Atan2(y2 - y, x2 - x);
		return radian;
	}
}