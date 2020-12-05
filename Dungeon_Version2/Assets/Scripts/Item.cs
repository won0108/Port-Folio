using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Item : MonoBehaviour
{
    public DataManager dm;

    public Text ItInfoText; 

    public enum ItName
    {
        SWORD, KEY, PORTION, STEAL, CRYSTAL, TREASON, GOLD, NONE
    }
    public enum FxType
    {
        POWER, HP, GOLD, TURN
    }

    public bool canUse;  //사용할 수 있는지 확인
    public bool used;  //사용했는지 확인
    public ItName itName;  //아이템 이름
    public FxType fxType;
    public int fxValue;

    public int PlayerIdx;  //플레이어 1~4
    public int UIPlace;

    private void Awake()
    {
        dm = GameObject.Find("GameControler").GetComponent<DataManager>();  //하이러키에서 GameCotroler 찾음 > DataManager 연결
        itName = ItName.NONE; //초기화
    }

    private void Update()
    {
        
    }

    public void ItemGetNode(XmlNode iNode) //DataManager을 통해 추출한 값 노출
    {
        this.itName = (ItName)System.Enum.Parse(typeof(ItName), iNode.SelectSingleNode("ItName").InnerText);  //enum 형식으로 ItName 변환
        this.fxType = (FxType)System.Enum.Parse(typeof(FxType), iNode.SelectSingleNode("FxType").InnerText);  //enum 형식으로 FxType 변환
        this.fxValue = int.Parse(iNode.SelectSingleNode("FxValue").InnerText);  //int 형식으로 FxValue 변환
        
        string str = itName.ToString(); //itName 값 저장
        ItInfoText.text = str;  //인스펙트 창에 직접 전달
    }

    public Item ItemMake(ItName itname)      //아이탬 생성자
    {
        if(itname != ItName.NONE)  //itname이 NONE 아닐 경우
        {
            int idx = (int)itname; //ItName 에 해당하는 인수 값을 idx에 저장
            XmlNode inode = dm.ItmNodes[idx];  //DataManager 스크립트 > 해당하는 itName,Fxtype,Fxvalue 가져옴
            ItemGetNode(inode);  //함수 호출
        }
        return this;
    }


    public void InfoReset()  
    {
        string str = itName.ToString();
        ItInfoText.text = str;
    }
}
