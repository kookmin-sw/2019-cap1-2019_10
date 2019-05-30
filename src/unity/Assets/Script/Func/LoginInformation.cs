using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
