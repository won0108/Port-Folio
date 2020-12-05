using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Player : MonoBehaviour
{
    public Inven inven;

    DataManager dm;
    Controler ctrl;
    
    [HideInInspector]
    public Item itmake;             //Item 클래스 연결 

    public GameObject invenObj;     //하이러키 게임 오브젝트 연결
    public GameObject fieldObj;

    public plyClass className;
    //public int classNum = 4;        //직업 개수

    public Image exitImg;
    float[,] imgPos = new float[,] { { 43.1f, 9.22f }, { 37.6f, 36.5f }};  //보스 방에서 탈출 성공 시, 이미지가 노출될 위치

    public Text PlrText;  //게임 플레이어 인원수에 따라 player 1~4로 노출
    public Text PlaStatText;  //직업에 따라 기본적으로 가지는 hp 및 gold
    public Text HpChangeText;
    public Text GoldChangeText;

    public bool moreturn;           //수정구 카드 사용시 True 값 유지. (본인 차례에 행동을 할 수 있는지) 
    public int gold;
    public int hp;
    public bool exit;               //탈출 성공 시 true
    int HpMax = 15;
    //public bool isAlive = true;

    public enum plyClass
    {
        GRAVEROBBER, WORRIOR, PRIEST, WITCH
    }

    private void Awake()  //게임 시작위한 기본 세팅
    {
        gold = 0;
        hp = 15;
        exit = false;
        moreturn = true;

        ctrl = GameObject.Find("GameControler").GetComponent<Controler>();  //하이러키에서 찾고 연결
        dm = GameObject.Find("GameControler").GetComponent<DataManager>();  //하이러키에서 찾고 연결
        itmake = inven.item[0].GetComponent<Item>();
    }


    //직업 부여
    public void GetClass(int i)
    {
        int idx = i;
        XmlNode pnode = dm.PlCNodes[idx]; //DataManager 스크립트 > xml중 idx 위치 className,hp, gold 불러옴
        ClassInfo(pnode);
    }

    void ClassInfo(XmlNode pNode)  //직업 이름, hp, Gold, 아이템 카드 xml 에서 추출
    {
        this.className = (plyClass)System.Enum.Parse(typeof(plyClass), pNode.SelectSingleNode("PlName").InnerText);  //해당 직업 PIName을 enum형식으로 변환
        this.hp += int.Parse(pNode.SelectSingleNode("Hp").InnerText);  //해당 직업 Hp int 형식으로 변환
        this.gold += int.Parse(pNode.SelectSingleNode("Gold").InnerText);  //해당 직업 Gold int 형식으로 변환
        //해당 직업 Item enum 형식으로 변환 : Item 스크립트 > itName enum
        Item.ItName itName = (Item.ItName)System.Enum.Parse(typeof(Item.ItName), pNode.SelectSingleNode("Item").InnerText);   
        AddItem(itName); //함수 호출
        //inven.item[0].AddItem(itName);
    }
    

    //아이템 획득
    public void AddItem(Item.ItName itname)
    {
        if(itname != Item.ItName.NONE)  //itname 이 NONE이 아니면 실행
        {
            if (inven.item_objs[3].gameObject.activeSelf) //아이템 저장소가 다 찼을 때
            {
                Debug.Log("Item Fulled");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (inven.item_objs[i].gameObject.activeSelf == false)      //빈칸 있는 경우
                {
                    inven.item_objs[i].gameObject.SetActive(true);          //아이템 활성화 -> 해야지만 아이탬 스크립트 가동됨.
                    //아이템 카드 명칭 (Inven 스크립트 > item 리스트로 item 스크립트 접근 > ItemMake 함수 호출 및 ItName 추출, 화면에 획득 아이템 표시
                    inven.item[i].ItemMake(itname);  
                    inven.item_objs[i].GetComponent<MeshRenderer>().material.color = Color.white; //하얀색 카드 노출
                    break;
                }
            }
        }
    }

    public void HpMgr(int hp)                           //체력 변화 Hp >=15 시 15로 초기화
    {
        this.hp += hp;

        if (this.hp>= HpMax)
        {
            Debug.Log("Hp Fulled");
            this.hp = HpMax;
        }
        ChangeStatText(hp, 0);
    }//HpMgr                       

    public void GoldMgr(int gold)                       //금화 변화
    {
        this.gold += gold;

        if(this.gold <= 0)
        {
            this.gold = 0;
        }

        ChangeStatText(0, gold);
    }//GoldMgr          


    //Hp, Gold Text
    public void ChangeStatText(int hpValue, int goldValue)  //데미지 또는 증가할 체력, 금화를 text로 보여준다
    {
        if(hpValue != 0)
        {
            HpChangeText.gameObject.SetActive(true);
            if (hpValue > 0)
            {
                HpChangeText.text = "  +" + hpValue.ToString();
            }
            else
            {
                HpChangeText.text = "   " + hpValue.ToString();
            }
            StartCoroutine(FadeTextStat(HpChangeText));
        }
        if (goldValue != 0)
        {
            GoldChangeText.gameObject.SetActive(true);
            if (goldValue > 0)
            {
                GoldChangeText.text = "  +" + goldValue.ToString();
            }
            else
            {
                GoldChangeText.text = "   " + goldValue.ToString();
            }
            StartCoroutine(FadeTextStat(GoldChangeText));
        }

    }

    IEnumerator FadeTextStat(Text txt)  //데미지 나 증가할 체력, 금화를 애니메이션 처럼 보여줌
    {
        Color textColor = txt.color;
        Vector3 Startposition = txt.GetComponent<RectTransform>().localPosition;
        Vector3 EndPosition = Startposition + 2*Vector3.up;

        textColor.a = 1.0f;     //불투명 시작
        float duration = 1f;

        float fadeSpeed = Mathf.Abs(textColor.a - 0.1f) / duration;

        while( !Mathf.Approximately(textColor.a, 0.1f) )      //점점 투명해지면서 사라짐
        {
            textColor.a = Mathf.MoveTowards(textColor.a, 0.1f, fadeSpeed * Time.deltaTime);
            Vector3 newPos = Vector3.MoveTowards(txt.rectTransform.localPosition, EndPosition, fadeSpeed * Time.deltaTime);

            txt.color = textColor;
            txt.rectTransform.localPosition = newPos;
            yield return null;
        }
        TextReload(txt);
    }

    void TextReload(Text txt)  //데미지나 증가할 체력, 금화를 다시 세팅
    {
        txt.gameObject.SetActive(false);
        PlaStatText.text = "Hp : " + hp.ToString() + "   Gold : " + gold.ToString();
    }

    public void IsExit() //보스 방에서 탈출 했을 때 이미지 노출
    {
        int idx = inven.pIdx % 2;
        exitImg.gameObject.SetActive(true);
        exitImg.transform.localPosition = new Vector3(imgPos[idx,0], imgPos[idx, 1],0f);
        exit = true;
    }

}

