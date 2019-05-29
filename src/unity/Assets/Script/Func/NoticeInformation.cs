using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeInformation : MonoBehaviour
{
    public bool clicked = true;

    public GameObject noticeInformation;
    public RectTransform panelRectTransform;
    public Toggle loginToggle;
    public Toggle noticeToggle;

    private void Start()
    {
        clicked = true;
    }

    public void onClicked()
    {
        if (loginToggle.isOn == true)
        {
            noticeToggle.isOn = false;
            return;
        }
        //clicked = !clicked;

        if (noticeToggle.isOn == true)
        {
            panelRectTransform.SetAsLastSibling();
            noticeInformation.SetActive(true);
        }
        else if (noticeToggle.isOn == false)
        {
            noticeInformation.SetActive(false);
        }
    }
}
