using UnityEngine;
using System.Collections;

public class FadeSample : MonoBehaviour
{

	public void FadeScene ()
	{
		FadeManager.Instance.LoadLevel ("Make_Object", 2.0f);
	}
}
