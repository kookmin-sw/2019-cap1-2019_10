﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialogue : BaseMenu
{
    public VoidDelegate TakePhoto;
    public VoidDelegate OnRecorde, OffRecorde, Recording;
    public VoidDelegate GetResult, GetResultEnded;
    public VoidDelegate ShowResult, HideResult;
    public VoidDelegate Reset;

    public static Dialogue instance;
    public Toggle ResultToggle;
    public GameObject LoadingImg;
    public Text Clicktxt;

    public Text debug;

    public Text txt;
    public List<string> content = new List<string>();
    public int cnt = 0;

    State state;
    public bool locked = true;
    public bool clicked = false;
    public bool isRecorded = false;
    public int retry = 0;

    //public Button.ButtonClickedEvent OnItemClick;
    public const int STATE_MAX = 7;
    enum State { First, Start, Photo, PhotoFin, Question, RecordeFin, Loading, Reset };

    protected void Awake()
    {
        base.Awake();

        instance = this;
    }

    // Start is called before the first frame update
    // 초기화
    void Start()
    {
        locked = true;
        clicked = false;
        isRecorded = false;
        cnt = 0;
        retry = 0;
        state = State.First;
        Clicktxt.enabled = false;
        LoadingImg.SetActive(false);

        content.Add("안녕, 나는 양양이야");
        content.Add("나한테 집중해줘");
        content.Add("너의 하루를 표정으로 표현해줘!\n" + "조금 오래 걸릴 수 있어");
        content.Add("고마워, 이제 내 질문에 대답해줘");
        content.Add("오늘 하루는 어땠어?");
        content.Add("대답해줘서 고마워");
        content.Add("기다려봐, 결과를 보여줄게");
        content.Add("다시할래?");

        //backendManager.OnPhotoLoaded += SettingTag;
        backendManager.OnPostPhotoSuccess += OnPostPhotoSuccess;
        backendManager.OnPostPhotoFailed += OnPostPhotoFailed;
        backendManager.OnPostAudioSuccess += OnPostAudioSuccess;
        backendManager.OnPostAudioFailed += OnPostAudioFailed;
    }

    // Update is called once per frame
    void Update()
    {
        // 다른 기능 수행중일땐 lock이 걸려있어서 다음으로 넘어가지 못하게
        if (clicked && !locked)
        {
            clicked = false;
            locked = true;
            Clicktxt.enabled = false;
            
            if (cnt < STATE_MAX)
            {
                cnt++;
                state++;
            }
            StartCoroutine("Printing");
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
        Debug.Log(cnt + " : " + state);
        txt.text = content[cnt];
        locked = false;
        Clicktxt.enabled = true;
    }

    // 어플 흐름
    // 해당 state에 맞는 기능 수행
    private IEnumerator Printing()
    {
        txt.text = content[cnt];

        switch (state)
        {
            case State.Photo:
                yield return StartCoroutine("TakePicture");
                EndPrinting();
                txt.text = content[cnt];
                break;
            case State.Question:
                yield return StartCoroutine("TakeRecorde");
                EndPrinting();
                txt.text = content[cnt];
                OffRecorde();
                break;
            case State.Loading:
                yield return StartCoroutine("TakeResult");
                EndPrinting();
                locked = false;
                break;
            case State.Reset:
                break;
        }

        EndState();

    }

    private void EndPrinting()
    {
        ++state;
        ++cnt;
    }

    private void EndState()
    {
        //++state;
        //++cnt;
        txt.text = content[cnt];
        locked = false;
        if(state == State.Reset)
        {
            Clicktxt.enabled = false;
        }
        else
        {
            Clicktxt.enabled = true;
        }
    }

    // 얼굴 인식 요청
    // 결과를 수신할 때까지 대기
    private IEnumerator TakePicture()
    {
        TakePhoto();
        LoadingImg.SetActive(true);
        yield return new WaitUntil(() => BaymaxGame.instance.photoCheck);
        yield return new WaitForSeconds(1f);
        LoadingImg.SetActive(false);
        BaymaxGame.instance.photoCheck = false;
    }

    // 음성 인식 요청
    private IEnumerator TakeRecorde()
    {
        OnRecorde();
        yield return new WaitUntil(() => BaymaxGame.instance.recodeCheck);
        yield return new WaitForSeconds(1f);
        LoadingImg.SetActive(false);
        BaymaxGame.instance.recodeCheck = false;
    }

    // 결과 요청
    private IEnumerator TakeResult()
    {
        GetResult();
        LoadingImg.SetActive(true);
        debug.text = "get";
        yield return new WaitUntil(() => BaymaxGame.instance.resultCheck);
        debug.text = "result end";
        yield return new WaitForSeconds(1f);
        LoadingImg.SetActive(false);
        Clicktxt.enabled = false;
        ShowResult();
    }

    public void ResultError()
    {
        LoadingImg.SetActive(false);
        txt.text = "서버 상태가 안좋아서 더이상 진행할 수 없어.. 미안해\n" + "다음에 다시 해줘";
    }

    public void OnExit()
    {
        HideResult();
        //if (ResultToggle.isOn)
        //{
        //    ShowResult();
        //}
        //else
        //{
        //    HideResult();
        //}

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

        isRecorded = false;
        Clicktxt.enabled = true;
        txt.text = content[cnt];
    }

    public void SettingTag(string[] photoEmotion)
    {


    }

    public void OnPostPhotoSuccess(string[] emotions)
    {
        Debug.Log("success");
        //for (int i = 0; i < emotions.Length; i++)
        //{
        //    if (emotions[i] != "") Debug.Log(emotions[i]);
        //}
        BaymaxGame.instance.photoCheck = true;
        retry = 0;
    }

    public void OnPostPhotoFailed()
    {
        if (retry < 3)
        {
            txt.text = "잘 안보여서.. 잠시만 기다려봐";
            ++retry;
            Debug.Log("다시" + retry);
            TakePhoto();
        }
        else
        {
            ResultError();
            //txt.text = "서버 상태가 안좋아서 더이상 진행할 수 없어.. 미안해";
        }
    }

    public void OnPostAudioSuccess(string[] emotions)
    {
        Debug.Log("success");
        Debug.Log(emotions[0]);
        BaymaxGame.instance.recodeCheck = true;
        retry = 0;
    }

    public void OnPostAudioFailed()
    {
        if (retry < 3)
        {
            LoadingImg.SetActive(false);
            ++retry;
            Debug.Log("retry : " + retry);
            txt.text = "다시 들려줄 수 있어?";
        }
        else
        {
            ResultError();
            //txt.text = "서버 상태가 안좋아서 더이상 진행할 수 없어, 미안해..";
        }
    }

    public void OnPostRecorder()
    {
        isRecorded = true;
    }
}
