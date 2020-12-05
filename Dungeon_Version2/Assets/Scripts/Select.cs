using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    UIManager ui;
    public bool selected;  //선택 받았는지 확인
    public Text InfoText;


    public enum Select_State
    {
        Ready,
        Selecting,
        Selected
    }

    private void Awake()
    {
        ui = GameObject.Find("UIMgr").GetComponent<UIManager>();  //하이러키에서 UIMgr 찾기
    }

    private void OnEnable()
    {
        selected = false;   //비활성
        ui.areaState = Select_State.Selecting;
    }


    private void OnMouseEnter()   //마우스를 해당 GameObject에 가져다댈 시, 노란색으로 변경
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    private void OnMouseExit()   //마우스 해당 GameObject에 떨어질 시, 빨간색으로 변경
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void OnMouseDown()    //GameObject 클릭 시, 해당 정보 저장
    {
        Debug.Log("Mouse Down");

        this.selected = true;
        this.ui.areaState = Select_State.Selected;
    }
    

    public void LoadInfo(Item.ItName itname) //UIManager > selectAreas가 활성화시, 같이 표시된다.
    {
        if (itname == Item.ItName.STEAL)
        {
            InfoText.text = "금화 2개를 뺏어온다.";
        }
        else if(itname == Item.ItName.TREASON)
        {
            InfoText.text = "체력에 피해 2를 입힌다.";
        }
    }
}
