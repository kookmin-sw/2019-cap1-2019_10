using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Android;

// 오디오 녹음 및 저장
public class AudioRecorder : BaseMenu
{
    AudioClip recording;
    //Keep this one as a global variable (outside the functions) too and use GetComponent during start to save resources
    public AudioSource audioSource;
    private float startRecordingTime;
    private string filepath = "";
    private bool clicked = false;
    public byte[] audioData;

    public GameObject Loading;
    public Text debug;
    public GameObject RecorderError;

    //Get the audiosource here to save resources
    private void Start()
    {

    }

    // 클릭을 하면 녹음 시작, 다시 한 번 클릭을 하면 그때까지의 음성 녹음하기
    public void onClicked()
    {
        clicked = !clicked;

        // 첫번째 클릭은 녹음 시작 (~ing)
        if (clicked)
        {
            RecorderError.SetActive(false);

            //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
            int minFreq;
            int maxFreq;
            int freq = 44100;
            Microphone.GetDeviceCaps(Microphone.devices[0], out minFreq, out maxFreq);
            if (maxFreq < 44100)
                freq = maxFreq;

            //Start the recording, the length of 300 gives it a cap of 5 minutes
            recording = Microphone.Start(Microphone.devices[0], false, 300, 44100);
            //debug.text = recording.ToString();
            startRecordingTime = Time.time;
        }

        // 두번째 클릭은 녹음 멈추고 저장하고 서버보내고 지우기
        if (!clicked)
        {
            //End the recording
            Microphone.End("");

            //3초보다 작으면 재요청
            if ((int)((Time.time - startRecordingTime)) < 3)
            {
                RecorderError.SetActive(true);
            }
            else
            {
                //Trim the audioclip by the length of the recording
                AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
                float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
                recording.GetData(data, 0);
                recordingNew.SetData(data, 0);
                this.recording = recordingNew;

                audioSource.clip = recording;

                //잘 녹음됐는지 플레이해보기
                //Play recording
                //audioSource.Play();

                // .wav로 변환하기 (음성 모델에 인풋 형식 맞추기 위함)
                filepath = SavWav.Save("myfile", audioSource.clip);
                audioData = File.ReadAllBytes(filepath);

                //debug.text = filepath;
                //debug.text = audioData.ToString();

                // 서버로 음성 데이터 전송
                backendManager.PostAudio(audioData, PlayerPrefs.GetString("x2").FromBase64());
                Loading.SetActive(true);

                // 음성 데이터 삭제
                audioData = null;
                File.Delete(filepath);
                audioSource.clip = null;
            }
        }
    }
}