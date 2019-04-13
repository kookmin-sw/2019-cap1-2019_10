using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture frontCam;
    private Texture defaultBackground;
    private int cameraPermission = 0;

    public RawImage background;
    public AspectRatioFitter fit;

    Vector3 localScale = new Vector3(1f, 1f, 1f);
    Vector3 localOrient = new Vector3(0, 0, 0);

    private string _SavePath = ".\\MyMoodMusic\\";
    int _CaptureCounter = 0;


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

        defaultBackground = background.texture;
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
        // 근데 처음에 스타트하고 퍼미션 받고 돌아야하는데 스타트에 들어있어서 퍼미션이 바로 적용이 안됨
        frontCam.Play();

        // 이건 그냥 디버깅용으로 안보이니까 카메라 잘 떴는지 확인할려고 UI 텍스쳐로 띄워줌 카메라
        background.texture = frontCam;

        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        // 카메라 스케일이랑 각도를 내가 핸드폰을 돌릴때마다 맞춰서 나올 수 있도록 조정
        float ratio = (float)frontCam.width / (float)frontCam.height;
        fit.aspectRatio = ratio;

        float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
        //background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        localScale.Set(1f, scaleY, 1f);
        background.rectTransform.localScale = localScale;

        int orient = -frontCam.videoRotationAngle;
        //background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        localOrient.Set(0, 0, orient);
        background.rectTransform.localEulerAngles = localOrient;
    }

    private void OnGUI()
    {
        // 사진 찍을 버튼 생성
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            TakeSnapshot();
    }

    // 저장경로 찾기..
    public string pathForDocumentsFile(string filename)
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if(Application.platform == RuntimePlatform.Android)
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

    // 사진찍기
    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(frontCam.width, frontCam.height);
        snap.SetPixels(frontCam.GetPixels());
        snap.Apply();

        _SavePath = pathForDocumentsFile("MyMoodMusic");
        Debug.Log(_SavePath);
        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        
        ++_CaptureCounter;
        Debug.Log(_CaptureCounter);
    }
}
