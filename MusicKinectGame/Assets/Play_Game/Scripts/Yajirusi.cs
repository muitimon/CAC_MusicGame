﻿using UnityEngine;
using System.Collections;

public class Yajirusi : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}

    private float x = 0f;
    private bool Xincrease = true;
	// Update is called once per frame
	void Update () {
        if (Xincrease) {
            x = x + 0.015f;
            if (Music.IsNearChangedBar())
            {
                x = x - 0.025f;
                Xincrease = false;
            }
        }else
        {
            x = x - 0.025f;
            if (Music.IsJustChangedBar())
            {
                Xincrease = true;
                x = 0f;
            }
        }
        float scale = QuadricFunc(x) + 0.8f;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
  
        
	}

    float QuadricFunc(float x)
    {
        x = x * x;
        return x;
    }
}
