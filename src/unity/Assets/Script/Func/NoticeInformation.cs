using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 어플 설명을 위한 notice 화면 구성
// 제일 처음 어플 실행시 토글 버튼이 활성화 되어있어 설명창이 띄워져있을 수 있도록
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
            //panelRectTransform.SetAsLastSibling();
            panelRectTransform.SetAsFirstSibling();
            noticeInformation.SetActive(true);
        }
        else if (noticeToggle.isOn == false)
        {
            noticeInformation.SetActive(false);
        }
    }
}
