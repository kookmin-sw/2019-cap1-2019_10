using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListView : MonoBehaviour
{
    public GameObject ItemObject;
    public Transform Content;

    public List<Item> ItemList;

    // Start is called before the first frame update
    void Start()
    {
        this.Binding();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Binding()
    {
        GameObject btnItemTemp;
        ItemObject itemObjectTemp;

        //list에 있는 내용 btnItem으로 변환 후 content에 추가
        foreach(Item item in this.ItemList)
        {
            //추가할 오브젝트 생성
            btnItemTemp = Instantiate(this.ItemObject) as GameObject;
            //오브젝트가 갖고 있는 item object 찾기
            itemObjectTemp = btnItemTemp.GetComponent<ItemObject>();

            itemObjectTemp.Music.text = item.Music;
            itemObjectTemp.Tag.text = item.Tag;

            btnItemTemp.transform.SetParent(this.Content);
        }
    }

    public void ItemClick_Result()
    {
        Debug.Log("클릭");
    }
}
