using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

    public AudioClip sound01;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            // Torigger
            Debug.Log("A!");
            GetComponent<AudioSource>().PlayOneShot(sound01);
        }
    }
}
