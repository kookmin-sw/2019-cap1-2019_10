using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

// 결과 화면 출력
public class ResultMenu : BaseMenu
{
    private bool loading;
    private List<Result> results;
    private Score newestResult;

    // 3가지 노래 추천 결과
    public Text MusicButton1, MusicButton2, MusicButton3;
    public Text MusicButtonTag1, MusicButtonTag2, MusicButtonTag3;
    // 태그 출력화면
    public Text Tag1, Tag2, Tag3, Tag4, Tag5, Tag6, Tag7, Tag8;
    // 썸네일 이미지
    public RawImage image1, image2, image3;

    Vector2 vec2;
    public float textureWidth, textureHeight;

    int[] tag_ = { 0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        backendManager.OnResultLoaded += OnResultLoaded;
        backendManager.OnPostPhotoSuccess += SettingTag;
    }

    // 얼굴 인식 높은 3순위로 태그 뿌리기
    public void SettingTag(string[] photos)
    {
        Debug.Log("set tag");
        string[] emotions = { "happy", "sad", "neutral", "surprised", "scared", "disgust", "angry", "Lie" };
        string[] check = { Tag1.text, Tag2.text, Tag3.text, Tag4.text, Tag5.text, Tag6.text, Tag7.text, Tag8.text };

        string[] photo = { "", "", "", "", "", "", "", };

        int cnt = 0;
        foreach (String str in photos)
        {
            if (!String.IsNullOrEmpty(str) && str != "\"")
            {
                Debug.Log(str);
                photo[cnt++] = str;
            }
        }

        switch (photo[0])
        {
            case "happy":
                Tag1.text = happy[UnityEngine.Random.Range(0, 16)];
                Tag2.text = happy[UnityEngine.Random.Range(0, 16)];
                Tag3.text = happy[UnityEngine.Random.Range(0, 16)];
                Tag4.text = happy[UnityEngine.Random.Range(0, 16)];
                break;
            case "sad":
                Tag1.text = sad[UnityEngine.Random.Range(0, 16)];
                Tag2.text = sad[UnityEngine.Random.Range(0, 16)];
                Tag3.text = sad[UnityEngine.Random.Range(0, 16)];
                Tag4.text = sad[UnityEngine.Random.Range(0, 16)];
                break;
            case "neutral":
                Tag1.text = neutral[UnityEngine.Random.Range(0, 16)];
                Tag2.text = neutral[UnityEngine.Random.Range(0, 16)];
                Tag3.text = neutral[UnityEngine.Random.Range(0, 16)];
                Tag4.text = neutral[UnityEngine.Random.Range(0, 16)];
                break;
            case "surprised":
                Tag1.text = surprised[UnityEngine.Random.Range(0, 16)];
                Tag2.text = surprised[UnityEngine.Random.Range(0, 16)];
                Tag3.text = surprised[UnityEngine.Random.Range(0, 16)];
                Tag4.text = surprised[UnityEngine.Random.Range(0, 16)];
                break;
            case "scared":
                Tag1.text = scared[UnityEngine.Random.Range(0, 16)];
                Tag2.text = scared[UnityEngine.Random.Range(0, 16)];
                Tag3.text = scared[UnityEngine.Random.Range(0, 16)];
                Tag4.text = scared[UnityEngine.Random.Range(0, 16)];
                break;
            case "disgust":
                Tag1.text = disgust[UnityEngine.Random.Range(0, 16)];
                Tag2.text = disgust[UnityEngine.Random.Range(0, 16)];
                Tag3.text = disgust[UnityEngine.Random.Range(0, 16)];
                Tag4.text = disgust[UnityEngine.Random.Range(0, 16)];
                break;
            case "angry":
                Tag1.text = angry[UnityEngine.Random.Range(0, 16)];
                Tag2.text = angry[UnityEngine.Random.Range(0, 16)];
                Tag3.text = angry[UnityEngine.Random.Range(0, 16)];
                Tag4.text = angry[UnityEngine.Random.Range(0, 16)];
                break;
            case "Lie":
                Tag1.text = Lie[UnityEngine.Random.Range(0, 16)];
                Tag2.text = Lie[UnityEngine.Random.Range(0, 16)];
                Tag3.text = Lie[UnityEngine.Random.Range(0, 16)];
                Tag4.text = Lie[UnityEngine.Random.Range(0, 16)];
                break;
        }

        switch (photo[1])
        {
            case "happy":
                Tag5.text = happy[UnityEngine.Random.Range(0, 16)];
                Tag6.text = happy[UnityEngine.Random.Range(0, 16)];
                break;
            case "sad":
                Tag5.text = sad[UnityEngine.Random.Range(0, 16)];
                Tag6.text = sad[UnityEngine.Random.Range(0, 16)];
                break;
            case "neutral":
                Tag5.text = neutral[UnityEngine.Random.Range(0, 16)];
                Tag6.text = neutral[UnityEngine.Random.Range(0, 16)];
                break;
            case "surprised":
                Tag5.text = surprised[UnityEngine.Random.Range(0, 16)];
                Tag6.text = surprised[UnityEngine.Random.Range(0, 16)];
                break;
            case "scared":
                Tag5.text = scared[UnityEngine.Random.Range(0, 16)];
                Tag6.text = scared[UnityEngine.Random.Range(0, 16)];
                break;
            case "disgust":
                Tag5.text = disgust[UnityEngine.Random.Range(0, 16)];
                Tag6.text = disgust[UnityEngine.Random.Range(0, 16)];
                break;
            case "angry":
                Tag5.text = angry[UnityEngine.Random.Range(0, 16)];
                Tag6.text = angry[UnityEngine.Random.Range(0, 16)];
                break;
            case "Lie":
                Tag5.text = Lie[UnityEngine.Random.Range(0, 16)];
                Tag6.text = Lie[UnityEngine.Random.Range(0, 16)];
                break;
        }

        switch (photo[2])
        {
            case "happy":
                Tag7.text = happy[UnityEngine.Random.Range(0, 16)];
                Tag8.text = happy[UnityEngine.Random.Range(0, 16)];
                break;
            case "sad":
                Tag7.text = sad[UnityEngine.Random.Range(0, 16)];
                Tag8.text = sad[UnityEngine.Random.Range(0, 16)];
                break;
            case "neutral":
                Tag7.text = neutral[UnityEngine.Random.Range(0, 16)];
                Tag8.text = neutral[UnityEngine.Random.Range(0, 16)];
                break;
            case "surprised":
                Tag7.text = surprised[UnityEngine.Random.Range(0, 16)];
                Tag8.text = surprised[UnityEngine.Random.Range(0, 16)];
                break;
            case "scared":
                Tag7.text = scared[UnityEngine.Random.Range(0, 16)];
                Tag8.text = scared[UnityEngine.Random.Range(0, 16)];
                break;
            case "disgust":
                Tag7.text = disgust[UnityEngine.Random.Range(0, 16)];
                Tag8.text = disgust[UnityEngine.Random.Range(0, 16)];
                break;
            case "angry":
                Tag7.text = angry[UnityEngine.Random.Range(0, 16)];
                Tag8.text = angry[UnityEngine.Random.Range(0, 16)];
                break;
            case "Lie":
                Tag7.text = Lie[UnityEngine.Random.Range(0, 16)];
                Tag8.text = Lie[UnityEngine.Random.Range(0, 16)];
                break;
        }
    }

