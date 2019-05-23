using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCountDetector : MonoBehaviour, IPointerClickHandler
{
    public GameObject startButton;
    public BaymaxGame baymaxGame;

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if(clickCount == 2)
        {
            startButton.SetActive(false);
            baymaxGame.OnDialogue();
        }
    }
}
