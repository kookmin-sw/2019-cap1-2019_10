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
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : BaseMenu {
    public delegate void LoggedIn();
    public LoggedIn HasLoggedIn;

    private const float LABEL_WIDTH = 110;
    private bool loggingIn = false;
    private bool rememberMe = false;
    private bool hasFocussed = false;
    private int dotNumber = 1;
    private float nextStatusChange;
    public float checkStartTime;
    public int checkTry;
    private string status = "";
    public string username = "", password = "";
    public bool isSignup;
    private SignupMenu signupMenu;

    // 로그인 창 메뉴 setting
    public GameObject loginMenu;
    public GameObject loginObject;
    public GameObject signupObject;
    public InputField usernameInput;
    public InputField passwordInput;
    public Toggle rememberToggle;
    public Text statusText;
    public Button loginBtn;
    public Button signupBtn;
    public Button exitBtn;

    private void Start() {
        //windowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150);
        //windowRect = new Rect(Screen.width / 2 - 430, Screen.height / 2 - 500, 500, 300);

        // login을 위한 setting
        loginMenu.SetActive(true);
        loginObject.SetActive(true);
        checkTry = 0;
        backendManager.OnLoggedIn += OnLoggedIn;
        backendManager.OnLoginFailed += OnLoginFailed;

        // 회원가입을 위한 setting
        signupMenu = gameObject.GetComponent<SignupMenu>();
        signupObject.SetActive(false);
        signupMenu.enabled = false;
        signupMenu.OnCancel += OnSignupCancelOrSuccess;
        signupMenu.OnSignedUp += OnSignupCancelOrSuccess;

        isSignup = false;

        //로그인 정보를 기억하고 있으면 자동으로 로그인 정보를 채우기
        if (PlayerPrefs.HasKey("x1")) {
            username = PlayerPrefs.GetString("x2").FromBase64();
            password = PlayerPrefs.GetString("x1").FromBase64();
            usernameInput.ActivateInputField();
            usernameInput.text = username;
            passwordInput.ActivateInputField();
            passwordInput.text = password;
            rememberToggle.isOn = true;
            rememberMe = true;
        }
    }

    private void OnSignupCancelOrSuccess() {
        enabled = true;
        isSignup = true;
        loginObject.SetActive(true);
    }

    //로그인 정보 저장하기
    private void SaveCredentials() {
        PlayerPrefs.SetString("x2", username.ToBase64());
        PlayerPrefs.SetString("x1", password.ToBase64());
    }

    //로그인 정보 삭제
    private void RemoveCredentials() {
        if (PlayerPrefs.HasKey("x1")) {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnLoginFailed(string error) {
        //status = "Login error: " + error;
        statusText.text = "Login error: " + error;
        loggingIn = false;
    }

    private void OnLoggedIn() {
        //status = "Logged in!";
        statusText.text = "Logged in!";
        loggingIn = false;

        //if (rememberMe) {
        //    SaveCredentials();
        //} else {
        //    RemoveCredentials();
        //}
        if (rememberToggle.isOn)
        {
            SaveCredentials();
        }
        else
        {
            RemoveCredentials();
        }

        // 안들어가는데 이거 왜 있는거지
        if (HasLoggedIn != null)
        {
            HasLoggedIn();
        }
    }

    // 서버에 로그인 요청
    public void DoLogin() {
        if(usernameInput.text == "")
        {
            statusText.text = "username is empty";
            return;
        }
        if (passwordInput.text == "")
        {
            statusText.text = "password is empty";
            return;
        }
        username = usernameInput.text;
        password = passwordInput.text;
        if (loggingIn) {
            Debug.LogWarning("Already logging in, returning.");
            return;
        }
        loggingIn = true;
        backendManager.Login(username, password);
        checkStartTime = Time.time;
    }

    public void DoSignup()
    {
        loginObject.SetActive(false);
        signupObject.SetActive(true);
        signupMenu.enabled = true;
    }

    //status에 로그인 시도중임을 표시하기 위함
    private void Update() {
        if(!loggingIn) {
            return;
        }

        if (Time.time > nextStatusChange) {
            nextStatusChange = Time.time + 0.5f;
            
            //status = "Logging in";

            if((float)Time.time - checkStartTime > 20.0f)
            {
                loggingIn = false;
                statusText.text = "Please try again later";
            }
            else
            {
                statusText.text = "Logging in";
                for (int i = 0; i < dotNumber; i++)
                {
                    //status += ".";
                    statusText.text += ".";
                }
                if (++dotNumber > 3)
                {
                    dotNumber = 1;
                }
            }
        }
    }

}
