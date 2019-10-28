using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaymaxGame : BaseGame
{
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private HighscoreMenu highscoreMenu;

    [SerializeField]
    private Dialogue dialogue;

    public GameObject recorderToggle;

    public GameObject resetButton;
    public GameObject loadingImage;
    public GameObject resultImage;

    public int checkReusltResquestTime;
    public float checkStartTime;

    //[SerializeField]
    //private GUIText turnText;

    //[SerializeField]
    //private GUIText scoreText;

    private float Score;
    public Text debug;

    public static BaymaxGame instance;
    public bool photoCheck = false;
    public bool recodeCheck = false;
    public bool resultCheck = false;

    // Toast를 띄우기 위한
    string toastString;
    string input;
    AndroidJavaObject currentActivity;
    AndroidJavaClass UnityPlayer;
    AndroidJavaObject context;
    AndroidJavaObject toast;
    public bool isPressed = false;
    public float pressedTime = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }
    
    // 초기화
    protected override void Start()
    {
        base.Start();

        photoCheck = false;
        recodeCheck = false;
        resultCheck = false;

        checkReusltResquestTime = 0;

        highscoreMenu.enabled = false;

        recorderToggle.SetActive(false);

        resetButton.SetActive(false);
        loadingImage.SetActive(false);
        resultImage.SetActive(false);

        // Setup a delegate which will trigger when we succesfully posted a new highscore to the server.
        backendManager.OnPostScoreSucces += OnPostScoreSuccess;

        backendManager.OnResultLoaded += OnResultLoaded;
        backendManager.OnResultLoadedFailed += OnResultLoadedFailed;

        backendManager.OnPostAudioSuccess += OnPostAudioSuccess;
        backendManager.OnPostAudioFailed += OnPostAudioFailed;

        // Setup a delegate for when we close the highscore screen. This will reset the game and set it up for a new round of play
        //highscoreMenu.OnClose += ResetGame;

        dialogue.TakePhoto += TakePhoto;
        dialogue.OnRecorde += OnRecorde;
        dialogue.OffRecorde += OffRecorde;
        dialogue.GetResult += GetResult;
        dialogue.ShowResult += ShowResult;
        dialogue.HideResult += HideResult;
        dialogue.Reset += OnReset;


        isPressed = false;
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        }
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("time : " + Time.time + ", pressed : " + pressedTime);

            if(Time.time - pressedTime > 2.0f)
            {
                pressedTime = Time.time;
                showToastOnUiThread("\'뒤로\' 버튼을 한 번 더 누르면 종료됩니다.");
                return;
            }
            if(Time.time - pressedTime <= 2.0f)
            {
                Application.Quit();
                CancelToast();
            }
        }
#endif
    }

    public void CancelToast()
    {
        currentActivity.Call("runOnUiThread",
            new AndroidJavaRunnable(() =>
            {
                if (toast != null) toast.Call("cancel");
            }));
    }

    public void showToastOnUiThread(string toastString)
    {
        this.toastString = toastString;
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
    }

    public void showToast()
    {
        Debug.Log(this + ": Running on UI thread");

        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }

    private void OnPostScoreSuccess()
    {
        // Do a GET request on the server for all the highscores. Whenever this is successfull, the highscore menu will automatticlly be triggered and opened
        backendManager.GetAllScores();
    }

    //private void OnPostResultSuccess()
    //{
    //    backendManager.GetResult();
    //}

    private void OnResultLoaded(List<Result> result)
    {
        resultCheck = true;
    }

    private void OnResultLoadedFailed()
    {
        checkReusltResquestTime++;
        if (checkReusltResquestTime < 3)
        {
            GetResult();
        }
        else
        {
            dialogue.ResultError();
        }

        Debug.Log("Get Result Failed" + checkReusltResquestTime);
    }

    public void ResetGame()
    {
        //ShowSaveMenu();
        highscoreMenu.enabled = false;
    }

    public void OnReset()
    {
        resetButton.SetActive(false);
        HideResult();
        
    }

    // 얼굴인식
    public void TakePhoto()
    {
        phoneCamera.OnCamera();
    }

    // 녹음 토글 on
    public void OnRecorde()
    {
        recorderToggle.SetActive(true);
    }

    public void OnPostAudioSuccess(string[] emotions)
    {
        recorderToggle.GetComponent<Toggle>().interactable = true;
    }

    public void OnPostAudioFailed()
    {
        recorderToggle.GetComponent<Toggle>().interactable = true;
    }


    // 녹음 토글 버튼 off
    public void OffRecorde()
    {
        recorderToggle.SetActive(false);
    }

    // 서버에 결과 요청
    public void GetResult()
    {
        backendManager.GetResult(PlayerPrefs.GetString("x2").FromBase64());
    }

    // 결과 display
    public void ShowResult()
    {
        debug.text = "show";
        resultImage.SetActive(true);
        resetButton.SetActive(true);
    }

    public void HideResult()
    {
        resultImage.SetActive(false);
    }

    protected override bool IsMouseOverMenu()
    {
        return base.IsMouseOverMenu() || highscoreMenu.IsMouseOver();
    }

    private void OnGameFinished()
    {
        //HideSaveMenu();

        StartCoroutine(PostScores());
    }

    private IEnumerator PostScores()
    {
        yield return new WaitForFixedUpdate();

        // Every 0.5 second, check if velocity of balls is below the BALL_VELOCITY_THRESHOLD, if so, then post scores. 
        //while (balls.Where(ball => ball.GetComponent<Rigidbody>().velocity.sqrMagnitude > BALL_VELOCITY_THRESHOLD).ToArray().Length != 0)
        yield return new WaitForSeconds(2f);

        highscoreMenu.enabled = true;
        highscoreMenu.CurrentScore = Score;

        // Post our final score to the back-end. When this request is succesfull, it will trigger the ExampleBackend.OnPostScoreSucces() delegate. 
        backendManager.PostScore((int)Score);
    }
}
