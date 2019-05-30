using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ResultMenu : BaseMenu
{
    private bool loading;
    private List<Result> results;
    private Score newestResult;

    public Text MusicButton1, MusicButton2, MusicButton3;
 
    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        backendManager.OnResultLoaded += OnResultLoaded;
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

    private string[] Happiness = { "#기쁨", "#신남", "#흥폭팔", "#댄스댄스", "#환희", "#희열", "#스마일", "#행복" };
    private string[] Sadness = {"#슬픔", "#눈물뚝뚝", "#쓸쓸함", "#우울", "#서러움", "#애통", "#애처로움"};
    private string[] Surprise = {"#깜짝", "#놀람", "#화들짝", "#멘붕", "#당황", "#깜놀", "#봉변", "#혼비백산" };
    private string[] Fear = { "#무서움", "#공포", "#겁", "#두려움", "#불안", "#섬뜩함", "#아찔함", "#살벌" };
    private string[] Disgust = {"#역겨움", "#불쾌", "#우웩", "#진짜싫음", "#메스꺼움", "#역함", "#넌더리남"};
    private string[] Anger = {"#화남", "#화", "#폭발", "#짜증", "#스트레스", "#분노", "#역정", "#심통", "#울화", "#성질" };
    private string[] Lie = { "#거짓말", "#뻥", "#다시해보세요", "#거짓말쟁이", "#거짓", "#한번더", "#다시", "#아니야" };
}
