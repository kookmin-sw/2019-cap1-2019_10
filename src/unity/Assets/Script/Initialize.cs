using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    private int cameraPermission = 0;

    private void Start()
    {
        // permission 체크..
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.HasUserAuthorization(UserAuthorization.WebCam);
        }
        else
        {
            cameraPermission = 1;
        }

        while (cameraPermission != 1)
        {
            System.Threading.Thread.Sleep(1000);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                cameraPermission = 1;
            }
        }
    }

}
