using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    AudioClip myAudioClip;
    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        Debug.Log(Microphone.devices[0]);
    }

    void Update() { }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 60, 50), "Record"))
        {
            //myAudioClip = Microphone.Start(null, false, 10, 44100);
            //myAudioClip = Microphone.Start(Microphone.devices[0], false, 10, 44100);
            aud.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
            Debug.Log(aud.clip);
        }
        if (GUI.Button(new Rect(10, 70, 60, 50), "Save"))
        {
            SavWav.Save("myfile", aud.clip);
            aud.Play();
        }
    }

}