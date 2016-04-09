using UnityEngine;
using System.Collections;

public class NoteMana : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Music.IsJustChangedBar())
        {
            // オリジナルのノーツからコピーを生成し、向き情報を与える
            GameObject original = GameObject.Find("NoteL");
            GameObject copied = Object.Instantiate(original) as GameObject;
            int rand = Random.Range(0, 4);
            copied.SendMessage("setDirection", rand);  // 新しいノーツのsetDirectionメソッドを引数randを与えて実行
            /*
            TextMesh tm = copied.GetComponentInChildren<TextMesh>();
            switch (rand)
            {
                case 0:
                    tm.text = "↑";
                    break;
                case 1:
                    tm.text = "→";
                    break;
                case 2:
                    tm.text = "↓";
                    break;
                case 3:
                    tm.text = "←";
                    break;

            }
            */

            //copied = transform.Find("Arrow").gameObject;
            //copied.transform.gameObject.SetActive(false);
            //copied.transform.Translate(2, 6, 0);
        }
    }
}
