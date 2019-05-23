using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour
{
    public static Scenario instance;

    //생성자 함수, start보다 빨리
    private void Awake()
    {
        instance = this;
    }

    public string ChangeContent(int stage)
    {
        switch (stage)
        {
            case 0:
                return ("Hi I'm baymax\n" +
                    "your personal healthcare companion!");
                break;
            case 1:
                return("take a picture");
                break;
            case 2:
                return ("blahhbllahhh");
                break;
            case 3:
                return("Recorder ...");
                break;
            case 4:
                return ("asdf");
                break;
            case 5:
                return ("ddddd");
                break;
            case 6:
                return ("Dddddddddd");
                break;
        }

        return "";
    }
}
