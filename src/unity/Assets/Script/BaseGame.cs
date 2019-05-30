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
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class BaseGame : MonoBehaviour
{
    protected BackendManager backendManager;
    protected bool isLoggedIn { get; private set; }
    protected bool isStarted { get; set; }

    //private SavegameMenu saveMenu;
    private LoginMenu loginMenu;

    protected AudioRecorder audioRecorder;
    protected PhoneCamera phoneCamera;

    public GameObject loginObject;
    public GameObject startButton;
    public GameObject SideMenu;
    public GameObject noticeImage;

    public List<Result> results;
    public int checkAllReusltResquestTime;

    protected virtual void Awake() {
        if (loginMenu == null) {
            loginMenu = gameObject.GetOrCreateComponent<LoginMenu>();
        }
        if(audioRecorder == null)
        {
            audioRecorder = gameObject.GetOrCreateComponent<AudioRecorder>();
        }
        if (phoneCamera == null)
        {
            phoneCamera = gameObject.GetOrCreateComponent<PhoneCamera>();
        }
        if (backendManager == null) {
            backendManager = gameObject.GetOrCreateComponent<BackendManager>();
        }
    }

    protected virtual void Start() {
        isLoggedIn = false;
        isStarted = false;
        startButton.SetActive(false);

        //saveMenu.enabled = false;
        //audioRecorder.enabled = false;

        backendManager.OnLoggedIn += OnLoggedIn;
        backendManager.OnAllResultLoaded += OnAllResultLoaded;
        backendManager.OnAllResultLoadedFailed += OnAllResultLoadedFailed;
        //saveMenu.OnSaveButtonPressed += OnSaveButtonPressed;
        //saveMenu.OnLoadButtonPressed += OnLoadButtonPressed;
    }

    protected bool CanClick() {
        foreach (BaseMenu menu in FindObjectsOfType<BaseMenu>()) {
            if (menu.IsMouseOver()) {
                return false;
            }
        }
        return true;
    }

    protected virtual void DisableLoginMenu() {
        loginObject.SetActive(false);
        loginMenu.enabled = false;
        startButton.SetActive(true);
        SideMenu.SetActive(true);
        noticeImage.SetActive(true);
        audioRecorder.enabled = true;
        phoneCamera.enabled = true;
        isLoggedIn = true;

        if (!loginMenu.isSignup) GetAllResult();
    }

    public void ExitLogin()
    {
        if (PlayerPrefs.HasKey("x1"))
        {
            PlayerPrefs.DeleteAll();
        }
        loginObject.SetActive(false);
        loginMenu.enabled = false;
        startButton.SetActive(true);
        SideMenu.SetActive(true);
        noticeImage.SetActive(true);
        audioRecorder.enabled = true;
        phoneCamera.enabled = true;
        isLoggedIn = false;
    }

    protected virtual bool IsMouseOverMenu() {
        return loginMenu.IsMouseOver();
    }

    private void OnLoggedIn() {
        Invoke("DisableLoginMenu", 1.0f);
    }

    public void GetAllResult()
    {
        backendManager.GetAllResult(PlayerPrefs.GetString("x2").FromBase64());
    }

    private void OnAllResultLoaded(List<Result> results)
    {
        this.results = results;
    }

    private void OnAllResultLoadedFailed()
    {
        checkAllReusltResquestTime++;
        if(checkAllReusltResquestTime < 3)
        {
            GetAllResult();
        }

        Debug.Log("Get All Result Failed" + checkAllReusltResquestTime);
    }
}
