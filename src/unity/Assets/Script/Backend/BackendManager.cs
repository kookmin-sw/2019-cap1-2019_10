﻿// Copyright (c) 2015 Eamon Woortman
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#define UNITY_4_PLUS
#define UNITY_5_PLUS

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
    #define UNITY_4_X
    #undef UNITY_5_PLUS
#elif UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
    #define UNITY_5_X
#endif

using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;

//---- Public Enums ----//
public enum ResponseType {
    /// <summary>
    /// ClientError, the client could not perform the request(eg. could not reach destination host)
    /// </summary>
    ClientError,
    /// <summary>
    /// PageNotFound, the page could not be found(invalid url)
    /// </summary>
    PageNotFound,
    /// <summary>
    /// The server returned an error regarding the request, which can be invalid post data or invalid authentication for example
    /// </summary>
    RequestError,
    /// <summary>
    /// The server returned an error but the response body could not be parsed
    /// </summary>
    ParseError,
    /// <summary>
    /// Success, if the server returns content it will be parsed into a JObject or JArray.
    /// </summary>
    Success,
}

/// <summary>
/// The type of the request(sets the HTTP method)
/// </summary>
public enum RequestType {
    /// <summary>
    /// GET
    /// </summary>
    Get,
    /// <summary>
    /// POST
    /// </summary>
    Post,
    /// <summary>
    /// PUT
    /// </summary>
    Put,
    /// <summary>
    /// DELETE
    /// </summary>
    Delete
}

public partial class BackendManager : MonoBehaviour {

    //---- Public Delegates ----//
    /// <summary>
    /// The response delegate
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="jsonResponse">the json object of the response, this can be null when no content is returned(eg. HTTP 204)</param>
    /// <param name="callee">the name of the method doing the request(used for testing)</param>
    public delegate void RequestResponseDelegate(ResponseType responseType, JToken jsonResponse, string callee);
    public delegate void FileRequestResponseDelegate(ResponseType responseType, string[] strResponse, JToken jsonResponse, string callee);


    //---- Public Properties ----//
    public string BackendUrl {
        get {
            return UseProduction ? ProductionUrl : DevelopmentUrl;
        }
    }

    //---- URLS ----//
    public bool UseProduction = false;
    public bool Secure;
    public string ProductionUrl = "http://203.246.113.177:8000/";
    public string DevelopmentUrl = "http://localhost:8000/api/";


    //---- Private Methods ----//

    /// <summary>Performs a request to the backend.</summary>
    /// <param name="type">The request type(Get, Post, Update, Delete)</param>
    /// <param name="command">Command that is pasted after the url to backend. For example: "localhost:8000/api/" + command</param>
    /// <param name="wwwForm">A WWWForm to send with the request</param>
    /// <param name="onResponse">A callback which will be called when we retrieve the response</param>
    /// <param name="authToken">An optional authToken which, when set will be put in the Authorization header</param>
    public void Send(RequestType type, string command, WWWForm wwwForm, RequestResponseDelegate onResponse = null, string authToken = "")
    {
        WWW request;
#if UNITY_5_PLUS
        Dictionary<string, string> headers;
#else
        Hashtable headers;
#endif
        byte[] postData;
        string url = BackendUrl + command;

        if (Secure)
        {
            url = url.Replace("http", "https");
        }

        if (wwwForm == null)
        {
            wwwForm = new WWWForm();
            postData = new byte[] { 1 };
        }
        else
        {
            postData = wwwForm.data;
            //Debug.Log(postData.Length);
        }

        headers = wwwForm.headers;

        //make sure we get a json response
        headers.Add("Accept", "application/json");

        //also add the correct request method
        headers.Add("X-UNITY-METHOD", type.ToString().ToUpper());

        //also, add the authentication token, if we have one
        if (authToken != "")
        {
            //for more information about token authentication, see: http://www.django-rest-framework.org/api-guide/authentication/#tokenauthentication
            headers.Add("Authorization", "Token " + authToken);
        }
        request = new WWW(url, postData, headers);

        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        string callee = stackTrace.GetFrame(1).GetMethod().Name;
        StartCoroutine(HandleRequest(request, onResponse, callee));
    }

