using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDialogueDetector : MonoBehaviour, IPointerClickHandler
{
    public BaymaxGame baymaxGame;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (baymaxGame.stageCount)
        {
            case 1:
                baymaxGame.ChangeDialogue(baymaxGame.stageCount);
                GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = false;
                baymaxGame.TakePhoto();
                baymaxGame.stageCount++;
                break;
            default:
                baymaxGame.stageCount++;
                baymaxGame.ChangeDialogue(baymaxGame.stageCount);
                break;
        }
    }
}
