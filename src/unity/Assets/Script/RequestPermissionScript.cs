using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RequestPermissionScript : MonoBehaviour
{
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            // The user authorized use of the microphone.
        }
        else
        {
            // We do not have permission to use the microphone.
            // Ask for permission or proceed without the functionality enabled.
            Debug.Log("퍼미션 줘");
            Permission.RequestUserPermission(Permission.Microphone);
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
}
