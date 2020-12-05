using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadRoom : MonoBehaviour
{
    public Stage stage;
    public UIManager ui;
    LoadRoom.LoadRoom_State rState;

    public Transform[] LoadRooms;      //로드할 게임오브젝트 게임오브젝트 연결

    public int idxRoom;
    public Text[] roomInfo;

    //플레이어에게 보여 줄 정보
    //방 타입
    //몬스터 : 종류, 데미지, 체력 // 골드 : 보상 목록 // 아이탬 : 아이탬 목록, 가치 
    // 함정 : 타입, 위력 // 보스 : 종류, 데미지, 체력, 특수능력

    public enum LoadRoom_State
    {
        Ready,
        Loading,
        UnLoad
    }

    private void Awake()
    {
        //rState = Controler.instance.ui.stLoad;
        idxRoom = 0;
        //Controler.instance.ui.stLoad = LoadRoom_State.Ready;
        ui.stLoad = LoadRoom_State.Ready;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        switch (ui.stLoad)  //현재 룸 진행 상태
        {
            case LoadRoom_State.Ready:
                break;

            case LoadRoom_State.Loading:
                Debug.Log($"Loading Room.......");
                idxRoom = (int)stage.crrRoom.rTyp;
                ReadInfo(idxRoom);

                LoadRooms[idxRoom].gameObject.SetActive(true);
                Debug.Log($"Loaded Room.......");

                ui.stLoad = LoadRoom_State.Ready;
                break;

            case LoadRoom_State.UnLoad:
                LoadRooms[idxRoom].gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    void ReadInfo(int rTyp)  //UIMgr 에 표시될 Text
    {
        int[] value;

        switch (rTyp)  
        {
            case 0:
                Stage.MoName moname = stage.crrRoom.mRoom.moName;
                string str = moname.ToString();
                roomInfo[0].text = "몬스터 방" 
                         + "\n" + str
                         + "\n" + "Hp : " + stage.crrRoom.mRoom.hp[Controler.Player_num-2].ToString()
                         + "\n" + "Damage : " + stage.crrRoom.mRoom.damage.ToString();
                break;

            case 1:
                roomInfo[1].text = "금화 방"
                    + "\n" + "1등 보상 : " + stage.crrRoom.gRoom.golds[0].ToString()
                    + "\n" + "2등 보상 : " + stage.crrRoom.gRoom.golds[1].ToString();
                break;

            case 2:
                Item.ItName[] itname = stage.crrRoom.iRoom.itmEnumList;
                string[] strarray = new string[5];
                    for (int i = 0; i < 5; i++)
                {
                    strarray[i] = itname[i].ToString();
                }
                value = stage.crrRoom.iRoom.getValues;

                roomInfo[2].text = "아이탬 방"
                    + "\n" + "목록 : " + string.Format($"[1]:{strarray[0]}, [2]:{strarray[1]}, [3]:{strarray[2]}, [4]:{strarray[3]}, [5]:{strarray[4]}")
                    + "\n" + "가치 : " + string.Format($"{value[0]}, {value[1]}, {value[2]}, {value[3]}, {value[4]}");
                break;

            case 3:
                value = stage.crrRoom.tRoom.LossValue;
                Stage.TrType tTyp = stage.crrRoom.tRoom.tTyp;
                string tstr = tTyp.ToString();

                //if (stage.crrRoom.tRoom.tTyp==Stage.TrType.GOLD)
                //{
                roomInfo[3].text = "함정 방"
                    + "\n" + "피해 타입" + tTyp
                    + "\n" + "피해" + string.Format($"{value[0]}, {value[1]}, {value[2]}, {value[3]}, {value[4]}")
                    + "\n" + "Max Power : 1, 2, 3, 4, 5";
                //}
                //else
                //{

                //}
                break;

            case 4:
                Stage.BoName boname = stage.crrRoom.bRoom.boName;
                string bstr = boname.ToString();
              
                if (stage.crrRoom.bRoom.IsDefeat)
                {
                    roomInfo[4].text = "보스 방" 
                        + "\n" + bstr
                        + "\n" + "Hp : " + stage.crrRoom.bRoom.hp[Controler.Player_num - 2].ToString()
                        + "\n" + "Damage : " + stage.crrRoom.bRoom.damage.ToString()
                        + "\n" + "Gold : " + stage.crrRoom.bRoom.gold.ToString();
                }
                else
                {
                    roomInfo[4].text = "보스 방 : " + bstr
                        + "\n" + "Damage : " + stage.crrRoom.bRoom.damage.ToString();
                }
                break;
               

            default:
                break;
        }

        //roomInfo.text=string.Format()
    }
}
