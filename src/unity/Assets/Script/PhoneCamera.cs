using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture frontCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    Vector3 localScale = new Vector3(1f, 1f, 1f);
    Vector3 localOrient = new Vector3(0, 0, 0);

    private string _SavePath = ".\\MyMoodMusic\\";
    private string path = "";
    int _CaptureCounter = 0;
    public byte[] bytes;

    public string url = "";
    public float StayTime = 2f;

    public Text txt;

    private void Start()
    {

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
        frontCam.Play();

        // 이건 그냥 디버깅용으로 안보이니까 카메라 잘 떴는지 확인할려고 UI 텍스쳐로 띄워줌 카메라
        background.texture = frontCam;

        camAvailable = true;

        StartCoroutine("TakePicture");
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

    //private void OnGUI()
    //{
    //    // 사진 찍을 버튼 생성
    //    if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
    //        TakeSnapshot();
    //}

    //public void OnClickSaveButton()
    //{
    //    TakeSnapshot();
    //}

    //// 저장경로 찾기..
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

    //public void onClickSendButton()
    //{
    //    StartCoroutine(ServerThrows());
    //}

    // 사진찍기
    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(frontCam.width, frontCam.height);
        snap.SetPixels(frontCam.GetPixels());
        snap.Apply();

        //_SavePath = pathForDocumentsFile("MyMoodMusic");
        //Debug.Log(_SavePath);
        //System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());

        //bytes = snap.EncodeToPNG();
        bytes = snap.EncodeToJPG();

        txt.text = "찰칵";

        UnityEngine.Object.Destroy(snap);

        //path = _SavePath + _CaptureCounter.ToString() + ".png";

        //++_CaptureCounter;
        //Debug.Log(_CaptureCounter);

        StartCoroutine(ServerThrows());
    }

    IEnumerator TakePicture()
    {
        TakeSnapshot();
        yield return new WaitForSecondsRealtime(StayTime);
        TakeSnapshot();
        
        frontCam.Stop();
        camAvailable = false;
        Scenario.instance.isEnd = false;
    }

    IEnumerator ServerThrows()
    {
        //string imageAsJson = File.ReadAllText(path);
        //byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(imageAsJson);
        //UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/", "POST");
        //www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //www.chunkedTransfer = false;
        //www.SetRequestHeader("Content-Type", "application/json");

        //yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    GetResponse(www);
        //}

        //WWWForm form = new WWWForm();
        //form.AddField("frameCount", Time.frameCount.ToString());
        //form.AddBinaryData("fileUpload", bytes);

        //WWW w = new WWW("http://127.0.0.1:8000/", form);
        //yield return w;

        //if (w.error != null)
        //{
        //    Debug.Log(w.error);
        //}
        //else
        //{
        //    Debug.Log("Finished Uploading Screenshot");
        //}

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("photo",bytes, "byte[]"));
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        //UnityWebRequest www = UnityWebRequest.Post(url, null, bytes);
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!" + www.downloadHandler.text);
        }
        txt.text = "보냄";
        www.Dispose();
    }

    private void GetResponse(UnityWebRequest www)
    {
        throw new NotImplementedException();
    }
}
