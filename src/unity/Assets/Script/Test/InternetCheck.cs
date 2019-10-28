using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetCheck : MonoBehaviour
{
    private const bool allowCarrierDataNetwork = false;
    private const string pingAddress = "8.8.8.8"; // Google Public DNS server
    private const float waitingTime = 2.0f;

    private Ping ping;
    private float pingStartTime;

    public void Start()
    {
        bool internetPossiblyAvailable;
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                internetPossiblyAvailable = true;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                internetPossiblyAvailable = true;
                break;
            default:
                internetPossiblyAvailable = false;
                break;
        }
        if (!internetPossiblyAvailable)
        {
            InternetIsNotAvailable();
            return;
        }
        ping = new Ping(pingAddress);
        pingStartTime = Time.time;
    }

    public void Update()
    {
        if (ping != null)
        {
            bool stopCheck = true;
            if (ping.isDone)
            {
                if (ping.time >= 0)
                    InternetAvailable();
                else
                    InternetIsNotAvailable();
            }
            else if (Time.time - pingStartTime < waitingTime)
                stopCheck = false;
            else
                InternetIsNotAvailable();
            if (stopCheck)
                ping = null;
        }
    }

    private void InternetIsNotAvailable()
    {
        Debug.Log("No Internet :(");
    }

    private void InternetAvailable()
    {
        Debug.Log("Internet is available! ;)");
    }
}
