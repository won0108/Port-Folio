
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class Room
{
    public Stage.roomType rTyp;  //열거형 방타입
    public bool showupMap;              //true 맵에 정보가 보여진다, false 안보여진다.

    [HideInInspector] public List<Player> players;        //Result()와 플레이어 연결(인스펙터 창에 변수 숨김)
    [HideInInspector] public List<Player> players2;
    public MonRoom mRoom;               //floor Room부모클래스에서 자식Room 변수 접근 연결.
    public GolRoom gRoom; 
    public ItmRoom iRoom; 
    public TrpRoom tRoom; 
    public BosRoom bRoom; 

    public virtual void Result() { }
}//Room class
public class MonRoom : Room
{
    public Stage.MoName moName;  //열거형 몬스터 종류
    public int[] hp;
    public int damage;

    public MonRoom(XmlNode mNode)
    {
        mRoom = this;
        this.rTyp = Stage.roomType.MON;  //몬스터 방
        //xml에 있는 몬스터 이름 찾아 enum 타입으로 변환
        this.moName = (Stage.MoName)System.Enum.Parse(typeof(Stage.MoName), mNode.SelectSingleNode("MoName").InnerText); 
        this.hp = System.Array.ConvertAll(mNode.SelectSingleNode("Hp").InnerText.Split(','), int.Parse); //, 제외하고 xml 파일에 있는 몬스터 체력 저장
        this.damage = int.Parse(mNode.SelectSingleNode("Damage").InnerText);  //xml 파일에 있는 몬스터 damage 값 가져오기
    }//MonRoom 생성자

    public override void Result()
    {
        if (players != null) //플레이어가 있으면
        {
            for (int i = 0; i < players.Count; i++) 
            {
                players[i].HpMgr(-damage);  //해당 플레이어 데미지 입음
            }
        }
    }
}//MonRoom class
public class GolRoom : Room
{
    public int[] golds;

    public GolRoom(XmlNode gNode)
    {
        gRoom = this;
        this.rTyp = Stage.roomType.GOLD;
        this.golds = System.Array.ConvertAll(gNode.SelectSingleNode("Gold").InnerText.Split(','), int.Parse);  //xml에 있는 골드 값 가져오기
    }

    public override void Result()  //보상 받는 함수
    {
        int reward = 0;
        int reward2 = 0;

        if (players.Count!=0) //reward 몫이 나누어지면 보상을 받음
            reward = golds[0] / players.Count;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GoldMgr(reward); //보상을 줌
        }

        //2등 보상이 있는 경우
        if (golds[1]!=0 && players2 != null)
        {
            if (players2.Count != 0) //reward 몫이 나누어지면 보상을 받음
                reward2 = golds[1] / players2.Count;
            for (int j = 0; j < players2.Count; j++)
            {
                players2[j].GoldMgr(reward2); //보상을 줌
            }
        }
    }
}//GolRoom class
public class ItmRoom : Room
{
    public Item.ItName[] itmEnumList;
    public bool[] useNow;
    public int[] getValues;
    
    public List<Power> fInfo = new List<Power>();       //Result() 필요 변수

    public ItmRoom(XmlNode iNode)  //xml 파일에서 아이템방 불러오기
    {
        iRoom = this;
        this.rTyp = Stage.roomType.ITEM;
        string[] iList = iNode.SelectSingleNode("ItemList").InnerText.Split(',');
        this.itmEnumList = (from item in iList
                         let parsed = (Item.ItName)System.Enum.Parse(typeof(Item.ItName), item)
                         select parsed).ToArray();
        this.useNow = System.Array.ConvertAll(iNode.SelectSingleNode("UseNow").InnerText.Split(','), bool.Parse);
        this.getValues = System.Array.ConvertAll(iNode.SelectSingleNode("GetValues").InnerText.Split(','), int.Parse);
    }//ItmRoom 생성자

    public override void Result()  //상점 같은 기능
    {
        for (int i = 0; i < fInfo.Count; i++)
        {
            int idx = fInfo[i].power - 1 ; 
            if( useNow[idx] )
            {
                int reward = 0;
                if (itmEnumList[idx] == Item.ItName.GOLD)  //금화를 살 수 있는 파워를 맞게 낸 대상이 해택을 받음
                {
                    reward = getValues[idx];
                    players[i].GoldMgr(reward);
                }
                else if(itmEnumList[idx] == Item.ItName.PORTION)  //체력을 살 수 있는 파워를 맞게 낸 대상이 해택을 받음
                {
                    reward = getValues[idx];
                    players[i].HpMgr(reward);
                }
            }
            else
            {
                players[i].AddItem(itmEnumList[idx]);  //얻은 아이템 카드 노출
            }
        }
    }
}//ItmRoom class
public class TrpRoom : Room
{
    public Stage.TrType tTyp;
    public int[] LossValue;         //양수 : 피해량

