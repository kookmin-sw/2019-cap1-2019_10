// Copyright (c) 2015 Eamon Woortman
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

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class BackendManager {
    public Text text;

    public delegate void LoginFailed(string errorMsg);
    public delegate void LoggedIn();
    public LoggedIn OnLoggedIn;
    public LoginFailed OnLoginFailed;

    public delegate void SignupFailed(string errorMsg);
    public delegate void SignupSuccess();
    public SignupSuccess OnSignupSuccess;
    public SignupFailed OnSignupFailed;

    public delegate void ScoresLoaded(List<Score> scores);
    public delegate void ScoreLoadedFailed(string errorMsg);
    public ScoresLoaded OnScoresLoaded;
    public ScoreLoadedFailed OnScoreLoadedFailed;

    public delegate void PostScoreSucces();
    public delegate void PostScoreFailed(string errorMsg);
    public PostScoreSucces OnPostScoreSucces;
    public PostScoreFailed OnPostScoreFailed;

    public delegate void PostPhotoSuccess(string[] photos);
    public delegate void PostPhotoFailed();
    public PostPhotoSuccess OnPostPhotoSuccess;
    public PostPhotoFailed OnPostPhotoFailed;

    public delegate void PostAudioSuccess(string[] audios);
    public delegate void PostAudioFailed();
    public PostAudioSuccess OnPostAudioSuccess;
    public PostAudioFailed OnPostAudioFailed;

    public delegate void ResultLoaded(List<Result> results);
    public delegate void ResultLoadedFailed();
    public ResultLoaded OnResultLoaded;
    public ResultLoadedFailed OnResultLoadedFailed;

    //public delegate void AllResultLoaded(List<Result> results);
    public delegate void AllResultLoaded(List<Result> results);
    public delegate void AllResultLoadedFailed();
    public AllResultLoaded OnAllResultLoaded;
    public AllResultLoadedFailed OnAllResultLoadedFailed;

    // the authentication token will be set when a user has logged in
    private string authenticationToken = "";

    static JsonSerializerSettings settings = new JsonSerializerSettings()
    {

        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    /// <summary>
    /// Does a POST request to the backend, trying to get an authentication token. On succes, it will save the auth token for further use. On success, the OnLoggedIn
    /// delegate will be called. On fail, the OnLoginFailed delegate will be called.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void Login(string username, string password) {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        Send(RequestType.Post, "getauthtoken", form, OnLoginResponse);
    }

    private void OnLoginResponse(ResponseType responseType, JToken responseData, string callee) {
        if (responseType == ResponseType.Success) {
            authenticationToken = responseData.Value<string>("token");
            if (OnLoggedIn != null) {
                OnLoggedIn();
            }
        } else if (responseType == ResponseType.ClientError) {
            if (OnLoginFailed != null) {
                OnLoginFailed("Could not reach the server. Please try again later.");
            }
        } else {
            JToken fieldToken = responseData["non_field_errors"];
            if (fieldToken == null || !fieldToken.HasValues) {
                if (OnLoginFailed != null) {
                    OnLoginFailed("Login failed: unknown error.");
                }
            } else {
                string errors = "";
                JToken[] fieldValidationErrors = fieldToken.Values().ToArray();
                foreach (JToken validationError in fieldValidationErrors) {
                    errors += validationError.Value<string>();
                }
                if (OnLoginFailed != null) {
                    OnLoginFailed("Login failed: " + errors);
                }
            }
        }
    }


    /// <summary>
    /// Does a POST request to the backend, trying to get an authentication token. On succes, it will save the auth token for further use. On success, the OnLoggedIn
    /// delegate will be called. On fail, the OnLoginFailed delegate will be called.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public void Signup(string username, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        Send(RequestType.Post, "user", form, OnSignupResponse);
    }

    private void OnSignupResponse(ResponseType responseType, JToken responseData, string callee)
    {
        if (responseType == ResponseType.Success)
        {
            if (OnSignupSuccess != null)
            {
                OnSignupSuccess();
            }
        }
        else if (responseType == ResponseType.ClientError)
        {
            if (OnSignupFailed != null)
            {
                if(responseData != null)
                {
                    OnSignupFailed("username already exist");
                }
                else
                {
                    OnSignupFailed("Could not reach the server. Please try again later.");
                }
            }
        }
        else if (responseType == ResponseType.RequestError)
        {
            string errors = "";
            JObject obj = (JObject)responseData;
            foreach (KeyValuePair<string, JToken> pair in obj)
            {
                errors += "[" + pair.Key + "] ";
                foreach (string errStr in pair.Value)
                {
                    errors += errStr;
                }
                errors += '\n';
            }
            if (OnSignupFailed != null)
            {
                OnSignupFailed(errors);
            }
        }
    }

    /// <summary>
    /// Does a GET request at the backend, getting you all scores. When succesfull, the OnScoresLoaded delegate will be called. When failing, the OnScoresLoadedFailed delegate will be called.
    /// </summary>
    public void GetAllScores() {
        Send(RequestType.Get, "score", null, OnGetAllScoresResponse, authenticationToken);
    }

    private void OnGetAllScoresResponse(ResponseType responseType, JToken responseData, string callee) {
        if (responseType == ResponseType.Success) {
            if (OnScoresLoaded != null) {
                OnScoresLoaded(JsonConvert.DeserializeObject<List<Score>>(responseData.ToString()));
            }
        } else {
            if (OnScoreLoadedFailed != null) {
                OnScoreLoadedFailed("Could not reach the server. Please try again later.");
            }
        }
    }

    /// <summary>
    /// Does a POST request to the backend, containing the score of the player.
    /// </summary>
    /// <param name="score"></param>
    public void PostScore(int score) {
        WWWForm form = new WWWForm();
        form.AddField("score", score);
        Send(RequestType.Post, "score", form, OnPostScoreResponse, authenticationToken);
    }

    private void OnPostScoreResponse(ResponseType responseType, JToken responseData, string callee) {
        if (responseType == ResponseType.Success) {
            if (OnPostScoreSucces != null) {
                OnPostScoreSucces();
            }
        } else if (responseType == ResponseType.ClientError) {
            if (OnPostScoreFailed != null) {
                OnPostScoreFailed("Could not reach the server. Please try again later.");
            }
        } else {
            if (OnPostScoreFailed != null) {
                OnPostScoreFailed("Request failed: " + responseType + " - " + responseData["detail"]);
            }
        }
    }

    public void GetResult(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("recommand", id);
        byte[] postData = { 1 };
        form.AddBinaryData("recommand", postData);
        Send(RequestType.Post, "recommand", form, OnGetResultResponse, authenticationToken);
    }

    private void OnGetResultResponse(ResponseType responseType, JToken responseJson, string callee)
    {
        if (responseType == ResponseType.Success)
        {
            if (OnResultLoaded != null)
            {
                //Result results = JsonConvert.DeserializeObject<Result>(responseJson.ToString());
                Debug.Log(responseJson);
                //Debug.Log(JsonConvert.DeserializeObject<List<Result>>(responseJson.ToString(), settings));
                OnResultLoaded(JsonConvert.DeserializeObject<List<Result>>(responseJson.ToString(), settings));
                //OnResultLoaded(JsonConvert.DeserializeObject<List<Result>>(responseJson.ToString()));
            }
        }
        else if (responseType == ResponseType.ClientError)
        {
            if (OnResultLoadedFailed != null)
            {
                OnPostScoreFailed("Could not reach the server. Please try again later.");
            }
        }
        else
        {
            if (OnResultLoadedFailed != null)
            {
                OnResultLoadedFailed();
            }
        }
    }

    public void GetAllResult(string id)
    {
        //Debug.Log(imageData.Length + " " + id);
        WWWForm form = new WWWForm();
        form.AddField("result", id);
        byte[] postData = { 1 };
        form.AddBinaryData("temp", postData);
        Send(RequestType.Post, "result", form, OnGetAllResultResponse, authenticationToken);
    }
    private void OnGetAllResultResponse(ResponseType responseType, JToken responseJson, string callee)
    {
        if (responseType == ResponseType.Success)
        {
            if (OnAllResultLoaded != null)
            {
                //Result results = JsonConvert.DeserializeObject<Result>(responseJson.ToString());
                Debug.Log(responseJson);
                OnAllResultLoaded(JsonConvert.DeserializeObject<List<Result>>(responseJson.ToString(), settings));
                //OnResultLoaded(JsonConvert.DeserializeObject<List<Result>>(responseJson.ToString()));
            }
        }
        else if (responseType == ResponseType.ClientError)
        {
            if (OnAllResultLoadedFailed != null)
            {
                OnPostScoreFailed("Could not reach the server. Please try again later.");
            }
        }
        else
        {
            if (OnAllResultLoadedFailed != null)
            {
                OnAllResultLoadedFailed();
            }
        }
    }

    public void PostPhoto(byte[] imageData, string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("photo", id);
        form.AddBinaryData("photo", imageData, "photo.png", "image/png");
        Send(RequestType.Post, "face", form, OnPostPhotoResponse);
    }

    private void OnPostPhotoResponse(ResponseType responseType, JToken responseJson, string callee)
    {
        if (responseType == ResponseType.Success)
        {
            if (OnPostPhotoSuccess != null)
            {
                //Debug.Log("success");
                Debug.Log(responseJson);
                string responseData = responseJson.ToString();
                char[] delimiterChars = { ' ', ',', '[', ']', '\'', '\'', '\t', '\n', '\0' };
                string[] photoResult = responseData.Split(delimiterChars);
                Debug.Log(photoResult);
                OnPostPhotoSuccess(photoResult);
            }
        }
        else if (responseType == ResponseType.ClientError)
        {
            if (OnPostPhotoFailed != null)
            {
                OnPostScoreFailed("Could not reach the server. Please try again later.");
            }
        }
        else
        {
            if (OnPostPhotoFailed != null)
            {
                text.text = "failed";
                //OnScoreLoadedFailed("Could not reach the server. Please try again later.");
                OnPostPhotoFailed();
            }
        }
    }

    //public void PostPhoto(byte[] imageData, string id)
    //{
    //    //Debug.Log(imageData.Length + " " + id);
    //    WWWForm form = new WWWForm();
    //    form.AddField("photo", id);
    //    form.AddBinaryData("photo", imageData, "photo.png", "image/png");
    //    SendFile(RequestType.Post, "face", form, OnPostPhotoResponse, authenticationToken, true);
    //}

    //private void OnPostPhotoResponse(ResponseType responseType, string[] responseData, JToken responseJson, string callee)
    //{
    //    if (responseType == ResponseType.Success)
    //    {
    //        if (OnPostPhotoSuccess != null)
    //        {
    //            text.text = responseData[0];
    //            OnPostPhotoSuccess(responseData);
    //        }
    //    }
    //    else
    //    {
    //        if (OnPostPhotoFailed != null)
    //        {
    //            text.text = "failed";
    //            //OnScoreLoadedFailed("Could not reach the server. Please try again later.");
    //            OnPostPhotoFailed();
    //        }
    //    }
    //}

    public void PostAudio(byte[] audioData, string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("audio", id);
        form.AddBinaryData("audio", audioData, "myfile.wav", "audio/wav");
        Send(RequestType.Post, "speech", form, OnPostAudioResponse, authenticationToken);
    }

    private void OnPostAudioResponse(ResponseType responseType, JToken responseJson, string callee)
    {
        if (responseType == ResponseType.Success)
        {
            if (OnPostAudioSuccess != null)
            {
                string result = JsonConvert.DeserializeObject(responseJson.ToString(), settings).ToString();
                Debug.Log(result);
                List<string> test = new List<string>();
                test.Add(result);
                string[] test_arr = test.ToArray();
                OnPostAudioSuccess(test_arr);
            }
        }
        else if (responseType == ResponseType.ClientError)
        {
            if (OnPostAudioFailed != null)
            {
                OnPostScoreFailed("Could not reach the server. Please try again later.");
            }
        }
        else
        {
            if (OnPostAudioFailed != null)
            {
                //OnScoreLoadedFailed("Could not reach the server. Please try again later.");
                OnPostAudioFailed();
            }
        }
    }

    /// <summary>
    /// Helper method which will check and fill the given string[] array, if the given JToken has the given key
    /// </summary>
    /// <param name="jsonObject"></param>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    private bool ContainsSubfield(JToken jsonObject, string key, out string[] values) {
        JToken fieldToken = jsonObject[key];
        values = new string[0];

        if (fieldToken == null || !fieldToken.HasValues) {
            return true;
        }

        values = fieldToken.Values().ToArray().Select(token => token.Value<string>()).ToArray();
        return values.Length == 0;
    }
}
