using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MySystem
{
    public class Thumbnail : MonoBehaviour

    {
        public int textureWidth = 400;
        public int textureHeight = 400;

        public RawImage textureDisplayer;

        void Start()
        {
            string url = "https://youtu.be/-e7gEaTtiHA";
            StartCoroutine(LoadImg(YoutubeImage(url)));

        }

        void displayImage(Texture2D imgToDisp)
        {
            //Resize Image
            textureDisplayer.GetComponent<RectTransform>().sizeDelta = new Vector2(textureWidth, textureHeight);
            textureDisplayer.texture = imgToDisp;
        }


        IEnumerator LoadImg(string url)
        {
            Debug.Log(url);
            yield return null;
            WWW www = new WWW(url);
            yield return www;
            Debug.Log(www.error);

            displayImage(www.texture);
        }

        static string YoutubeImage(string url)
        {

            string img = "http://i.ytimg.com/vi/"; //썸네일 추출 url 
            string urlsubstring = img + url.Substring(17, 11) + "/mqdefault.jpg";//Substring(동영상 ID)
            return urlsubstring;

        }

    }
}