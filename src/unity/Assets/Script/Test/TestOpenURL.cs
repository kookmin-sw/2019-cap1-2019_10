using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenURL : MonoBehaviour
{
    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        cnt = 0;
    }

    public void onClick()
    {
        switch (cnt)
        {
            case 0:
                Application.OpenURL("https://youtu.be/jR61JQwFnls/");
                break;
            case 1:
                Application.OpenURL("https://www.youtube.com/");
                break;
            case 2:
                cnt = 0;
                break;
        }
        cnt++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
