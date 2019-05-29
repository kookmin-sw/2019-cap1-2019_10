using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginInformation : MonoBehaviour
{
    public bool clicked = false;

    public GameObject loginInformation;
    public RectTransform panelRectTransform;
    public Text idText;
    public Toggle noticeToggle;
    public Toggle loginToggle;

    private string username = "";

    private void Start()
    {
        clicked = false;
        username = PlayerPrefs.GetString("x2").FromBase64();
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

}
