using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectLevelSystem : AllSystem
{

    // Use this for initialization
    public GameObject[] easyPanel;
    public GameObject[] hardPanel;
    void Start()
    {
        /*easyPanel = new GameObject[2];
        hardPanel = new GameObject[2];*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Underscore))
        {
            Debug.Log("C");
            loadJsonFileName = "kanki_Heaven_Hard";
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Period))
        {
            Debug.Log("Z");
            loadJsonFileName = "kanki_Heaven_Easy";
        }
        if (loadJsonFileName.Equals("kanki_Heaven_Hard"))
        {
            hardPanel[0].SetActive(false);
            hardPanel[1].SetActive(true);
            easyPanel[0].SetActive(true);
            easyPanel[1].SetActive(false);

        }
        else if (loadJsonFileName.Equals("kanki_Heaven_Easy"))
        {
            hardPanel[0].SetActive(true);
            hardPanel[1].SetActive(false);
            easyPanel[0].SetActive(false);
            easyPanel[1].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Colon))
        {
            SceneManager.LoadScene(1);

        }
    }

    public void SetloadJsonFileName(string jsonName)
    {
        loadJsonFileName = jsonName;
    }

}