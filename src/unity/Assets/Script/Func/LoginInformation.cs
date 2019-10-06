using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 로그인시 전에 분석을 통해 나왔던 전체 노래 결과들을 display해주는 기능
public class LoginInformation : MonoBehaviour
{
    public bool clicked = false;

    public BackendManager backendManager;
    public GameObject loginInformation;
    public RectTransform panelRectTransform;
    public Text idText;
    public Toggle noticeToggle;
    public Toggle loginToggle;

    public List<Result> results;

    private string username = "";

    private void Start()
    {
        //backendManager.OnAllResultLoaded += DisplayAllResult;
        
        clicked = false;
        username = PlayerPrefs.GetString("x2").FromBase64();
        Debug.Log(username);
        if (username == "")
        {
            idText.text = "Not login";
        }
        else
        {
            idText.text = username;
        }
    }

    // 노래 결과 화면창 toggle
    public void onClicked()
    {
        if (noticeToggle.isOn == true)
        {
            loginToggle.isOn = false;
            return;
        }

        //clicked = !clicked;

        if (loginToggle.isOn == true)
        {
            panelRectTransform.SetAsLastSibling();
            loginInformation.SetActive(true);
        }
        else if (loginToggle.isOn == false)
        {
            loginInformation.SetActive(false);
        }
    }

    // 전체 노래결과 띄워줄 때 사용자 id도 함께 display
    public void DisplayAllResult(List<Result> results)
    {
        username = PlayerPrefs.GetString("x2").FromBase64();
        Debug.Log(username);
        if (username == "")
        {
            idText.text = "Not login";
        }
        else
        {
            idText.text = username;
        }

        this.results = results;
    }
}
