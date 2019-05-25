using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCountDetector : MonoBehaviour, IPointerClickHandler
{
    public GameObject DialogueImage;
    public GameObject start;

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if(clickCount == 2)
        {
            start.SetActive(false);
            //깨어나기
            Debug.Log("깨어나기");
            DialogueImage.SetActive(true);
            Dialogue.instance.DialogueStart();
        }
    }
}
