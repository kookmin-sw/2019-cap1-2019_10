using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
