using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{   
    static public int Player_num;               //플레이어 인원 수
    

    public int turn;                            //현재 턴
    public int firstOrder;                      //한 방의 첫번째 턴
    public List<int> usingPwrOrder;             //파워 카드 사용한 순서 저장

    public enum State
    {
        Init,
        FloorSetting,
        RoomSetting,
        Room,
        ItemUsed,
        NextTurn,
        RoomEnd,
        FloorEnd,
        Exit,
        Ending,
        End
    }

    [HideInInspector]
    public List<Player> players;
    public List<Player> winners;
    public List<Player> non_Escapers;

    public GameManager gm;                  //인스펙트 창에서 GameManager와 스크립트 연결
    public UIManager ui;                    //인스펙트 창에서 UIMgr과 스크립트 연결 (무슨 방인지 알려주는 plane)

    public State state;                     //코루틴을 사용하기 위한 객체

    public Room[] floor;                    //한 층에 불러 올 방 5개
    public int floorNum;                    //층 수 1~5
    public int roomNum;                     //방 번호 0~4
    public bool[] pattern;                  //맵에서 보여줄 방에대한 정보 t/f 패턴
    
    private void Awake()
    {
        //Player_num = 4;         //임시값

        state = State.Init;  //state 상태 -1에서 init 으로 변경

        gm = GetComponent<GameManager>();
        ui = GameObject.Find("UIMgr").GetComponent<UIManager>();  //하이러키에 있는 UIMgr 찾아서 불러오기

        floor = new Room[5];
        floorNum = 1;
        roomNum = 0;
        pattern = new bool[5];

        usingPwrOrder = new List<int>(); //파워카드 사용한 숫자 저장
        players = new List<Player>();
        non_Escapers = new List<Player>();
        gm.PlayerSetting(Player_num);  //GameManager 스크립트 > PlayerSetting 함수 호출 : 플레이어 인원수대로 세팅
    }//Awake

    void Start()
    {
        StartCoroutine(Init());  //코루틴으로 init() 함수 호출
    }

    IEnumerator Init()                     //게임 준비, ->랜덤 방 셋팅?
    {
        //Opening UI

        while (state==State.Init)
        {
            int[] classArray = gm.ClassSetting();  //셔플 된 4개 직업

            for (int i = 0; i < Player_num; i++)
            {
                players[i].GetClass(classArray[i]); //player 스크립트 호출 > GetClass 함수 호출을 통해 직업 세팅
                //직업에 따라 hp, gold 가 자동으로 인스펙터 창에 노출
                players[i].PlaStatText.text = "Hp : " + players[i].hp.ToString() + "   Gold : " + players[i].gold.ToString();
            }
            firstOrder = gm.RandomFirst(); //GameManager 스크립트 > RandomFirst 함수 호출 > Player 인원수에 따라 첫게임에 순서가 랜덤으로 지정

            state = State.FloorSetting;   //State 를 FloorSetting으로 변경
            yield return null;            // update() 가 먼저 실행된 후 FloorSetting 코루틴 실행
        }
        NextState();  //FloorSetting 실행
    }

    IEnumerator FloorSetting()         //다음 층으로 올라간다.
    {
        Debug.Log("Floor Setting");
        ui.FloorNumber();             // 몇번째 층과 방인지 노출

        while (state==State.FloorSetting)  //state가 FloorSetting 이면 실행
        {
            pattern = gm.RandShowPatt();  //GameManager 스크립트 > 한 층에 보여질 방 패턴 ex.ttfft
            gm.RoomSetting(floor);        //GameManager 스크립트 > RoomSetting 호출 > 확률로 방 세팅

            ui.mapState = LoadMap.LoadMap_State.Loading;  // LoadMap 스크립트 > Loading 상태 저장

            state = State.RoomSetting;  //State를 RoomSetting 으로 변경
            yield return null;
        }
        NextState();   //RoomSetting  시작
    }

    IEnumerator RoomSetting()               //다음 방으로 들어간다.
    {
        Debug.Log($"Roon Setting");
        ui.stLoad = LoadRoom.LoadRoom_State.Ready;  //UIManager 스크립트 > Ready 상태 저장
        ui.FloorNumber();   //UIManager 스크립트 > FloorNumber 함수 호출 > 몇번째 층과 방인지 노출

        while (state==State.RoomSetting)  //현 상태 RoomSetting 이면
        {
            gm.EnterRoom();             //GameManager 스크립트 > EnterRoom 함수 호출 > 함정방인지 확인
            turn = firstOrder;          //게임 시작 Player 순서
            //방 진입 관련 UI이벤트 
            players[turn].inven.MyTurn();  //Inven 스크립트 > 시작 player board 색 변경

            state = State.Room;         //현 상태 Room으로 변경
            yield return null;
        }
        NextState();   //Room 시작
    }

    IEnumerator Room()
    {
        Debug.Log($"Room");
        while (state == State.Room)  //현 상태 Room이면
        {
            if (players[turn].moreturn)                              //selfActive 활성 비활성화로 체크 변경
            {
                gm.Active(players[turn].inven, floor[roomNum].rTyp); //GameManager > 사용 가능한 카드 활성화
            }
            else
                state = State.NextTurn;  //현상태 NextTurn 변경
            yield return null;
        }//while
        NextState();  //NextTurn 시작
    }

    IEnumerator ItemUsed()
    {
        Debug.Log($"ItemUsed");
     
        while (state == State.ItemUsed)
        {
            if (gm.field[turn].fItmObj.gameObject.activeSelf)       //아이탬 사용시 효과발동
            {
                Item.ItName itname = gm.field[turn].fItem.itName;
                gm.ItemActuration(itname);
                //yield return null;

                players[turn].inven.DeleteUsedItem();
                yield return new WaitForSeconds(0.1f);
                
                //if(players[turn].moreturn)                             //필드 검사 -> 상태 변환
                state = (State)gm.field[turn].WhatsOnFiled();
            }
            yield return null;
        }

        NextState();
    }

    IEnumerator NextTurn()
    {
        Debug.Log($"Next Turn");
        int nextTurn = gm.NextTurn(turn, Player_num);

        while (state==State.NextTurn)
        {
            bool isMore = IsMoreTurn();

            if (isMore == false)  //현재 방에 모든 player가 카드를 냈을 경우
            {
                players[turn].inven.TurnEnd();  //inven> board 검정색으로 변경
                state = State.RoomEnd;  //현 상태 RoomEnd 로 변경
            }
            else  //아직 카드를 안낸 player 존재
            {
                players[turn].inven.TurnEnd();  //현재 player 턴이 아닌 경우 board 검정색 변경

                turn = nextTurn;   //그 다음 순서
                players[turn].inven.MyTurn();  //해당 순서 player boar 색 변경

                state = State.Room;  //현 상태 room으로 변경
            }

            yield return null;
        }

        NextState();  //해당하는 state 시작
    }

    IEnumerator RoomEnd()  //다음 방 또는 층 종료를 확인
    {
        while (state == State.RoomEnd)  //현상태가 RoomEnd일 때
        {
            Debug.Log($"Roon End");
            gm.RoomResult(floor[roomNum].rTyp);  //GameManager > 각 해당하는 방 결과 공유
            yield return new WaitForSeconds(1f);  //1초 기다리기

            gm.RoomEnd();                  //GameManager > 필드 초기화, firstOrder 정하기.

            usingPwrOrder.Clear();        //사용한 파워카드 리셋
            ui.stLoad = LoadRoom.LoadRoom_State.UnLoad;  //현재상태 Unload

            roomNum = (int)(roomNum + 1) % 5;                   //다음 방
            if (roomNum == 0)
            {
                state = State.FloorEnd;               //층 종료 및 상태 FloorEnd
            }
            else
                state = State.RoomSetting;                      //아직 층이 안끝났다. 다음 방으로 이동.

            yield return null;
        }
        NextState();  //해당하는 상태 시작
    }

    IEnumerator FloorEnd()                  //층이 종료된다.
    {
        Debug.Log($"Floor End");

        while (state==State.FloorEnd)
        {
            gm.FloorEnd();                              //층 초기화, 힘 초기화

            if (floorNum == 5)                          //현재 5층이었으면 탈출
                state = Controler.State.Exit;
            else                                        //다음 층으로 이동
            {
                floorNum++;             
            
                state = State.FloorSetting;             //상태 floorSetting
            }

            yield return null;
        }
        NextState();  //해당하는 상태 시작
    }

    IEnumerator Exit()    //탈출 > 5층일 때
    {
        Debug.Log("Exit");

        while (state == State.Exit)   //State가 Exit일 때
        {
            gm.Exit();                //GameManager > hp가 0이상 대상 탈출
            gm.EndResult();           //GameManager > 탈출 또는 탈출 못한 대상 찾아서 결론내기


            state = State.Ending;    //상태 Ending
            yield return null;
        }
        NextState();                //Ending 시작
    }

    IEnumerator Ending()
    {
        Debug.Log("Ending...");

        while (state == State.Ending)
        {
            ui.ShowUpResult();    //UIManager > 엔딩 UI 노출

            state = State.End;   //상태 End
            yield return null;
        }
        NextState();   //End 시작
    }

    IEnumerator End()
    {
        Debug.Log("End");
        yield return null;
        Application.Quit();  //게임 종료
    }

    protected void NextState()          //state에 따라서 코루틴 함수 실행 시켜줌
    {
        string methodName = state.ToString();// + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    bool IsMoreTurn()                   //수정구 사용한 사람이 있는지 검사
    {
        for (int i = 0; i < Player_num; i++)
        {
            if (players[i].moreturn)
                return true;
        }
        return false;
    }
    
}