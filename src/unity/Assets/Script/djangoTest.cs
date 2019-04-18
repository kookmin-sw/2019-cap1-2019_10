using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class djangoTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:8000/");
        yield return www.SendWebRequest();

        //UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/", formData);
        //www.chunkedTransfer = false;
        //yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield break;
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

        string SetCookie = www.GetRequestHeader("set-cookie");
        Debug.Log(SetCookie);
        Regex rxCookie = new Regex("csrftoken=(?<csrf_token>.{64});");
        MatchCollection cookieMatches = rxCookie.Matches(SetCookie);
        string csrfCookie = cookieMatches[0].Groups["csrf_token"].Value;

        string wwwHtml = www.downloadHandler.text;
        Regex rxMiddleware = new Regex("name='csrfmiddlewaretoken' value='(?<csrf_token>.{64})'");
        MatchCollection middlewareMatches = rxMiddleware.Matches(wwwHtml);
        string csrfMiddlewareToken = middlewareMatches[0].Groups["csrf_token"].Value;

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest dowww = UnityWebRequest.Post("http://127.0.0.1:8000/", formData);

        dowww.SetRequestHeader("test", "http://127.0.0.1:8000/");
        Debug.Log(dowww.GetRequestHeader("cookie"));
        dowww.SetRequestHeader("cookie", "csrftoken=" + csrfCookie);
        Debug.Log(dowww.GetRequestHeader("cookie"));
        dowww.SetRequestHeader("X-CSRFToken", csrfCookie);

        yield return dowww.SendWebRequest();

        Debug.Log(dowww.downloadHandler.text);
    }
}
