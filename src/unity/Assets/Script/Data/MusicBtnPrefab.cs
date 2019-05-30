using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBtnPrefab : MonoBehaviour
{
    public GameObject BtnPrefab;
    GameObject BtnPrefabClone;
    public Transform Grid;
    Vector3 one = new Vector3(1.0f, 1.0f, 1.0f);

    public List<Result> results;
    BackendManager backendManager;

    private void Start()
    {
        backendManager = GetComponent<BackendManager>();
        backendManager.OnAllResultLoaded += SettingClone;
        backendManager.OnResultLoaded += SettingClone;
        //this.results = BaymaxGame.instance.results;

    }

    public void SettingClone(List<Result> results)
    {
        this.results = results;

        //list에 있는 내용 btnItem으로 변환 후 content에 추가
        foreach (Result result in this.results)
        {
            BtnPrefabClone = Instantiate(BtnPrefab, transform.position, Quaternion.identity) as GameObject;
            BtnPrefabClone.transform.localScale = one;
            MusicBtnObject musicBtnObject = BtnPrefabClone.GetComponent<MusicBtnObject>();

            musicBtnObject.music.text = result.music;
            musicBtnObject.link = result.link;

            if (result.tag_1 != "")
            {
                musicBtnObject.tag.text = "#" + result.tag_1;
            }
            if(result.tag_2 != "")
            {
                musicBtnObject.tag.text  += " " + "#" + result.tag_2;
            }

            ////추가할 오브젝트 생성
            //btnItemTemp = Instantiate(this.ItemObject) as GameObject;
            ////오브젝트가 갖고 있는 item object 찾기
            //itemObjectTemp = btnItemTemp.GetComponent<ItemObject>();

            //itemObjectTemp.Music.text = item.Music;
            //itemObjectTemp.Tag.text = item.Tag;

            BtnPrefabClone.transform.SetParent(this.Grid, false);
        }
    }
}
