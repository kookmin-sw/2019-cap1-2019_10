using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDialogueDetector : MonoBehaviour, IPointerClickHandler
{
    public BaymaxGame baymaxGame;
    public GameObject Dialogue;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!baymaxGame.check)
        {
            switch (baymaxGame.stageCount)
            {
                case 1:
                    baymaxGame.ChangeDialogue(baymaxGame.stageCount);
                    baymaxGame.check = true;
                    baymaxGame.TakePhoto();
                    //Dialogue.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = false;
                    ++baymaxGame.stageCount;
                    break;
                default:
                    ++baymaxGame.stageCount;
                    baymaxGame.ChangeDialogue(baymaxGame.stageCount);
                    break;
            }
        }

    }
}
