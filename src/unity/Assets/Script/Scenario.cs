using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour
{
    public GameObject Background;
    public Text Content;
    public Button Next;
    public GameObject Reset;
    public GameObject Recorder;
    public GameObject Camera;

    public bool isReady = true;
    public bool isEnd = false;
    public int stage = 0;

    public static Scenario instance;

    //생성자 함수, start보다 빨리
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Reset.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isReady = false;

                Background.SetActive(true);
            }
        }

        if (!isReady)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (isEnd)
                {
                    return;
                }

                ChangeContent(stage);
                stage++;
            }
        }
    }

    void ChangeContent(int stage)
    {
        switch (stage)
        {
            case 0:
                Content.text = 
                    "Hi I'm baymax\n" +
                    "your personal healthcare companion!";
                break;
            case 1:
                Content.text = "take a picture";
                Camera.SetActive(true);
                isEnd = true;
                break;
            case 2:
                Camera.SetActive(false);
                Content.text = "blahhbllahhh";
                break;
            case 3:
                Content.text = "Recorder ...";
                Recorder.SetActive(true);
                isEnd = true;
                break;
            case 4:
                Recorder.SetActive(false);
                Content.text = "asdf";
                break;
            case 5:
                Content.text = "또 뭐있나";
                break;
            case 6:
                Content.text = "끝";
                Reset.SetActive(true);
                break;
        }
    }

    public void OnClickedReset()
    {
        stage = 0;

        isReady = true;
        Background.SetActive(false);
        Reset.SetActive(false);
    }
}
