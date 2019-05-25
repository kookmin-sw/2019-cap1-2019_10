﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhoneCamera : BaseMenu
{
    private bool camAvailable;
    private WebCamTexture frontCam;

    Vector3 localScale = new Vector3(1f, 1f, 1f);
    Vector3 localOrient = new Vector3(0, 0, 0);

    public byte[] imageData;

    public float StayTime = 2f;

    //디버깅용
    private Texture defaultBackground;
    private string _SavePath = ".\\MyMoodMusic\\";
    private string path = "";
    int _CaptureCounter = 0;
    //public RawImage background;
    //public AspectRatioFitter fit;

    private void Start()
    {
        //defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        // 사용할 수 있는 카메라를 못찾음
        if(devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        // 사용할 수 있는 카메라 중에 전면 카메라를 사용하고 싶다
        for(int i = 0; i < devices.Length; i++)
        {
            if(devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(frontCam == null)
        {
            Debug.Log("Unable to find front camera");
            return;
        }

        // 카메라 사용
        //frontCam.Play();

        // 이건 그냥 디버깅용으로 안보이니까 카메라 잘 떴는지 확인할려고 UI 텍스쳐로 띄워줌 카메라
        //background.texture = frontCam;

        Debug.Log("find front camera");
        camAvailable = true;

        //StartCoroutine("TakePicture");
    }

    // 사진찍기
    private void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(frontCam.width, frontCam.height);
        snap.SetPixels(frontCam.GetPixels());
        snap.Apply();

        //잘 찍히는지 사진으로 저장해보는 코드
        //_SavePath = pathForDocumentsFile("MyMoodMusic");
        //Debug.Log(_SavePath);
        //System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        //path = _SavePath + _CaptureCounter.ToString() + ".png";
        //++_CaptureCounter;
        //Debug.Log(_CaptureCounter);

        imageData = snap.EncodeToPNG();

        UnityEngine.Object.Destroy(snap);

        backendManager.SendFile("face/", "photo", imageData, "photo.png", "image/png");

        BaymaxGame.instance.photoCheck = true;
    }

    private IEnumerator TakePicture()
    {
        frontCam.Play();
        yield return new WaitForSecondsRealtime(StayTime);
        TakeSnapshot();
        frontCam.Stop();
    }

    public bool OnCamera()
    {
        StartCoroutine("TakePicture");

        return true;
    }

    // 저장경로 찾기
    //public string pathForDocumentsFile(string filename)
    //{
    //    if (Application.platform == RuntimePlatform.IPhonePlayer)
    //    {
    //        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
    //        path = path.Substring(0, path.LastIndexOf('/'));
    //        return Path.Combine(Path.Combine(path, "Documents"), filename);
    //    }
    //    else if (Application.platform == RuntimePlatform.Android)
    //    {
    //        string path = Application.persistentDataPath;
    //        path = path.Substring(0, path.LastIndexOf('/'));
    //        return Path.Combine(path, filename);
    //    }
    //    else
    //    {
    //        string path = Application.dataPath;
    //        path = path.Substring(0, path.LastIndexOf('/'));
    //        return Path.Combine(path, filename);
    //    }
    //}

    //private void Update()
    //{
    //    if (!camAvailable)
    //        return;

    //    // 카메라 스케일이랑 각도를 내가 핸드폰을 돌릴때마다 맞춰서 나올 수 있도록 조정
    //    //float ratio = (float)frontCam.width / (float)frontCam.height;
    //    //fit.aspectRatio = ratio;

    //    //float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
    //    //background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
    //    //localScale.Set(1f, scaleY, 1f);
    //    //background.rectTransform.localScale = localScale;

    //    //int orient = -frontCam.videoRotationAngle;
    //    //background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    //    //localOrient.Set(0, 0, orient);
    //    //background.rectTransform.localEulerAngles = localOrient;
    //}
}
