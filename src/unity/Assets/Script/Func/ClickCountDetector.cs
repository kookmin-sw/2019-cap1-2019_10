using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCountDetector : MonoBehaviour, IPointerClickHandler
{
    public GameObject DialogueImage;
    public GameObject start;
    public GameObject LoginToggle;

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if(clickCount == 2)
        {
            //start.SetActive(false);
            ////깨어나기
            //Debug.Log("깨어나기");
            //DialogueImage.SetActive(true);
            //Dialogue.instance.DialogueStart();
        }
    }

    public int tapCount = 0;
    public void OnTap()
    {
        if (tapCount >= 2) return;
        if (tapCount <= 0) Invoke("TapCheck", 0.3f);
        tapCount++;
    }

    void TapCheck()
    {
        switch (tapCount)
        {
            case 2:
                start.SetActive(false);
                //깨어나기
                Debug.Log("깨어나기");
                LoginToggle.SetActive(true);
                DialogueImage.SetActive(true);
                Dialogue.instance.DialogueStart();
                break;

        }
        tapCount = 0;
    }
}
