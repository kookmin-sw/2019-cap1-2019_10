using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginInformation : MonoBehaviour
{
    private bool clicked = false;

    public GameObject loginInformation;
    public Text idText;

    private string username = "";

    public void onClicked()
    {
        clicked = !clicked;

        if (clicked)
        {
            username = PlayerPrefs.GetString("x2").FromBase64();
            idText.text = username;

            loginInformation.SetActive(true);
        }
        else if (!clicked)
        {
            loginInformation.SetActive(false);
        }
    }

}
