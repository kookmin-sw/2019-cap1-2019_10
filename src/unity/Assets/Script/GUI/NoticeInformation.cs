using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeInformation : MonoBehaviour
{
    private bool clicked = true;

    public GameObject noticeInformation;

    public void onClicked()
    {
        clicked = !clicked;

        if (clicked)
        {
            noticeInformation.SetActive(true);
        }
        else if (!clicked)
        {
            noticeInformation.SetActive(false);
        }
    }
}