    public int max;                 //Result() 필요 변수

    public TrpRoom(XmlNode tNode)  //함정방 Xml 파일에서 가져오기
    {
        tRoom = this;
        this.rTyp = Stage.roomType.TRAP;
        this.tTyp = (Stage.TrType)System.Enum.Parse(typeof(Stage.TrType), tNode.SelectSingleNode("TrType").InnerText);
        this.LossValue = System.Array.ConvertAll(tNode.SelectSingleNode("LossValue").InnerText.Split(','), int.Parse);
    }

    public override void Result()  //피해입는 함수
    {
        int LossV = 0;
        if(this.tTyp==Stage.TrType.GOLD)  //금화 함정방일 때
        {
            for (int i = 0; i < players.Count; i++)   //플레이어 금화 깍임
            {
                LossV= LossValue[max - 1];   
                players[i].GoldMgr(-LossV);
            }
        }
        else  //데미지 방일 때
        {
            for (int i = 0; i < players.Count; i++)  //플레이어 hp 깍임
            {
                LossV = LossValue[max - 1];
                players[i].HpMgr(-LossV);
            }
        }
    }
}//TrpRoom class
public class BosRoom : Room
{
    public Stage.BoName boName;
    public bool IsDefeat;
    public int[] hp;
    public Stage.ExitType exTyp;
    public int damage;                      // 양수  (음수 : 메두사)
    public int gold;                        // - 값은 금화가 깍임 ( + 인경우 금화를 얻는 경우)

    public BosRoom(XmlNode bNode)   //xml 파일에서 BOSS Text 가져온다
    {
        bRoom = this;
        this.rTyp = Stage.roomType.BOSS;
        this.boName = (Stage.BoName)System.Enum.Parse(typeof(Stage.BoName), bNode.SelectSingleNode("BoName").InnerText);
        this.IsDefeat = bool.Parse(bNode.SelectSingleNode("IsDefeat").InnerText);
        this.hp = System.Array.ConvertAll(bNode.SelectSingleNode("Hp").InnerText.Split(','), int.Parse);
        this.exTyp = (Stage.ExitType)System.Enum.Parse(typeof(Stage.ExitType), bNode.SelectSingleNode("ExitType").InnerText);
        this.damage = int.Parse(bNode.SelectSingleNode("Damage").InnerText);
        this.gold = int.Parse(bNode.SelectSingleNode("Gold").InnerText);
    }//BosRoom 생성자

    public override void Result()  //보스에 따라 피해가 다름
    {
        if(IsDefeat)
        {
            if (players != null)        //처치 실패 시
            {
                if (damage<0)           //메두사 : 플레이어의 Hp가 0이된다.
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        players[i].HpMgr(-players[i].hp);
                    }
                }
                else  //플레이어 hp 깍임 (메두사 아닐 때)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        players[i].HpMgr(-damage);
                        players[i].GoldMgr(gold);
                    }
                }
            }
        }//if IsDefeat
        if(boName==Stage.BoName.OBERON)  //보스가 오베론일 때 체력 깍임
            for (int i = 0; i < players.Count; i++)
            {
                players[i].HpMgr(-damage);
            }
        if (boName==Stage.BoName.DRAGONKING)  //보스가 드레곤킹일 때 체력 깍임
        {
            for (int i = 0; i < players2.Count; i++)
            {
                players2[i].HpMgr(-damage);
            }
        }
    }//Result
}//BosRoom class

public class Stage : MonoBehaviour
{
    public enum roomType
    {
        MON, GOLD, ITEM, TRAP, BOSS
    }
    public enum MoName
    {
        SKELETON, GOBLIN, ORC, DWARF, DRAGON, WEREWOLF, VIPER
    }
    public enum BoName
    {
        MEDUSA, CERBERUS, OBERON, DRAGONKING
    }
    public enum ExitType
    {
        NONE, MAX, ITEM
    }
    public enum TrType
    {
        GOLD, HP
    }

    //[HideInInspector]
    public int[] roomRange;
    [HideInInspector]
    public List<Room> rooms;
    public Room crrRoom;        //현재 방정보를 저장한다.

    public StageManager sm;
    public DataManager dm;

    private void Awake()
    {
        roomRange = new int[25];
        rooms = new List<Room>();
        crrRoom = new Room();

        sm = GetComponent<StageManager>();
        dm = GameObject.Find("GameControler").GetComponent<DataManager>();

        roomRange = sm.Random_room();  //StageManager > 확률로 랜덤 방
        //roomRange = sm.CountableRandom_room();
        rooms = sm.RoomRange(roomRange);  //StageManager > 랜덤 방 5개 저장
    }//Awake
}

