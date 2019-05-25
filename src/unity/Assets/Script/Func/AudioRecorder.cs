using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.IO;

//Use the PointerDown and PointerUP interfaces to detect a mouse down and up on a ui element
public class AudioRecorder : BaseMenu
{
    AudioClip recording;
    //Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
    public AudioSource audioSource;
    private float startRecordingTime;
    private string filepath = "";
    private bool clicked = false;
    public byte[] audioData;

    //Get the audiosource here to save resources
    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    public void onClicked()
    {
        clicked = !clicked;

        if (clicked)
        {
            //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
            int minFreq;
            int maxFreq;
            int freq = 44100;
            Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
            if (maxFreq < 44100)
                freq = maxFreq;

            //Start the recording, the length of 300 gives it a cap of 5 minutes
            recording = Microphone.Start("", false, 300, 44100);
            startRecordingTime = Time.time;
        }

        if (!clicked)
        {
            //End the recording when the mouse comes back up, then play it
            Microphone.End("");

            //Trim the audioclip by the length of the recording
            AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
            float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
            recording.GetData(data, 0);
            recordingNew.SetData(data, 0);
            this.recording = recordingNew;

            //Play recording
            audioSource.clip = recording;
            //audioSource.Play();

            filepath = SavWav.Save("myfile", audioSource.clip);
            audioData = File.ReadAllBytes(filepath);
            backendManager.SendFile("speech/", "audio", audioData, "myfile.wav", "audio/wav");
            BaymaxGame.instance.recodeCheck = true;

            audioData = null;
            File.Delete(filepath);
        }
    }
}