    public void OnResult()
    {
        backendManager.GetResult(PlayerPrefs.GetString("x2").FromBase64());
    }

    private void OnResultLoaded(List<Result> results)
    {
        //Debug.Log(results[0].music + " " + results[0].link + " " + results[0].tag_1 + " " + results[0].tag_2);
        //results = newResults.ToList();

        foreach (Result result in results)
        {
            Debug.Log(result);
        }
        this.results = results;

        MusicButton1.text = results[0].music;
        MusicButton2.text = results[1].music;
        MusicButton3.text = results[2].music;

        MusicButtonTag1.text = "#" + results[0].tag_1 + ",  #" + results[0].tag_2;
        MusicButtonTag2.text = "#" + results[1].tag_1 + ",  #" + results[1].tag_2;
        MusicButtonTag3.text = "#" + results[2].tag_1 + ",  #" + results[2].tag_2;

        findThumbnail(results[0].link, image1);
        findThumbnail(results[1].link, image2);
        findThumbnail(results[2].link, image3);


        //for (int i = 0; i < 8; i++)
        //{
        //    tag_[i] = UnityEngine.Random.Range(0, random.Length);
        //}

        ////string str = "th";
        ////String.IsNullOrEmpty(str);

        //if (!String.IsNullOrEmpty(results[0].tag_1))
        //{
        //    Tag1.text = "#" + results[0].tag_1;
        //    Tag2.text = "#" + results[0].tag_2;
        //}
        //else
        //{
        //    Tag1.text = random[tag_[0]];
        //    Tag2.text = random[tag_[1]];
        //}

        //if (!String.IsNullOrEmpty(results[1].tag_1))
        //{
        //    Tag3.text = "#" + results[1].tag_1;
        //    Tag4.text = "#" + results[1].tag_2;
        //}
        //else
        //{
        //    Tag3.text = random[tag_[2]];
        //    Tag4.text = random[tag_[3]];
        //}

        //if (!String.IsNullOrEmpty(results[2].tag_1))
        //{
        //    Tag5.text = "#" + results[2].tag_1;
        //    Tag6.text = "#" + results[2].tag_2;
        //}
        //else
        //{
        //    Tag5.text = random[tag_[4]];
        //    Tag6.text = random[tag_[5]];
        //}

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
    
    private string[] happy = { "#기쁨", "#신남", "#흥폭팔", "#댄스댄스", "#환희", "#희열", "#스마일", "#행복", "#해피", "#화창", "#밝음", "#업 템포", "#좋음", "#행복함", "#기뻐", "#둠칫둠칫" };
    private string[] sad = {"#슬픔", "#눈물뚝뚝", "#쓸쓸함", "#우울", "#서러움", "#애통", "#애처로움", "#힘듦", "#비", "#씁쓸", "#우중충", "#먹구름", "#다운", "#무기력", "#폭풍우", "#BLUE" };
    private string[] neutral = { "#평범", "#무난", "#온화", "#내츄럴", "#평온", "#아무 생각이 없다", "#중립", "#편안", "#뒹굴", "#똑같은 날", "#오늘도", "#일상", "#데일리", "#늘", "#보통", "#항상" };
    private string[] surprised = {"#깜짝", "#놀람", "#화들짝", "#멘붕", "#당황", "#깜놀", "#봉변", "#혼비백산", "#헉", "#엌", "#뚀잉", "#세상에나", "#질겁", "#황당", "#경악", "#ㅇ0ㅇ!" };
    private string[] scared = { "#무서움", "#공포", "#겁", "#두려움", "#불안", "#섬뜩함", "#아찔함", "#살벌", "#근심", "#걱정", "#고뇌", "#그림자", "#심려", "#시름", "#우수", "#사념" };
    private string[] disgust = {"#역겨움", "#불쾌", "#우웩", "#진짜싫음", "#메스꺼움", "#역함", "#넌더리남", "#혐오", "#진절머리", "#싫증", "#진저리", "#몸서리", "#짜증", "#신물", "#엨", "#ㅡㅡ" };
    private string[] angry = {"#화남", "#폭발", "#짜증", "#스트레스", "#분노", "#역정", "#심통", "#울화", "#화통", "#울분", "#열받네", "#으와앍", "#다 사라졌으면", "#아 진짜", "#저리가", "#그만해" };
    private string[] Lie = { "#거짓말", "#뻥", "#다시해보세요", "#거짓말쟁이", "#거짓", "#한번더", "#다시", "#아니야", "#오잉", "#뭐징", "#츄라이", "#retry", "#다름", "#억지", "#일부러", "#고의" };
}
