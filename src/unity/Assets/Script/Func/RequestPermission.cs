using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RequestPermission : MonoBehaviour
{
    //permission 요청하기
    private void Awake()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            // The user authorized use of the microphone.
            Debug.Log("microphone permission ok");
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Permission.RequestUserPermission(Permission.Microphone);
        }

        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("camera permission ok");
        }
        else
        {
            Permission.RequestUserPermission(Permission.Camera);
        }


        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Debug.Log("permission ok");
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }


        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Debug.Log("permission ok");
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }


        
    }
}
