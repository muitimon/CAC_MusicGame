
using UnityEngine;
using System.Collections;

public class NoteMove : MonoBehaviour
{

    [SerializeField, Range(0, 16)]
    float time = 1;
    [SerializeField]
    Vector3 startPosition;
    [SerializeField]
    Vector3 endPosition;

    public AudioClip sound01;
    public NoteL note;
    public bool autoMode = false;    // デバッグ用。自動演奏をするかどうか。

    GameObject gameobject;

    int originalFlag;    // コピーされるためにある最初のオブジェクトは、"1"にする
    bool moving = true;

    private float startTime;

    //void OnEnable()
    void Start()
    {
        // コピー元は動かさないようにする
        if (Music.MusicalTime < 0)
        {
            originalFlag = 1;
        }
        else
        {
            originalFlag = 0;
            startTime = Music.MusicalTime;
        }
        // time に設定された値が0以下だった場合はすぐ終点へ移動させる
        if (time <= 0)
        {
            transform.position = endPosition;
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (originalFlag == 0 && moving == true)
        {
            // Music.MusicalTime    では、 1が 1/4拍(16分音符)を示す
            // Music.MusicalTimeBar では、 0.1が 1拍を示す
            var diff = Music.MusicalTime - startTime;
            if (diff > time)  // 指定された時間を超えると移動完了
            {
                // デバッグ用。的の中央でヒット処理。
                if (autoMode == true)
                {
                    enabled = false;
                    note.setHitFlag(1);
                    GetComponent<AudioSource>().PlayOneShot(sound01);
                }

                startTime = Music.MusicalTime;
                endPosition.y -= startPosition.y;
                startPosition = transform.position;
                diff = Music.MusicalTime - startTime;
            }
            var rate = diff / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, rate);

            // 逃したノーツを削除する処理
            if (transform.position.y < -6)
            {
                enabled = false;
                //Destroy(note);
                note.setHitFlag(1);       // デバッグ用。的の中央でヒット処理。
            }
        }
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR

        if (!UnityEditor.EditorApplication.isPlaying || enabled == false)
        {
            startPosition = transform.position;
        }

        UnityEditor.Handles.Label(endPosition, endPosition.ToString());
        UnityEditor.Handles.Label(startPosition, startPosition.ToString());
#endif
        Gizmos.DrawSphere(endPosition, 0.1f);
        Gizmos.DrawSphere(startPosition, 0.1f);

        Gizmos.DrawLine(startPosition, endPosition);
    }

    // ヒットした時に使用する
    public void setMoving(bool b)
    {
        this.moving = b;
    }
}