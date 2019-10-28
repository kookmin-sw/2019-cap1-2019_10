using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhoneCamera : BaseMenu
{
    private bool camAvailable;
    public WebCamTexture frontCam;
    public Texture2D snap;

    Vector3 localScale = new Vector3(1f, 1f, 1f);
    Vector3 localOrient = new Vector3(0, 0, 0);

    public byte[] imageData;

    public float StayTime = 3f;

    //디버깅용
    private Texture defaultBackground;
    private string _SavePath = ".\\MyMoodMusic\\";
    private string path = "";
    int _CaptureCounter = 0;
    public RawImage background;
    public AspectRatioFitter fit;
    public Quaternion baseRotation;

    private void Start()
    {
        //defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        // 사용할 수 있는 카메라를 못찾음
        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        // 사용할 수 있는 카메라 중에 전면 카메라를 사용하고 싶다
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (frontCam == null)
        {
            Debug.Log("Unable to find front camera");
            return;
        }

        // 카메라 사용
        //frontCam.Play();

        // 이건 그냥 디버깅용으로 안보이니까 카메라 잘 떴는지 확인할려고 UI 텍스쳐로 띄워줌 카메라
        //background.texture = frontCam;

        Debug.Log("find front camera");
        //baseRotation = transform.rotation;
        camAvailable = true;

        //StartCoroutine("TakePicture");        
    }

    // 사진찍기
    private void TakeSnapshot()
    {
        snap = new Texture2D(frontCam.width, frontCam.height);
        snap.SetPixels(frontCam.GetPixels());
        //Texture2D snap = background.texture as Texture2D;
        snap.Apply();

        if (Application.platform == RuntimePlatform.Android)
        {
            snap = RotateImage(snap, 90);
        }

        //잘 찍히는지 사진으로 저장해보는 코드
        //_SavePath = pathForDocumentsFile("MyMoodMusic");
        //Debug.Log(_SavePath);
        //System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        //path = _SavePath + _CaptureCounter.ToString() + ".png";
        //++_CaptureCounter;
        //Debug.Log(_CaptureCounter);

        imageData = snap.EncodeToPNG();

        backendManager.PostPhoto(imageData, PlayerPrefs.GetString("x2").FromBase64());

        UnityEngine.Object.Destroy(snap);
        imageData = null;
    }

    // 카메라 켜고 사진찍고 카메라 끄기
    private IEnumerator TakePicture()
    {
        frontCam.Play();
        yield return new WaitForSecondsRealtime(StayTime);
        TakeSnapshot();
        frontCam.Stop();
    }

    public void OnCamera()
    {
        StartCoroutine("TakePicture");
    }


    // 저장경로 찾기
    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }

    private void Update()
    {
        //if (!camAvailable)
        //    return;

        //카메라 스케일이랑 각도를 내가 핸드폰을 돌릴때마다 맞춰서 나올 수 있도록 조정
        //float ratio = (float)frontCam.width / (float)frontCam.height;
        //fit.aspectRatio = ratio;

        //transform.rotation = baseRotation * Quaternion.AngleAxis(frontCam.videoRotationAngle, Vector3.up);

        //int rotAngle = -frontCam.videoRotationAngle;
        //while (rotAngle < 0){ rotAngle += 360;
        //while (rotAngle > 360) rotAngle -= 360;

        //float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
        //background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        //localScale.Set(1f, scaleY, 1f);
        //background.rectTransform.localScale = localScale;

        //int orient = -frontCam.videoRotationAngle;
        //background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        //localOrient.Set(0, 0, orient);
        //background.rectTransform.localEulerAngles = localOrient;
    }


    //이미지 회전시키기 
    public static Texture2D RotateImage(Texture2D originTexture, int angle)
    {
        Texture2D result;
        result = new Texture2D(originTexture.width, originTexture.height);
        Color32[] pix1 = result.GetPixels32();
        Color32[] pix2 = originTexture.GetPixels32();
        int W = originTexture.width;
        int H = originTexture.height;
        int x = 0;
        int y = 0;
        Color32[] pix3 = rotateSquare(pix2, (Math.PI / 180 * (double)angle), originTexture);
        for (int j = 0; j < H; j++)
        {
            for (var i = 0; i < W; i++)
            {
                //pix1[result.width/2 - originTexture.width/2 + x + i + result.width*(result.height/2-originTexture.height/2+j+y)] = pix2[i + j*originTexture.width];
                pix1[result.width / 2 - W / 2 + x + i + result.width * (result.height / 2 - H / 2 + j + y)] = pix3[i + j * W];
            }
        }
        result.SetPixels32(pix1);
        result.Apply();
        return result;
    }

    static Color32[] rotateSquare(Color32[] arr, double phi, Texture2D originTexture)
    {
        int x;
        int y;
        int i;
        int j;
        double sn = Math.Sin(phi);
        double cs = Math.Cos(phi);
        Color32[] arr2 = originTexture.GetPixels32();
        int W = originTexture.width;
        int H = originTexture.height;
        int xc = W / 2;
        int yc = H / 2;
        for (j = 0; j < H; j++)
        {
            for (i = 0; i < W; i++)
            {
                arr2[j * W + i] = new Color32(0, 0, 0, 0);
                x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);
                if ((x > -1) && (x < W) && (y > -1) && (y < H))
                {
                    arr2[j * W + i] = arr[y * W + x];
                }
            }
        }
        return arr2;
    }
}
