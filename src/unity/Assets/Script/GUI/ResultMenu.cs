using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ResultMenu : BaseMenu
{
    private bool loading;
    private Result results;
    private Score newestResult;
 
    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        backendManager.OnResultLoaded += OnResultLoaded;
    }

    public void OnResult()
    {
        backendManager.GetResult();
    }

    private void OnResultLoaded(Result results)
    {
        Debug.Log("result menu");
        //results = newResults.ToList();

        //foreach (Result result in results)
        //{
        //    Debug.Log(result);
        //}
        this.results = results;
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
        Application.OpenURL(results.link_1);
    }
    public void OnClickMusicButton2()
    {
        Application.OpenURL(results.link_2);
    }
    public void OnClickMusicButton3()
    {
        Application.OpenURL(results.link_3);
    }

    private string[] Happiness = { "#기쁨", "#신남", "#흥폭팔", "#댄스댄스", "#환희", "#희열", "#스마일", "#행복" };
    private string[] Sadness = {"#슬픔", "#눈물뚝뚝", "#쓸쓸함", "#우울", "#서러움", "#애통", "#애처로움"};
    private string[] Surprise = {"#깜짝", "#놀람", "#화들짝", "#멘붕", "#당황", "#깜놀", "#봉변", "#혼비백산" };
    private string[] Fear = { "#무서움", "#공포", "#겁", "#두려움", "#불안", "#섬뜩함", "#아찔함", "#살벌" };
    private string[] Disgust = {"#역겨움", "#불쾌", "#우웩", "#진짜싫음", "#메스꺼움", "#역함", "#넌더리남"};
    private string[] Anger = {"#화남", "#화", "#폭발", "#짜증", "#스트레스", "#분노", "#역정", "#심통", "#울화", "#성질" };
    private string[] Lie = { "#거짓말", "#뻥", "#다시해보세요", "#거짓말쟁이", "#거짓", "#한번더", "#다시", "#아니야" };
}
