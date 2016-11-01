using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour
{
	public Material BoneMaterial;
	public GameObject BodySourceManager;

	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
	private BodySourceManager _BodyManager;

	float dx, dy;
	List<float> xList = new List<float>();
	List<float> yList = new List<float>();
	public float threshold = 1.5f;            // その方向に動いたと判定する閾値
	public string controlPoint = "HandTip";
	public string controlAxisText = "Sprine";
	private int direction = -1;
	public bool active = true;
	private Vector3 controlRight;
	private Vector3 controlLeft;
	private Vector3 controlAxis;
	public int previousAngleLeft = 0;
	public int previousAngleRight = 0;

	private Vector3 WristRight;
	private Vector3 WristLeft;
	private Vector3 HandTipRight;
	private Vector3 HandTipLeft;
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


	private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
	{
		{ Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
		{ Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
		{ Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
		{ Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

		{ Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
		{ Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
		{ Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
		{ Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

		{ Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
		{ Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
		{ Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
		{ Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.HandRight, Kinect.JointType.WristRight },
		{ Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
		{ Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
		{ Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
		{ Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
		{ Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
		{ Kinect.JointType.Neck, Kinect.JointType.Head },
	};

	void Update()
	{
		direction = -1;

		if (BodySourceManager == null)
		{
			return;
		}

		_BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
		if (_BodyManager == null)
		{
			return;
		}

		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null)
		{
			return;
		}

		List<ulong> trackedIds = new List<ulong>();
		foreach (var body in data)
		{
			if (body == null)
			{
				continue;
			}

			if (body.IsTracked)
			{
				trackedIds.Add(body.TrackingId);
			}
		}

		List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

		// First delete untracked bodies
		foreach (ulong trackingId in knownIds)
		{
			if (!trackedIds.Contains(trackingId))
			{
				Destroy(_Bodies[trackingId]);
				_Bodies.Remove(trackingId);
			}
		}

		foreach (var body in data)
		{
			if (body == null)
			{
				continue;
			}

			if (body.IsTracked)
			{
				if (!_Bodies.ContainsKey(body.TrackingId))
				{
					_Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
				}

				RefreshBodyObject(body, _Bodies[body.TrackingId]);

			}
		}

		// 通常の上下左右を認識させる処理
		checkNormal("Right", controlRight);
		checkNormal("Left", controlLeft);

		// 回転を認識させる処理
		angleLeftHand = getRadian(WristLeft, HandTipLeft);
		angleRightHand = getRadian(WristRight, HandTipRight);
		setDir("Left", angleLeftHand);
		setDir("Right", angleRightHand);
		checkRoll();

		//angleLeftHand = angleLeftHand * 180 / Mathf.PI;
	}

	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("Body:" + id);
		//body.GetComponent<Transform>().position.z = 3;
		body.SetActive(active);

		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.material = BoneMaterial;
			lr.SetWidth(0.05f, 0.05f);

			jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;
		}

		return body;
	}

	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
	{
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.Joint? targetJoint = null;

			if (_BoneMap.ContainsKey(jt))
			{
				targetJoint = body.Joints[_BoneMap[jt]];
			}

			Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint);

			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if (targetJoint.HasValue)
			{
				lr.SetPosition(0, jointObj.localPosition);
				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
				lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));

				//print(controlPoint + "Left");

				//if (jointObj.name.Equals("ElbowLeft"))
				if (jointObj.name.Equals("WristLeft"))
				{
					WristLeft = jointObj.position;
				}
				//if (jointObj.name.Equals("ElbowRight"))
				if (jointObj.name.Equals("WristRight"))
				{
					WristRight = jointObj.position;
				}
				if (jointObj.name.Equals("HandTipLeft"))
				{
					HandTipLeft = jointObj.position;
				}
				if (jointObj.name.Equals("HandTipRight"))
				{
					HandTipRight = jointObj.position;
				}
				if (jointObj.name.Equals(controlPoint + "Left"))
				{
					controlLeft = jointObj.position;
				}
				if (jointObj.name.Equals(controlPoint + "Right"))
				{
					controlRight = jointObj.position;
				}
				if (jointObj.name.Equals(controlAxisText))
				{
					controlAxis = jointObj.position;
				}
				//lr.enabled = false;


			}
		}
	}

	private static Color GetColorForState(Kinect.TrackingState state)
	{
		switch (state)
		{
			case Kinect.TrackingState.Tracked:
				return Color.green;

			case Kinect.TrackingState.Inferred:
				return Color.red;

			default:
				return Color.black;
		}
	}

	private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
	{
		return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}

	public int getDirection()
	{
		int dir = direction;
		direction = -1;
		return dir;
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
		xList.Add(jointPos.x - controlAxis.x);
		yList.Add(jointPos.y - controlAxis.y);

		dx = jointPos.x - xList[0];
		dy = jointPos.y - yList[0];
		if (xList.Count == 11)
		{
			xList.RemoveAt(0);
			yList.RemoveAt(0);

			//print("dx = " + dx + "\t dy = " + dy);
			if (dx > threshold)
			{
				xList.Clear();
				yList.Clear();
				print("→");
				direction = RIGHT;
			}
			else if (dx < -threshold)
			{
				xList.Clear();
				yList.Clear();
				print("←");
				direction = LEFT;
			}
			else if (dy > threshold)
			{
				xList.Clear();
				yList.Clear();
				print("↑");
				direction = UP;
			}
			else if (dy < -threshold)
			{
				xList.Clear();
				yList.Clear();
				print("↓");
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

	//ダミー
	private void sentMessage(int ans)
	{
		gameObject.SendMessage("GetInput", ans);
		//getInput(ans);
	}

}
