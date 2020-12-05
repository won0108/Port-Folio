using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class StageManager : MonoBehaviour
{
    public DataManager dm;

    private int[] varOrg = {33, 66, 84};   // M 33,  G 33,  I 18, T 16      //방 생성 누적 확률 초기값
    
    //v.1
    //private int[] varMon = {0, 44, 73};    // M  0,  G 44,  I 29, T 27
    //private int[] varGol = {44, 44, 73};   // M 44,  G  0,  I 29, T 27
    //private int[] varItm = {36, 72, 72};   // M 36,  G 36,  I  0, T 19
    //private int[] varTrp = {38, 76, 100};  // M 38,  G 38,  I 24, T  0

    //v.2
    private int[] varMon = { 0, 40, 30, 30 };
    private int[] varGol = { 40, 0, 30, 30 };
    private int[] varItm = { 35, 35, 0, 30 };
    private int[] varTrp = { 35, 35, 30, 0 };



    private int mVar = 8, gVar = 7, iVar = 4, tVar = 5, bVar = 1;       //카운트 가능한 정해진 방의 갯수 총 25 -> CountableRandom()

    private void Awake()
    {
        dm = GameObject.Find("GameControler").GetComponent<DataManager>();
    }

    public int RandomCalcV2 (int[] var)             //0은 진짜 0
    {
        int num = (int)Random.Range(0, 100);

        List<int> newvar = new List<int>();
        List<int> varidx = new List<int>();

        for (int i = 0; i < var.Length; i++)
        {
            if (var[i] == 0)
            {
                newvar.Add(var[i]);
                varidx.Add(i);
            }
        }

        for (int j = 0; j < var.Length; j++)
        {
            if (var[j] != 0)
            {
                newvar.Add(var[j]);
                varidx.Add(j);
            }
        }

        if (num <= newvar[0])
        { return varidx[0]; }
        else if (num > newvar[0] && num <= newvar[0] + newvar[1])
        { return varidx[1]; }
        else if (num > newvar[0] + newvar[1] && num <= newvar[0] + newvar[1] + newvar[2])
        { return varidx[2]; }
        else
        { return varidx[3]; }

    }

    public int RandomCalc(int[] var)            //  0 = 1/100
    {
        int num = (int)Random.Range(0, 100);

        if(num < var[0])
        { return 0; }
        else if( num >= var[0] && num < var[1])
        { return 1; }
        else if(num >= var[1] && num < var[2])
        { return 2; }
        else
        { return 3; }
    }//RandomCalc

    public int[] VarCalc(int idx, int[] arr, int[] var)     
    {
        if (var != varOrg)  return varOrg;

        switch(arr[idx-1])          //이전 방 확인
        {
            case 0 :
                if (idx < 2) return var;
                if(arr[idx - 2] == 0 && arr[idx-1] == 0)     //몬스터 방 연속 2회 등장 시  확률 0으로 조정
                //if(arr[idx - 2] == 0 && arr[idx - 1] == 0 && arr[idx] == 0)     //몬스터 방 연속3회 등장 시  확률 0으로 조정
                {
                    return varMon;
                }
                break;
            case 1:
                if (idx < 2) return var;
                if (arr[idx - 2] == 1 && arr[idx-1] == 1)        //골드 방 연속 2회 등장 시  확률 0으로 조정
                //if (arr[idx - 2] == 1 && arr[idx - 1] == 1 && arr[idx] == 1)        //골드 방 연속3회 등장 시  확률 0으로 조정
                {
                    return varGol;
                }
                break;
            case 2:
                if (arr[idx - 1] == 2)// && arr[idx] == 2)     // 아이탬 방 연속 1회 등장 시  확률 0으로 조정
                {
                    return varItm;
                }
                break;
            case 3:
                if (arr[idx - 1] == 3)// && arr[idx] == 3)     // 함정 방 연속 1회 등장 시  확률 0으로 조정
                {
                    return varTrp;
                }
                break;
            default:
                break;
        }
        return var;
    }//VarCalc

    public int[] Random_room()              //방 확률 적용 랜덤배열 
    {
        int[] newrange = new int[25];
        int[] var= varOrg; 
        int idx = 0;

        for (int i = 0; i < 24; i++)
        {
            idx = i;
            if(i>=1)
                var = VarCalc(i, newrange, var);

            newrange[i] = RandomCalcV2(var);
        }

        newrange[24] = 4;   //마지막방은 보스방

        return newrange;
    }//Random_room

    public int[] CountableRandom_room()             //카운트 가능한 정해진 방의 갯수 내에서의 랜덤배열
    {
        List<int> listOrg = new List<int>();
        int[] arr;

        for (int i = 0; i < mVar; i++)
        {
            listOrg.Add(0);
        }
        for (int i = 0; i < gVar; i++)
        {
            listOrg.Add(1);
        }
        for (int i = 0; i < iVar; i++)
        {
            listOrg.Add(2);
        }
        for (int i = 0; i < tVar; i++)
        {
            listOrg.Add(3);
        }
        for (int i = 0; i < bVar; i++)
        {
            listOrg.Add(4);
        }

        arr = listOrg.ToArray();
        int RangeMax = 23;
        int temp = 0;
        for (int i = 22; i >= 0; i--)
        {
            int idx = Random.Range(0, RangeMax);
            temp = arr[i];
            arr[i] = arr[idx];
            arr[idx] = temp;
            RangeMax--;
        }

        return arr;
    }//CountableRandom_room

    public XmlNode OutNode(Stage.roomType rTyp)     // 랜덤 Node 생성자
    {
        int count=0;
        int idx=0;
        XmlNode Node=null;

        switch (rTyp)
        {   //몬스터방 일 때, xml에 저장한 7마리 몬스터 (이름,체력, 데미지) 중 랜덤으로 1마리 뽑기
            case Stage.roomType.MON:
                count = dm.MonNodes.Count;
                idx = Random.Range(0, count);
                Node = dm.MonNodes[idx];
                return Node;
            //금화방일 때, xml에 저장한 모든 내용중 1개만 뽑기
            case Stage.roomType.GOLD:
                count = dm.GoRNodes.Count;
                idx = Random.Range(0, count);
                Node = dm.GoRNodes[idx];
                return Node;
            //일종의 상점 방에서 xml에 저장한 것 충 하나의 방만 뽑기
            case Stage.roomType.ITEM:
                count = dm.ItRNodes.Count;
                idx = Random.Range(0, count);
                Node = dm.ItRNodes[idx];
                return Node;
            //함정방 하나 뽑기
            case Stage.roomType.TRAP:
                count = dm.TpRNodes.Count;
                idx = Random.Range(0, count);
                Node = dm.TpRNodes[idx];
                return Node;
            //보스방 하나 뽑기
            case Stage.roomType.BOSS:
                count = dm.BsRNodes.Count;
                idx = Random.Range(0, count);
                Node = dm.BsRNodes[idx];
                return Node;

            default:
                return Node;
        }
    }//OutNode

    public Room Room(Stage.roomType rTyp)   //방 생성자
    {
        Room rm=null;
        XmlNode node= OutNode(rTyp);
        //Debug.Log($"{node.Name}");

        switch (rTyp)
        {
            case Stage.roomType.MON:
                rm = new MonRoom(node);
                return rm;

            case Stage.roomType.GOLD:
                rm = new GolRoom(node);
                return rm;

            case Stage.roomType.ITEM:
                rm = new ItmRoom(node);
                return rm;

            case Stage.roomType.TRAP:
                rm = new TrpRoom(node);
                return rm;

            case Stage.roomType.BOSS:
                rm = new BosRoom(node);
                return rm;

            default:
                return rm;
        }
    }//Room

    public List<Room> RoomRange(int[] roomRange)    // 랜덤 배열에 의한 25개의 방 생성
    {
        List<Room> newRoom = new List<Room>();

        for (int i = 0; i < roomRange.Length; i++)
        {
            Stage.roomType rt = (Stage.roomType)roomRange[i];
            Room _room=Room(rt);
            //Debug.Log($"{_room.rTyp}");
            if(_room!=null)
            {
                newRoom.Add(_room);
            }
        }
        return newRoom;
    }//RoomRange
}
