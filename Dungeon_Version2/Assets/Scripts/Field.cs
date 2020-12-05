using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Item fItem;
    public Power fPower;

    public Transform fItmObj;         //아이탬 게임 오브젝트 연결
    public Transform fPwrObj;         //파워 게임 오브젝트 연결

    private void Awake()
    {
        fItem = fItmObj.GetComponent<Item>();
        fPower = fPwrObj.GetComponent<Power>();
    }

    public void FieldMaker (int idx) //각 값 idx에 따라 초기화
    {
        fItem.PlayerIdx = idx;
        fItem.UIPlace = idx;
        fPower.PlayerIdx = idx;
        fPower.UIPlace = idx;
    }
    //필드에서 아이탬 카드가 파워카드로 전환될때 뒤집어지는 효과

    public int WhatsOnFiled()
    {
        //보스방에서 즉시탈출 아이탬을 사용한 경우


        //if(fPwrObj.gameObject.activeSelf || fItem.itName==Item.ItName.CRYSTAL)
        if (fItem.itName == Item.ItName.STEAL || fItem.itName == Item.ItName.TREASON)
        {
            fItem.itName = Item.ItName.NONE;       //사용된 카드정보 초기화
            fItmObj.gameObject.SetActive(false);   //필드 아이탬 비활성화
            return (int)Controler.State.Room;
        }
        else
        {
            fItem.itName = Item.ItName.NONE;       //사용된 카드정보 초기화
            fItmObj.gameObject.SetActive(false);   //필드 아이탬 비활성화
            return (int)Controler.State.NextTurn;
        }

    }

    public void ResetState() //리셋
    {
        fItmObj.gameObject.SetActive(false);           //필드 아이탬 비활성화
        fPwrObj.gameObject.SetActive(false);           //필드 파워 비활성화

        fPower.power = 0;
        fItem.itName = Item.ItName.NONE;
    }
}
