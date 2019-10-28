using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TestHttpPost : MonoBehaviour
{
    IEnumerator HttpPost(string uri, byte[] bodyData)
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // for WWW
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        WWW www = new WWW(uri, bodyData);
        yield return www;
        Debug.Log("www:" + www.error + "  responsed data len:" + www.bytesDownloaded);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // for UnityWebRequest
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(uri, Encoding.ASCII.GetString(bodyData));
        //UnityWebRequest unityWebRequest = UnityWebRequest.Put(uri, Encoding.ASCII.GetString(bodyData));
        yield return unityWebRequest.Send();


        if (unityWebRequest.isNetworkError)
        {
            Debug.Log("UnityWebRequest:" + unityWebRequest.error);
        }
        else
        {
            Debug.Log("UnityWebRequest: responsed data len:" + unityWebRequest.downloadedBytes);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // for HttpWebRequest
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ServicePointManager.ServerCertificateValidationCallback = delegate
        {
            return true;
        };

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/octet-stream";
        httpWebRequest.ContentLength = bodyData.Length;
        httpWebRequest.BeginGetRequestStream(new AsyncCallback((iar) => {
            try
            {
                using (Stream stream = httpWebRequest.EndGetRequestStream(iar))
                {
                    if (bodyData != null)
                    {
                        stream.Write(bodyData, 0, bodyData.Length);
                    }

                    stream.Close();

                    httpWebRequest.BeginGetResponse(new AsyncCallback((IAsyncResult iarres) => {
                        try
                        {
                            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.EndGetResponse(iarres))
                            {
                                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                                {
                                    Debug.Log("HttpWebRequest: responsed data len:" + streamReader.ReadToEnd().Length);
                                }

                                response.Close();
                            }

                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }

                    }), null);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }), null);
    }

    void Start()
    {
        StartCoroutine(HttpPost("http://ec2-54-180-83-211.ap-northeast-2.compute.amazonaws.com:8000/api/", new byte[] { 61, 61 }));
    }

}
