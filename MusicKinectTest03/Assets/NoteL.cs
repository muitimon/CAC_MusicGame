using UnityEngine;
using System.Collections;

public class NoteL : MonoBehaviour {

    public AudioClip sound01;
    private int hitFlag = 0;
    int count = 0;
    Color alpha = new Color(0, 0, 0, 0.03f);
    int direction;

    public BodySourceView bodyPoint;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        if (hitFlag == 1)
        {
            hit();
        }
    }

    // Notesがアクションポジションに触れているときの処理
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Target") && hitFlag == 0)
        {

            if(direction == bodyPoint.getDirection())
            {
                Debug.Log("モーションでヒットしました");
                GetComponent<AudioSource>().PlayOneShot(sound01);
                GetComponent<NoteMove>().setMoving(false);
                hitFlag = 1;
            }

            switch (direction)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.UpArrow) == true)
                    {
                        Debug.Log("ヒットしました");
                        GetComponent<AudioSource>().PlayOneShot(sound01);
                        GetComponent<NoteMove>().setMoving(false);
                        hitFlag = 1;
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.RightArrow) == true)
                    {
                        Debug.Log("ヒットしました");
                        GetComponent<AudioSource>().PlayOneShot(sound01);
                        GetComponent<NoteMove>().setMoving(false);
                        hitFlag = 1;
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.DownArrow) == true)
                    {
                        Debug.Log("ヒットしました");
                        GetComponent<AudioSource>().PlayOneShot(sound01);
                        GetComponent<NoteMove>().setMoving(false);
                        hitFlag = 1;
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
                    {
                        Debug.Log("ヒットしました");
                        GetComponent<AudioSource>().PlayOneShot(sound01);
                        GetComponent<NoteMove>().setMoving(false);
                        hitFlag = 1;
                    }
                    break;
                default:
                    break;
            }
        }

    }

    // Notesがヒットした後のアニメーションのための処理（Notesをだんだん消していくとか）
    void hit()
    {
        if(count >= 50)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Renderer>().material.color -= alpha;
            GetComponentInChildren<TextMesh>().color -= alpha;
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
            count++;
        }

    }

    // 他のスクリプトからヒット処理を行うときに使用
    public void setHitFlag(int flag)
    {
        this.hitFlag = flag;
    }

    // 引数に対応する矢印を、子オブジェクトのテキストに代入する
    public void setDirection(int dir)
    {
        this.direction = dir;
        TextMesh tm = this.GetComponentInChildren<TextMesh>();
        switch (dir)
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
    }
}
