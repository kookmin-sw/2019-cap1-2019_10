using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BaymaxGame : BaseGame
{ 
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private HighscoreMenu highscoreMenu;

    public GameObject Dialogue;
    public GameObject NoticeToggle;
    public GameObject RecorderToggle;
    public GameObject LoginToggle;    

    public GameObject ResetButton;
    public GameObject LoadingImage;
    public GameObject NoticeImage;
    public GameObject ResultImage;

    public int stageCount;
    public Text DialogueText;

    public bool check = false;

    //[SerializeField]
    //private GUIText turnText;

    //[SerializeField]
    //private GUIText scoreText;

    private float Score;

    protected override void Start()
    {
        base.Start();

        highscoreMenu.enabled = false;

        stageCount = 0;

        Dialogue.SetActive(false);
        NoticeToggle.SetActive(false);
        RecorderToggle.SetActive(false);
        LoginToggle.SetActive(false);

        ResetButton.SetActive(false);
        LoadingImage.SetActive(false);
        NoticeImage.SetActive(false);
        ResultImage.SetActive(false);

        // Setup a delegate which will trigger when we succesfully posted a new highscore to the server.
        backendManager.OnPostScoreSucces += OnPostScoreSuccess;

        // Setup a delegate for when we close the highscore screen. This will reset the game and set it up for a new round of play
        //highscoreMenu.OnClose += ResetGame;
    }

    private void OnPostScoreSuccess()
    {
        // Do a GET request on the server for all the highscores. Whenever this is successfull, the highscore menu will automatticlly be triggered and opened
        backendManager.GetAllScores();
    }

    public void OnDialogue()
    {
        Dialogue.SetActive(true);
        ChangeDialogue(stageCount);
    }

    public void ChangeDialogue(int stageCount)
    {
        DialogueText.text = Scenario.instance.ChangeContent(stageCount);
    }

    //public void ResetGame()
    //{
    //    //ShowSaveMenu();
    //    highscoreMenu.enabled = false;
    //}

    public void TakePhoto()
    {
        phoneCamera.OnCamera();
        //Dialogue.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = true;
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
