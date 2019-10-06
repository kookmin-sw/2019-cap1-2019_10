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

using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public class SignupMenu : BaseMenu {
    public VoidDelegate OnSignedUp;
    public VoidDelegate OnCancel;

    private const float LABEL_WIDTH = 110;

    private bool hasFocussed = false;
    private bool signingUp = false;
    public float checkStartTime;
    private int dotNumber = 1;
    private float nextStatusChange;
    private string status = "";
    private string username = "", email = "", password = "", password_confirm = "";

    public GameObject signupObject;
    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField repeatInput;
    public Text statusText;
    
    private void Start() {
        //windowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 210);
        backendManager.OnSignupSuccess += OnSignupSuccess;
        backendManager.OnSignupFailed += OnSignupFailed;
    }

    private void OnSignupFailed(string error) {
        //status = "Signup error: \n\n" + error;
        statusText.text = "Signup error: " + error;
        signingUp = false;
    }

    private void OnSignupSuccess() {
        //status = "Signup successful!";
        statusText.text = "Signup successful!";
        signingUp = false;

        Invoke("FinishSignup", 1.5f);
    }

    private void FinishSignup() {
        if (OnSignedUp != null) {
            OnSignedUp();
        }
        enabled = false;
        signupObject.SetActive(false);
    }

    public void DoSignup() {
        if(usernameInput.text == "")
        {
            statusText.text = "username is empty";
            return;
        }
        if (emailInput.text == "")
        {
            statusText.text = "email is empty";
            return;
        }
        if(passwordInput.text == "")
        {
            statusText.text = "password is empty";
            return;
        }
        if (repeatInput.text == "")
        {
            statusText.text = "please check repeat password";
            return;
        }
        if (passwordInput.text != repeatInput.text)
        {
            statusText.text = "password is not sample";
            return;
        }

        username = usernameInput.text;
        email = emailInput.text;
        password = passwordInput.text;

        Debug.Log(username + " " + email + " " + password);
        if (signingUp) {
            Debug.LogWarning("Already signing up, returning.");
            return;
        }
        backendManager.Signup(username, email, password);
        signingUp = true;
    }

    public void DoCancel()
    {
        enabled = false;
        signupObject.SetActive(false);
        OnCancel();
    }

    private void Update() {
        if(!signingUp) {
            return;
        }

        if (Time.time > nextStatusChange) {
            nextStatusChange = Time.time + 0.5f;
            checkStartTime = Time.time;
            //status = "Signing up";
            statusText.text = "Signing up";
            for (int i = 0; i < dotNumber; i++) {
                //status += ".";
                statusText.text += ".";
            }
            if (++dotNumber > 3) {
                dotNumber = 1;
            }
            if ((float)Time.time - checkStartTime > 5.0f)
            {
                signingUp = false;
                statusText.text = "try again please";
            }
        }
    }
}
