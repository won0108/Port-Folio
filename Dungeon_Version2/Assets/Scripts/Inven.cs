using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Inven : MonoBehaviour
{
    public Power[] powers;      //파워카드
    public List<Item> item;     //아이템 카드
    public int pIdx;        //Player Idx

    public int MaxItem = 4;
    public GameObject[] power_objs;     //하이러키 게임오브젝트 연결 Powers의 power들 :사이즈는 5
    public Transform[] item_objs;       //하이러키 게임오브젝트 연결 Items의 item들 : 크기는 4
    
    public void InvenMake()
    { 
        for (int i = 0; i < 5; i++)
        {
            powers[i].PlayerIdx = pIdx;  //4명의 경우, 각 0,1,2,3
            powers[i].UIPlace = i;  // 0~4 까지 생성
            
            powers[i].canUse = false;  //사용할 수 있는 카드 확인
            powers[i].used = false; //카드가 사용되었는지 확인 > false 사용되지 않음

            powers[i].PInfoText.text = (i+1).ToString();//각 파워카드 1~5 숫자로 노출
        }
        
        for (int i = 0; i < MaxItem; i++)
        {
            item[i].PlayerIdx = pIdx;  //4명의 경우, 각 0,1,2,3
            item[i].UIPlace = i; // 0~3 까지 생성
            ////실험을 위한것
            item[i].canUse = false;
            item[i].used = false;
        }
    }



    public void DeleteUsedItem()                //사용한 카드 비활성화
    {
        for (int i = 0; i < MaxItem; i++)
        {
            if (item[i].used)  //사용한 아이템 카드 확인
            {
                item[i].itName = Item.ItName.NONE;
                item_objs[i].gameObject.SetActive(false);  //해당카드 비활성

                if (i != MaxItem-1)
                    ItemReSort(i);
            }
        }
    }

    void ItemReSort(int idx)                           //아이탬 사용 후, 앞으로 땡기기
    {
        Debug.Log("Item Sorting");

        for (int i = idx; i < MaxItem - 1; i++)
        {
            if(item[i].itName==Item.ItName.NONE && item[i+1].itName != Item.ItName.NONE)
            //if (!item_objs[i].gameObject.activeSelf && item_objs[i+1].gameObject.activeSelf)
            {
                Transform temp = item_objs[i + 1];
                Item _temp = item[i + 1]; 

                item_objs[i + 1] = item_objs[i];
                item[i + 1] = item[i];

                item_objs[i] = temp;
                item[i] = _temp;

                Vector3 tempPos = item_objs[i].localPosition;
                item_objs[i].localPosition = item_objs[ i + 1 ].localPosition;
                item_objs[i + 1].localPosition  = tempPos;
            }
        }
    }

    public void MyTurn() //게임 시작 player board 색 변경
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    public void TurnEnd()  //해당 순서 아닐 때 player board 검정색 변경
    {
        GetComponent<Renderer>().material.color = Color.black;
    }


}
