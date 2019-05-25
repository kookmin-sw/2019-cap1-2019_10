using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialogue : BaseMenu
{
    public VoidDelegate TakePhoto;
    public VoidDelegate OnRecorde, OffRecorde, Recording;
    public VoidDelegate ShowResult, HideResult;
    public VoidDelegate Reset;

    public static Dialogue instance;

    public Text txt;
    public List<string> content = new List<string>();
    public int cnt = 0;

    State state;
    public bool locked = true;
    public bool clicked = false;

    public Vector3 MousePosition;

    //public Button.ButtonClickedEvent OnItemClick;
    public const int STATE_MAX = 6;
    enum State { Start, Photo, PhotoFin, Question, RecordeFin, Loading, Reset};

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
        clicked = false;
        cnt = 0;
        state = State.Start;

        content.Add("시작합니다");
        content.Add("사진");
        content.Add("다찍음");
        content.Add("질문");
        content.Add("녹음끝");
        content.Add("기다려봐");
        content.Add("다시할래");

        //backendManager.OnPhotoLoaded += SettingTag;
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked && !locked)
        {
            clicked = false;
            locked = true;
            StartCoroutine("Printing");
            if (cnt < STATE_MAX)
            {
                cnt++;
                state++;
            }
        }
    }

    public void Click()
    {
        if (!clicked)
        {
            clicked = true;
        }
    }

    public void DialogueStart()
    {
        locked = false;
    }

    private IEnumerator Printing()
    {
        txt.text = content[cnt];

        switch (state)
        {
            case State.Photo:
                yield return StartCoroutine("TakePicture");
                txt.text = content[cnt];
                break;
            case State.Question:
                yield return StartCoroutine("TakeRecorde");
                txt.text = content[cnt];
                OffRecorde();
                break;
            case State.Loading:
                yield return StartCoroutine("TakeResult");
                break;
            case State.Reset:
                
                break;
        }

        locked = false;
    }

    private void EndState()
    {
        ++state;
        ++cnt;
        txt.text = content[cnt];
    }

    private IEnumerator TakePicture()
    {
        TakePhoto();
        yield return new WaitUntil(() => BaymaxGame.instance.photoCheck);
        BaymaxGame.instance.photoCheck = false;
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator TakeRecorde()
    {
        OnRecorde();
        yield return new WaitUntil(() => BaymaxGame.instance.recodeCheck);
        BaymaxGame.instance.recodeCheck = false;
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator TakeResult()
    {
        yield return new WaitForSeconds(1f);
        ShowResult();
    }

    public void OnExit()
    {
        HideResult();
    }

    private IEnumerator Restart()
    {
        state = 0;
        cnt = 0;

        yield return null;
    }

    public void OnReset()
    {
        Reset();
        state = 0;
        cnt = 0;

        txt.text = content[cnt];
    }

    public void SettingTag(string[] photoEmotion)
    {

    }
}
