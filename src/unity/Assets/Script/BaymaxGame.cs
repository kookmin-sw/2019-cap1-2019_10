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

    //[SerializeField]
    //private GUIText turnText;

    //[SerializeField]
    //private GUIText scoreText;

    private float Score;

    public static BaymaxGame instance;
    public bool photoCheck = false;
    public bool recodeCheck = false;

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    protected override void Start()
    {
        base.Start();

        photoCheck = false;
        recodeCheck = false;

        highscoreMenu.enabled = false;

        recorderToggle.SetActive(false);

        resetButton.SetActive(false);
        loadingImage.SetActive(false);
        resultImage.SetActive(false);

        // Setup a delegate which will trigger when we succesfully posted a new highscore to the server.
        backendManager.OnPostScoreSucces += OnPostScoreSuccess;

        // Setup a delegate for when we close the highscore screen. This will reset the game and set it up for a new round of play
        //highscoreMenu.OnClose += ResetGame;

        dialogue.TakePhoto += TakePhoto;
        dialogue.OnRecorde += OnRecorde;
        dialogue.OffRecorde += OffRecorde;
        dialogue.ShowResult += ShowResult;
        dialogue.HideResult += HideResult;
        dialogue.Reset += OnReset;
    }

    private void OnPostScoreSuccess()
    {
        // Do a GET request on the server for all the highscores. Whenever this is successfull, the highscore menu will automatticlly be triggered and opened
        backendManager.GetAllScores();
    }

    private void OnPostResultSuccess()
    {

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

    public void TakePhoto()
    {
        phoneCamera.OnCamera();
    }

    public void OnRecorde()
    {
        recorderToggle.SetActive(true);
    }

    public void OffRecorde()
    {
        recorderToggle.SetActive(false);
    }

    public void ShowResult()
    {
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
