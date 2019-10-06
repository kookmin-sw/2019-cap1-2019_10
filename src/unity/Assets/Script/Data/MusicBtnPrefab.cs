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

    // 로그인 정보에 display하기 위해서 서버에 사용자의 이전 결과 전체 요청
    private void Start()
    {
        backendManager = GetComponent<BackendManager>();
        backendManager.OnAllResultLoaded += SettingClone;
        backendManager.OnResultLoaded += SettingClone;
        //this.results = BaymaxGame.instance.results;

    }

    // 미리 구성해놓은 노래 결과 버튼을 받은 정보만큼 복제시키기
    public void SettingClone(List<Result> results)
    {
        Debug.Log(results[0]);
        //this.results = results;

        //list에 있는 내용 btnItem으로 변환 후 content에 추가
        foreach (Result result in results)
        {
            Debug.Log(result);
            BtnPrefabClone = Instantiate(BtnPrefab, transform.position, Quaternion.identity) as GameObject;
            BtnPrefabClone.transform.localScale = one;
            MusicBtnObject musicBtnObject = BtnPrefabClone.GetComponent<MusicBtnObject>();

            //musicBtnObject.music.text = result.music;
            //musicBtnObject.link = result.link;

            //if (result.tag_1 != "")
            //{
            //    musicBtnObject.tag.text = "#" + result.tag_1;
            //}
            //if(result.tag_2 != "")
            //{
            //    musicBtnObject.tag.text  += " " + "#" + result.tag_2;
            //}

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
