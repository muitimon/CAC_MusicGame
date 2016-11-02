using UnityEngine;
using System.Collections;

public class JumpGesture : KinectManager.gestureData
{
    public bool moving = false;
    public JumpGesture(string fn, string gn) : base(fn, gn)
    {
    }
}
