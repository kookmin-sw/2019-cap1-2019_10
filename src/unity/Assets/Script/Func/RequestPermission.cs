using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RequestPermission : MonoBehaviour
{
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone) && Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // The user authorized use of the microphone.
            Debug.Log("microphone and camera permission ok");
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.Microphone);
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
}
