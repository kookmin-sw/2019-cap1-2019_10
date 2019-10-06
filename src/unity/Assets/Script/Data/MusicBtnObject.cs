using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 노래 정보 display button 구성 요소
public class MusicBtnObject : MonoBehaviour
{
    public Text music;
    public string link;
    public Text tag;

    public void OnClick()
    {
        Application.OpenURL(link);
    }
}
