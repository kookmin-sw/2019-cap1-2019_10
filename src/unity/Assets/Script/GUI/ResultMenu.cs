using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class ResultMenu : BaseMenu
{
    private bool loading;
    private List<Result> results;
    private Score newestResult;

    public Text MusicButton1, MusicButton2, MusicButton3;
    public Text TagButton1, TagButton2, TagButton3;
    public Text Tag1, Tag2, Tag3, Tag4, Tag5, Tag6, Tag7, Tag8;

    public RawImage image1, image2, image3;

    Vector2 vec2;
    public float textureWidth, textureHeight;

    int[] tag_ = { 0, 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        backendManager.OnResultLoaded += OnResultLoaded;
        //backendManager.OnPostPhotoSuccess += SettingTag;
    }

    public void SettingTag(string[] photos)
    {
        //int n;
        //for (n = 0; n < photos.Length;)
        //{
        //    if (photos[n] != "")
        //    {
        //        n++;
        //        Debug.Log(photos[n]);
        //    }
        //}
        //Debug.Log(n);
        string[] emotions = { "Happiness", "Sadness", "Surprise", "Fear", "Disgust", "Anger", "Lie" };
        string[] check = { Tag1.text, Tag2.text, Tag3.text, Tag4.text, Tag5.text, Tag6.text, Tag7.text, Tag8.text };


        //System.Random random = new System.Random();
        //int emotion = random.Next(0, 6);
        //int tag = random.Next(0, 7);
        //Debug.Log(emotion);

        //switch (emotion)
        //{
        //    case 0:
        //        Happiness[t
        //}

        //string emotion = photos[1];
        //Tag1.text = arr[tag];
        //int num = 0;

        //int cnt = 0;
        //for (int i = 0; i < 8;)
        //{
        //    if (photos[i] == "" | photos[i] == null) return;
        //    int tag_ = UnityEngine.Random.Range(0, 7);


        //    Debug.Log(i + "  : " + emotion_ + " " + tag_);

        //    check[i] = emotions[--emotion_][--tag_];
        //}


    }

    public void OnResult()
    {
        backendManager.GetResult(PlayerPrefs.GetString("x2").FromBase64());
    }

    private void OnResultLoaded(List<Result> results)
    {
        Debug.Log("result menu");
        Debug.Log(results[0].music + " "  + results[0].link + " " + results[0].tag_1 + " " + results[0].tag_2);
        //results = newResults.ToList();

        //foreach (Result result in results)
        //{
        //    Debug.Log(result);
        //}
        this.results = results;

        MusicButton1.text = results[0].music;
        MusicButton2.text = results[1].music;
        MusicButton3.text = results[2].music;

        TagButton1.text = "#" + results[0].tag_1 + "  " + "#" + results[0].tag_2;
        TagButton2.text = "#" + results[1].tag_1 + "  " + "#" + results[1].tag_2;
        TagButton3.text = "#" + results[2].tag_1 + "  " + "#" + results[2].tag_2;

        findThumbnail(results[0].link, image1);
        findThumbnail(results[1].link, image2);
        findThumbnail(results[2].link, image3);


        for (int i = 0; i < 8; i++)
        {
            tag_[i] = UnityEngine.Random.Range(0, random.Length);
            Debug.Log(tag_[i]);
        }
        Tag1.text = random[tag_[0]];
        Tag2.text = random[tag_[1]];
        Tag3.text = random[tag_[2]];
        Tag4.text = random[tag_[3]];
        Tag5.text = random[tag_[4]];
        Tag6.text = random[tag_[5]];
        Tag7.text = random[tag_[6]];
        Tag8.text = random[tag_[7]];

        loading = false;
        //newestScore = scores.OrderByDescending(s => s.Updated).FirstOrDefault(s => s.Amount == (int)CurrentScore);
        //loading = false;
    }

    private void ShowWindow()
    {
        if (loading)
        {
            GUILayout.Label("Loading..");
            Debug.Log("Loading");
            return;
        }
    }

    public void OnClickMusicButton1()
    {
        Application.OpenURL(results[0].link);
    }
    public void OnClickMusicButton2()
    {
        Application.OpenURL(results[1].link);
    }
    public void OnClickMusicButton3()
    {
        Application.OpenURL(results[2].link);
    }

    public void findThumbnail(string url, RawImage rawImage)
    {
        StartCoroutine(LoadImg(YoutubeImage(url), rawImage));
    }

    void displayImage(Texture2D imgToDisp, RawImage rawImage)
    {
        //Resize Image
        vec2.Set(textureWidth, textureHeight);
        rawImage.GetComponent<RectTransform>().sizeDelta = vec2;
        rawImage.texture = imgToDisp;
    }


    IEnumerator LoadImg(string url, RawImage rawImage)
    {
        Debug.Log(url);
        yield return null;
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.error);

        displayImage(www.texture, rawImage);
    }

    static string YoutubeImage(string url)
    {

        string img = "http://i.ytimg.com/vi/"; //썸네일 추출 url 
        string urlsubstring = img + url.Substring(17, 11) + "/mqdefault.jpg";//Substring(동영상 ID)
        return urlsubstring;

    }

    private string[] random = { "#기쁨", "#신남", "#흥폭팔", "#댄스댄스", "#환희", "#희열", "#스마일", "#행복" ,
    "#슬픔", "#눈물뚝뚝", "#쓸쓸함", "#우울", "#서러움", "#애통", "#애처로움", "#힘듦",
    "#깜짝", "#놀람", "#화들짝", "#멘붕", "#당황", "#깜놀", "#봉변", "#혼비백산",
     "#무서움", "#공포", "#겁", "#두려움", "#불안", "#섬뜩함", "#아찔함", "#살벌" ,
    "#역겨움", "#불쾌", "#우웩", "#진짜싫음", "#메스꺼움", "#역함", "#넌더리남", "#혐오",
    "#화남", "#폭발", "#짜증", "#스트레스", "#분노", "#역정", "#심통", "#울화",
    "#거짓말", "#뻥", "#다시해보세요", "#거짓말쟁이", "#거짓", "#한번더", "#다시", "#아니야"};

    private string[] Happiness = { "#기쁨", "#신남", "#흥폭팔", "#댄스댄스", "#환희", "#희열", "#스마일", "#행복" };
    private string[] Sadness = {"#슬픔", "#눈물뚝뚝", "#쓸쓸함", "#우울", "#서러움", "#애통", "#애처로움", "#힘듦" };
    private string[] Surprise = {"#깜짝", "#놀람", "#화들짝", "#멘붕", "#당황", "#깜놀", "#봉변", "#혼비백산" };
    private string[] Fear = { "#무서움", "#공포", "#겁", "#두려움", "#불안", "#섬뜩함", "#아찔함", "#살벌" };
    private string[] Disgust = {"#역겨움", "#불쾌", "#우웩", "#진짜싫음", "#메스꺼움", "#역함", "#넌더리남", "#혐오"};
    private string[] Anger = {"#화남", "#폭발", "#짜증", "#스트레스", "#분노", "#역정", "#심통", "#울화"};
    private string[] Lie = { "#거짓말", "#뻥", "#다시해보세요", "#거짓말쟁이", "#거짓", "#한번더", "#다시", "#아니야" };
}
