using UnityEngine;
using System.Collections;

public class Yajirusi1 : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}

    private float x = 0f;
    private bool Xincrease = true;
	// Update is called once per frame
	void Update () {
        if (Xincrease) {
            x = x + 0.1f;
            if (Music.IsNearChangedBeat())
            {
                x = x - 0.2f;
                Xincrease = false;
            }
        }else
        {
            x = x - 0.2f;
            if (Music.IsJustChangedBeat())
            {
                Xincrease = true;
                x = 0f;
            }
        }
		float scale = (QuadricFunc(x) + 0.8f) / 100;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
  
        
	}

    float QuadricFunc(float x)
    {
        x = 1/(1+Mathf.Exp(x));
        return x;
    }
}