    private IEnumerator HandleRequest(WWW request, RequestResponseDelegate onResponse, string callee)
    {
        //Wait till request is done
        while (true)
        {
            if (request.isDone)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(request.error);

        if (request.text == "{\"username\":[\"해당 사용자 이름은 이미 존재합니다.\"]}")
        {
            onResponse(ResponseType.ClientError, request.text, callee);

            yield break;
        }
        Debug.Log(request.text);

        if (request.text == "{\"username\":[\"해당 사용자 이름은 이미 존재합니다.\"]}")
        {
            onResponse(ResponseType.ClientError, request.text, callee);

            yield break;
        }
        //Debug.Log(request.text);

        //catch proper client errors(eg. can't reach the server)
        if (!String.IsNullOrEmpty(request.error))
        {
            if (onResponse != null)
            {
                onResponse(ResponseType.ClientError, null, callee);
            }
            yield break;
        }
        int statusCode = 200;

        if (request.responseHeaders.ContainsKey("REAL_STATUS"))
        {
            string status = request.responseHeaders["REAL_STATUS"];
            statusCode = int.Parse(status.Split(' ')[0]);
        }
        //if any other error occurred(probably 4xx range), see http://www.django-rest-framework.org/api-guide/status-codes/
        bool responseSuccessful = (statusCode >= 200 && statusCode <= 206);
        JToken responseObj = null;

        try
        {
            if (request.text == "Face is not detected") throw new Exception();

            if (request.text.StartsWith("["))
            {
                responseObj = JArray.Parse(request.text);
            }
            else if(request.text.StartsWith("\"["))
            {
                responseObj = request.text;
            }
            else
            {
                responseObj = JObject.Parse(request.text);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(statusCode);
            if (onResponse != null)
            {
                if (!responseSuccessful)
                {
                    if (statusCode == 404)
                    {
                        //404's should not be treated as unparsable
                        Debug.LogWarning("Page not found: " + request.url);
                        onResponse(ResponseType.PageNotFound, null, callee);
                    }
                    else
                    {
                        Debug.Log("Could not parse the response, request.text=" + request.text);
                        Debug.Log("Exception=" + ex.ToString());
                        onResponse(ResponseType.ParseError, null, callee);
                    }
                }
                else
                {
                    if (request.text == "")
                    {
                        onResponse(ResponseType.Success, null, callee);
                    }
                    else
                    {
                        Debug.Log("Could not parse the response, request.text=" + request.text);
                        Debug.Log("Exception=" + ex.ToString());
                        onResponse(ResponseType.ParseError, null, callee);
                    }
                }
            }
            yield break;
        }

        if (!responseSuccessful)
        {
            if (onResponse != null)
            {
                onResponse(ResponseType.RequestError, responseObj, callee);
            }
            yield break;
        }

        //deal with successful responses
        if (onResponse != null)
        {
            onResponse(ResponseType.Success, responseObj, callee);
        }
    }

    // UnityWebRequest로 하는 통신
    public void SendFile(RequestType type, string command, WWWForm wwwForm, FileRequestResponseDelegate onResponse = null, string authToken = "", bool check = false)
    {
        UnityWebRequest request;

        byte[] postData;
        string url = BackendUrl + command;

        if (Secure)
        {
            url = url.Replace("http", "https");
        }

        //if (wwwForm == null)
        //{
        //    wwwForm = new WWWForm();
        //    postData = new byte[] { 1 };
        //    request = UnityWebRequest.Get(url);
        //    check = false;
        //}
        //else
        //{
        postData = wwwForm.data;
        Debug.Log("Post");
        request = UnityWebRequest.Post(url, wwwForm);
        //}

        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        string callee = stackTrace.GetFrame(1).GetMethod().Name;
        StartCoroutine(HandleFileRequest(request, onResponse, callee, check));
    }

    private IEnumerator HandleFileRequest(UnityWebRequest request, FileRequestResponseDelegate onResponse, string callee, bool check)
    {
        //request.SendWebRequest();
        Debug.Log("request");

        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            //Debug.Log(request.ToString());
        }
        else
        {
            Debug.Log("된다구..");
        }

        //StartCoroutine(RequestToken(request));

        //Wait till request is done
        //while (true)
        //{
        //    if (request.isDone)
        //    {
        //        break;
        //    }
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return request.SendWebRequest();
        Debug.Log("downloadhandler\n" + request.downloadHandler.text);

        //catch proper client errors(eg. can't reach the server)
        if (!String.IsNullOrEmpty(request.error))
        {
            if (onResponse != null)
            {
                onResponse(ResponseType.ClientError, null, null, callee);
            }
            yield break;
        }
        int statusCode = 200;

        //if any other error occurred(probably 4xx range), see http://www.django-rest-framework.org/api-guide/status-codes/
        bool responseSuccessful = (statusCode >= 200 && statusCode <= 206);

        string emotion = null;
        string[] splitEmotion = { };
        JToken responseObj = null;

        try
        {
            emotion = request.downloadHandler.text;
            Debug.Log(emotion);
            Debug.Log(statusCode);

            // 서버 에러문 수신시 exception처리
            if (emotion == "please try again") throw new Exception();

            //Debug.Log(request.downloadHandler.text);
            if (check)
            {
                //emotion = emotion.Replace("", "\n");
                // json 형태 아닐때 split하기
                char[] delimiterChars = { ' ', ',', '[', ']', '\'', '\'', '\t', '\n', '\0'};
                splitEmotion = emotion.Split(delimiterChars);
            }
            else
            {
                //string finalJsonStr = emotion.Replace("\\", "");
                //finalJsonStr = finalJsonStr.Replace("[", "");
                //finalJsonStr = finalJsonStr.Substring(1, finalJsonStr.Length - 3);

                //Debug.Log(finalJsonStr);

                // json형태 파일 파싱
                if (emotion.StartsWith("["))
                {
                    responseObj = JArray.Parse(emotion);
                    Debug.Log("array : " + responseObj);
                }
                else
                {
                    responseObj = JObject.Parse(emotion);
                    Debug.Log("object : " + responseObj);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(statusCode);
            if (onResponse != null)
            {
                if (!responseSuccessful)
                {
                    if (statusCode == 404)
                    {
                        //404's should not be treated as unparsable
                        Debug.LogWarning("Page not found: " + request.url);
                        onResponse(ResponseType.PageNotFound, null, null, callee);
                    }
                    else
                    {
                        Debug.Log("Could not parse the response");
                        Debug.Log("Exception=" + ex.ToString());
                        onResponse(ResponseType.ParseError, null, null, callee);
                    }
                }
                else
                {
                    if (request.downloadHandler.text == "")
                    {
                        onResponse(ResponseType.Success, null, null, callee);
                    }
                    else
                    {
                        Debug.Log(emotion);
                        Debug.Log("Could not parse the response");
                        Debug.Log("Exception=" + ex.ToString());
                        onResponse(ResponseType.ParseError, null, null, callee);
                    }
                }
            }
            yield break;
        }

        //byte[] bytes = request.downloadHandler.data;
        //for (int i = 0; i < bytes.Length; i++) print(bytes[i]);

        //string[] emotions = { };
        //int cnt = 0;
        //for (int i = 0; i < splitEmotion.Length; i++)
        //{
        //    if (splitEmotion[i] != "")
        //    {
        //        Debug.Log(splitEmotion[i]);
        //    }
        //}

        //for(int i = 0; i<emotions.Length; i++)
        //{
        //    Debug.Log(emotions[i]);
        //}

        if (!responseSuccessful)
        {
            if (onResponse != null)
            {
                onResponse(ResponseType.RequestError, splitEmotion, null, callee);
            }
            yield break;
        }

        //deal with successful responses
        if (onResponse != null)
        {
            onResponse(ResponseType.Success, splitEmotion, responseObj, callee);
        }


    }


    public IEnumerator RequestToken(UnityWebRequest www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log("downloadedBytes : " + www.downloadedBytes);
            Debug.Log("certificateHandler : " + www.certificateHandler);
            Debug.Log("chunkedTransfer : " + www.chunkedTransfer);
            Debug.Log("downloadHandler : " + www.downloadHandler);
            Debug.Log("downloadProgress : " + www.downloadProgress);
            Debug.Log("isDone : " + www.isDone);
            Debug.Log("isNetworkError : " + www.isNetworkError);
            Debug.Log("method : " + www.method);
            Debug.Log("redirectLimit : " + www.redirectLimit);
            Debug.Log("responseCode : " + www.responseCode);
            Debug.Log("uploadedBytes : " + www.uploadedBytes);
            Debug.Log("useHttpContinue : " + www.useHttpContinue);
        }
        else
        {
            Debug.Log("Error" + www.error);
        }
        var p = www.downloadHandler.data;
        Debug.Log("Access token: " + p);
    }
}
    

