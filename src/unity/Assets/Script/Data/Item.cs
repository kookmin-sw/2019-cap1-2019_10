using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public string Music;
    public string Tag;
    public string Url;

    /// 아이템을 클릭했을때 발생할 이벤트
    /// </summary>
    public Button.ButtonClickedEvent OnItemClick;
}