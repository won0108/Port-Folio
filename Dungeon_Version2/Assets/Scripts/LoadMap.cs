using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    public Image[] roomsOnMap;
    public Sprite[] roomIcons;

    Vector3 currentPosition;
    Vector3 currentScale;

    Controler ctrl;
    UIManager ui;
    
    public enum LoadMap_State
    {
        Ready,
        Loading,
        ShowUp
    }

    private void Awake()
    {
        ui = GameObject.Find("UIMgr").GetComponent<UIManager>();  //하이러키에서 찾고 연결
        ui.mapState = LoadMap_State.Ready;  //지도 상태가 ready
        //stage = GameObject.Find("Room").GetComponent<Stage>();
        ctrl = GameObject.Find("GameControler").GetComponent<Controler>();  //하이러키에서 찾고 연결


        ui.Isbig = false;
        currentPosition = transform.position;
        currentScale = transform.localScale;
    }

    private void Update()  //지도 현재 상태
    {
        switch (ui.mapState)
        {
            case LoadMap_State.Ready:

                break;

            case LoadMap_State.Loading:
                //Debug.Log("Loading Map...");
                ReadInfo(ctrl.floor);

                break;

            case LoadMap_State.ShowUp:
                break;

            default:
                break;
        }
    }


    public void ShowBigger()  //지도 클릭 시, UIMgr 크기만큼 커짐
    {
        if(!ui.Isbig)
        {
            this.transform.position = new Vector3(0,0,-19);
            this.transform.localScale = new Vector3(7f, 3.5f, 7f);

            ui.Isbig = true;
        }
        else
        {
            transform.position = currentPosition;
            transform.localScale = currentScale;

            ui.Isbig = false;
        }
    }

    void ReadInfo(Room[] currFloor)  //보여질 방 안보여질 방을 지도에 표시해서 노출
    {
        for (int i = 0; i < 5; i++)
        {
            if (currFloor[i].showupMap)  //State 스크립트 > showupMap T/F 확인
            {
                int idx = (int)currFloor[i].rTyp;
                roomsOnMap[i].sprite = roomIcons[idx];
                roomsOnMap[i].color = Color.white;
            }
            else
            {
                roomsOnMap[i].sprite = roomIcons[5];
                roomsOnMap[i].color = Color.white;
            }
        }
    }
}
