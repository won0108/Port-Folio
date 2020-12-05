using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Controler ctrl;              // Controler 클래스 연결
    public UIManager ui;
   
    public List<Field> field;           // 플레이어들이 낸 카드 게임오브젝트와 연결
    public Stage stage;                 // stage 클래스 연결  하이러키 Room 게임오브젝트와 연결

    public int classNum = 4;            //직업 개수

    public bool[,] showPat = {  { true,true,false,true,false},      //한 층에서 보여질 방의 패턴 배열
                                { true,true,false,false,false},
                                { true,false,true,false,false},
                                { true,true,true,true,false},
                                { true,false,false,true,false},
                                { true,false,false,false,true},
                                { true,true,false,false,true} };

    public float[,] player_pos = new float[,] { { 0f, 0f }, { -33f, 0f }, { 0f, 0f }, { 33f, 0f } };
    public float[,] Field_pos = new float[,] { { 30f, -22.5f }, { 24.5f, 5f }, { 30f, -22.5f }, { 24.5f, 5f } };

    public GameObject PlayerPrefab; //인스펙터 창에서 player 프리팹과 연결
    public GameObject PlayersPosition;

    private void Awake()
    {
        ctrl = GetComponent<Controler>();

        ui = GameObject.Find("UIMgr").GetComponent<UIManager>();
        stage = GameObject.Find("Room").GetComponent<Stage>();
        PlayersPosition = GameObject.Find("Players"); //하이러키 Players와 연결 : 0, 0, 0

        field = new List<Field>();     //인원 수 만큼 필드 배열생성
    }//Awake
    
    private void Start()
    {
        for (int i = 0; i < Controler.Player_num; i++)
        {
            field.Add(ctrl.players[i].fieldObj.GetComponent<Field>());
        }
    }


    //초기 셋팅 관련 함수들 (플레이어 세팅)
    public void PlayerSetting(int player_num)
    {
        //플레이어의 사람 수에 따라서 프리펩을 다르게 생성한다.
        //플레이어가 위치를 선택한다.Player_num
        
        float rotate = -90.0f;

        int length = 0;
        int sum = 0;

        switch (player_num)
        {
            case 2:  //플레이어가 2명일 때
                length = 4;
                sum = 2;
                break;
            case 3:
                length = 3;
                sum = 1;
                break;
            case 4:
                length = 4;
                sum = 1;
                break;
            default:
                break;
        }
        int c = 0;
        for (int i = 0; i < length; i += sum)
        {
            //PlayerPrefab(player)과 PlayerPosition 위치를 가진 p_obj GameObject 생성
            GameObject p_obj = Instantiate(PlayerPrefab, PlayersPosition.transform) as GameObject;
            p_obj.transform.localRotation = Quaternion.Euler(0f, 0f, rotate * i); //2명 : z축이 0, -180 / 3명 : 추가로 -90 / 4명 : 추가로 90
            //4명일 경우, {0,0,0},{-33,0,0},{0,0,0},{33,0,0}
            p_obj.transform.localPosition = new Vector3(player_pos[i, 0], player_pos[i, 1], 0f);

            Transform f_obj = p_obj.transform.Find("Field");  //하이러키에서 Field 이름을 가진 객체 찾기
            //4명일 경우, {30,-22.5,0} , {24.5,5,0} , {30,-22.5,0} , {24.5,5,0} : 색상으로 순서 노출해주는 board
            f_obj.localPosition = new Vector3(Field_pos[i, 0], Field_pos[i, 1], 0f);  

            Field _fd = f_obj.GetComponent<Field>(); //Field 객체 생성 및 Board와 연결
            _fd.FieldMaker(c); //함수 호출

            Player _player = p_obj.GetComponent<Player>();  //Player 객체 생성 및 p_obj와 연결
            _player.inven.pIdx = c;  //플레이어의 pldx 값이 c 값으로 초기화
            _player.inven.InvenMake(); //함수 호출 : inven 스크립트
            _player.PlrText.text = "Player " + (c + 1).ToString(); //몇번째 플레이어인지 노출
            ctrl.players.Add(_player);  //하이러키 players에 _player 추가
            c++;
        }
    }// end PlayerSetting();

    public int[] ClassSetting()  //4개 직업 랜덤 배분을 위한 기본 세팅
    {
        int[] classArray = new int[classNum]; //크기가 4인 배열 생성

        for (int i = 0; i < classNum; i++)
        {
            classArray[i] = i; //{0,1,2,3}
        }

        int RangeMax = classNum;  //4
        int temp = 0;
        for (int i = classNum - 1; i >= 0; i--)
        {
            int _idx = Random.Range(0, RangeMax); //램덤 수 
            temp = classArray[i];  //현재 classArray[i] 값을 temp에 저장
            classArray[i] = classArray[_idx];  //현재 classArray[i]에 랜덤 위치 값 저장
            classArray[_idx] = temp; //temp 값 랜덤 자리에 저장
            RangeMax--;  //랜덤 수 감소
        }

        return classArray;  //4개의 값 리턴
    }

    public bool[] RandShowPatt()          //Map에 보여지는 정보 t/f 패턴 랜덤 뽑기
    {
        bool[] newPat = new bool[5];     //한 층에 총 5개의 방

        int idx = (int)Random.Range(0f, 7f);  // 0~6까지 랜덤수 뽑기
        for (int i = 0; i < 5; i++)
        {
            newPat[i] = showPat[idx, i];    //한 층에 보여질 방 패턴
        }
        return newPat;
    }//RandShowPatt

    public void RoomSetting(Room[] frooms)      //한 층에 구성 될 5개의 방 셋팅
    {
        for (int i = 0; i < 5; i++)
        {
            stage.rooms[i].showupMap = ctrl.pattern[i];     //Map에 보여질 패턴에 따라 변수값 설정
            //int idx = (ctrl.floorNum-1)*5 + i;
            frooms[i] = stage.rooms[i];           //확률로 방세팅
            //stage.rooms.Remove(stage.rooms[0]);
        }
    }//RoomSetting

    public int RandomFirst()                    //랜덤 첫 선 정하기
    {
        return (int)Random.Range(0f, Controler.Player_num);
    }//RandomFirst

    public void EnterRoom()                     //방에 진입한다.  
    {
        stage.crrRoom = ctrl.floor[ctrl.roomNum];   //현재 몇 층 몇번 방
        ui.stLoad = LoadRoom.LoadRoom_State.Loading;  //LoadRoom 스크립트 > Loading 상태

        if(stage.crrRoom.rTyp==Stage.roomType.TRAP)   //현재 Stage 가 Trap 이면
        {
            List<Player> trapped = new List<Player>();  //리스트로 player 객체 생성
            if (stage.crrRoom.tRoom.tTyp==Stage.TrType.GOLD)  //골드 함정 방일 때
            {
                trapped = GoldMax(ctrl.players);  //GoldMax 함수 호출
                ui.ShowTappedPlayers(trapped);    //UIManger 스크립트 > 최대 금화 가진 Player 활성화
            }
            else                                //hp 함정 방일 때
            {
                trapped = HpMax(ctrl.players);   //HPMax 함수 호출
                ui.ShowTappedPlayers(trapped);   //UIManger 스크립트 > 최대 체력 가진 Player 활성화
            }
        }
    }


    //카드 선택 관련 함수들
    public void Active(Inven inven, Stage.roomType rTyp)            //카드를 내기 전
    {
        for (int i = 0; i < inven.powers.Length; i++)
        {
            if (inven.powers[i].used == false)   //카드 사용 안됬으면
            {
                inven.powers[i].canUse = true;  //사용할 수 있는 파워 카드 활성화
            }
        }

        int idx = IsItemHas(inven.item_objs);   //아이템 카드 활성화
        //마지막 턴 수정구슬 비활성화
        if(idx != -1)
        {
            switch (rTyp)
            {
                case Stage.roomType.MON: //몬스터 방일 때
                    for (int i = 0; i < idx+1 ; i++)  //검,훔치기,수정구,배신 카드 활성화
                    {
                        if (inven.item[i].itName == Item.ItName.SWORD
                            || inven.item[i].itName == Item.ItName.STEAL
                            || inven.item[i].itName == Item.ItName.CRYSTAL
                            || inven.item[i].itName == Item.ItName.TREASON)
                        {
                            inven.item[i].canUse = true;
                        }
                    }
                    break;//MON

                case Stage.roomType.GOLD:  //금화방일 때
                    for (int i = 0; i < idx + 1; i++)   //훔치기, 수정구, 배신 카드 활성화
                    {
                        if ( inven.item[i].itName == Item.ItName.STEAL
                            || inven.item[i].itName == Item.ItName.CRYSTAL
                            || inven.item[i].itName == Item.ItName.TREASON)
                        {
                            inven.item[i].canUse = true;
                        }
                    }
                    break;//GOLD

                case Stage.roomType.ITEM:  //아이템 방일 때
                    for (int i = 0; i < idx + 1; i++)   //열쇠, 훔치기, 수정구, 배신 활성화
                    {
                        if ( inven.item[i].itName == Item.ItName.KEY
                            || inven.item[i].itName == Item.ItName.STEAL
                            || inven.item[i].itName == Item.ItName.CRYSTAL
                            || inven.item[i].itName == Item.ItName.TREASON)
                        {
                            inven.item[i].canUse = true;
                        }
                    }
                    break;//ITEM

                case Stage.roomType.TRAP:   //함정방일 때
                    for (int i = 0; i < idx + 1; i++) //훔치기, 수정구, 배신 활성화
                    {
                        if ( inven.item[i].itName == Item.ItName.STEAL
                            || inven.item[i].itName == Item.ItName.CRYSTAL
                            || inven.item[i].itName == Item.ItName.TREASON)
                        {
                            inven.item[i].canUse = true;
                        }
                    }
                    break;//TRAP

                case Stage.roomType.BOSS:  //보스방일 때
                    for (int i = 0; i < idx + 1; i++)  //검, 훔치기, 수정구, 배신 활성화
                    {
                        if ( inven.item[i].itName == Item.ItName.SWORD
                            || inven.item[i].itName == Item.ItName.STEAL
                            || inven.item[i].itName == Item.ItName.CRYSTAL
                            || inven.item[i].itName == Item.ItName.TREASON)
                        {
                            inven.item[i].canUse = true;
                        }

                        if(stage.crrRoom.bRoom.boName==Stage.BoName.DRAGONKING)  //보스가 드레곤킹일 때
                        {
                            if (inven.item[i].itName == Item.ItName.KEY)  //키카드만 활성화
                            {
                                inven.item[i].canUse = true;
                            }
                        }
                    }

                    break;//BOSS
                default:
                    break;
            }
        }
        
    }//Active

    int IsItemHas(Transform[] itemList)       //활성화 아이템 마지막 인덱스
    {
        int Count = -1;
        for (int i = 0; i < 4; i++)           //사용 가능한 아이템 카드 inven 활성화
        {
            if (itemList[i].gameObject.activeSelf)
            {
                Count = i;
            }
        }
        return Count;
    }

    public void ItemActuration(Item.ItName itname)                  //아이탬 사용시 효과 발동 함수
    {
        switch (itname)
        {
            case Item.ItName.SWORD:
                field[ctrl.turn].fPower.power = 5;                      //필드 파워 영역 5로 변경
                field[ctrl.turn].fPwrObj.gameObject.SetActive(true);    //파워 활성화

                field[ctrl.turn].fItmObj.gameObject.SetActive(false);   //필드 아이탬 비활성화
                field[ctrl.turn].fItem.itName = Item.ItName.NONE;       //사용된 카드정보 초기화

                ctrl.usingPwrOrder.Add(ctrl.turn);
                ctrl.players[ctrl.turn].moreturn = false;               //턴 종료
                break;

            case Item.ItName.KEY:
                //보스방에서 사용한 경우 -> 즉시 탈출
                if(stage.crrRoom.rTyp == Stage.roomType.BOSS)
                {
                    //탈출 이미지
                    ctrl.players[ctrl.turn].IsExit();
                    ctrl.players[ctrl.turn].moreturn = false;
                    break;
                }

                //파워카드로 전환(보스 방 아닌 다른 방에서 사용할 시)
                field[ctrl.turn].fPower.power = 5;  
                field[ctrl.turn].fPwrObj.gameObject.SetActive(true);

                field[ctrl.turn].fItmObj.gameObject.SetActive(false);
                field[ctrl.turn].fItem.itName = Item.ItName.NONE;

                ctrl.usingPwrOrder.Add(ctrl.turn);  //파워 카드 사용한 순서 저장
                ctrl.players[ctrl.turn].moreturn = false;
                break;

            case Item.ItName.STEAL:  //코루틴을 통해 함수 실행
                StartCoroutine(WaitForInput(itname));
                
                break;

            case Item.ItName.CRYSTAL:
                break;

            case Item.ItName.TREASON:  //코루틴 통해 함수 실행
                StartCoroutine(WaitForInput(itname));
                
                break;

            default:
                break;
        }

    }


    //공격할 플레이어 선택 관련 함수
    IEnumerator WaitForInput(Item.ItName itname)                    //마우스 선택 받을때까지 대기
    {
        ui.ShowSelectArea();  //아이템 카드 사용한 대상 제외 selectAreas 노출

        while (ui.areaState == Select.Select_State.Selecting)  //상태가 Selecting 일 때
        {
            //Debug.Log("Selecting....");
            Item item = field[ctrl.turn].fItem;  
            foreach (var obj in ui.selectAreas)  
            {
                obj.GetComponent<Select>().LoadInfo(item.itName);  //Select > 금화, 체력 뺏어오는 것을 Text로 같이 나타냄
            }

            yield return null;
        }

        while (ui.areaState == Select.Select_State.Selected)        //이벤트처리 
        {
            Who(itname);  //금화, 체력 상승/하락 나타냄

            ui.areaState = Select.Select_State.Ready;  //상태 ready 로 변동
            yield return null;
        }

        ui.HideSelectArea();  //selectAreas 비활성

    }

    void Who(Item.ItName itname)
    {
        Debug.Log("Who is....");
        for (int i = 0; i < Controler.Player_num; i++)
        {
            Select sel = ui.selectAreas[i].GetComponent<Select>();
            if (sel.selected)  //선택받은 대상
            {
                if (itname == Item.ItName.STEAL) //훔치기 경우 피해자 금화 깍이고, 아이템 사용자 금화 상승
                {
                    ctrl.players[i].GoldMgr(-2);
                    ctrl.players[ctrl.turn].GoldMgr(2);
                }
                else if (itname == Item.ItName.TREASON)  //배신 경우, 피해자 데미지, 아이템 사용자 체력 회복
                {
                    ctrl.players[i].HpMgr(-2);
                }
                sel.selected = false;
            }
        }
    }


    //방 이벤트 종료 관련 함수들
    public int NextTurn(int turn, int num)              //일반적으로 num = Controler.Player_num
    {
        return (int)(turn + 1) % num;
    }//NextTurn

    public void RoomResult(Stage.roomType rTyp)         //방의 결과 이벤트를 호출
    {
        List<Player> _pls = new List<Player>();         //효과 적용 받을 플레이어들의 리스트
        List<Player> _pls2 = new List<Player>();        //다른 효과 적용 받을 플레이어들의 리스트
        List<int> Mx = new List<int>();
        List<int> Mx2 = new List<int>();

        int sum = 0;
        int hp = 0;
        List<int> idx = new List<int>();

        switch (rTyp)
        {
            case Stage.roomType.MON:  //데미지 받을 플레이어(몬스터를 물리치지 못했을 시, 가장 적은 파워를 낸 플레이어 데미지)
                hp = ctrl.floor[ctrl.roomNum].mRoom.hp[Controler.Player_num - 2]; //해당 몬스터를 찾아 xml파일에서 player 인원수 대로 몬스터를 물리칠 수 있는 hp 추출
                sum = Sum();        //player 들의 파워 합계
                if (sum < hp)    //파워 합계가 몬스터 hp 보다 작을 경우
                {
                    _pls = Min();  //가장 적은 파워를 낸 player 나 players 를 색출
                    ctrl.floor[ctrl.roomNum].players = _pls;  //가장 적은 파워 낸 player 저장
                }
                break;//MON

            case Stage.roomType.GOLD:   //1등 보상 받을 플레이어 (가장 높은 파워 카드를 낸 플레이어)
                Mx = Max(field);   //가장 높은 파워 낸 player 찾기
                int num = Mx.Count;  
                for (int i = 1; i < num; i++)
                {
                    _pls.Add(ctrl.players[ Mx[i] ]);  //가장 높은 파워 낸 player 와 players 저장
                }
                ctrl.floor[ctrl.roomNum].players = _pls;

                //2등 보상이 있는 경우 (두번째로 높은 파워낸 player)
                if (ctrl.floor[ctrl.roomNum].gRoom.golds[1] != 0 && Mx.Count != Controler.Player_num + 1)
                {
                    Mx2 = SndMax(Mx);  //두번째로 높은 파워낸 player
                    if (Mx2 != null)
                    {
                        int num2 = Mx2.Count;
                        for (int j = 1; j < num2; j++)
                        {
                            _pls2.Add(ctrl.players[ Mx2[j] ]);   //두번째로 높은 파워낸 player와 players 저장
                        }
                    
                        ctrl.floor[ctrl.roomNum].players2 = _pls2;
                    }
                }
                break;//GOLD

            case Stage.roomType.ITEM:  //아이템 방
                for (int i = 0; i < field.Count; i++)  //각 플레이어들이 어떤 파워를 냈는지 저장
                {
                    ctrl.floor[ctrl.roomNum].iRoom.fInfo.Add( field[i].fPower );
                }
                    ctrl.floor[ctrl.roomNum].players=ctrl.players;
                break;//ITEM

            case Stage.roomType.TRAP:                   //피해 대상 플레이어들
                if(ctrl.floor[ctrl.roomNum].tRoom.tTyp == Stage.TrType.GOLD)
                    _pls = GoldMax(ctrl.players);  //금화 함정 방일 때 피해 입을 플레이어
                else
                    _pls = HpMax(ctrl.players);   //데미지 방일 때 피해 입을 플레이어

                Mx = Max(field);  //가장 높은 파워 카드 숫자 찾기
                ctrl.floor[ctrl.roomNum].tRoom.max = Mx[0];  
                ctrl.floor[ctrl.roomNum].players = _pls;
                //피해 받을 플레이어

                break;//TRAP

            case Stage.roomType.BOSS:
                //처치가능 _ 몬스터방과 동일_피해 입을 플레이어들
                if (ctrl.floor[ctrl.roomNum].bRoom.IsDefeat)
                {
                    hp = ctrl.floor[ctrl.roomNum].bRoom.hp[Controler.Player_num - 2];  //몬스터를 물리칠 수 있는 hp
                    sum = Sum();  //파워 합
                    if (sum < hp)
                    {
                        //데미지 받을 플레이어
                        _pls = Min(); 
                        ctrl.floor[ctrl.roomNum].players = _pls;
                    }
                }
                //처치불가능
                if (ctrl.floor[ctrl.roomNum].bRoom.boName == Stage.BoName.OBERON)
                    ctrl.floor[ctrl.roomNum].players = Oberon();
                if (ctrl.floor[ctrl.roomNum].bRoom.boName == Stage.BoName.DRAGONKING)
                {
                    //데미지 받을 플레이어
                    _pls2 = Min();
                    ctrl.floor[ctrl.roomNum].players2 = _pls2;
                }
                break;//BOSS

            default:
                break;
        }
        ctrl.floor[ctrl.roomNum].Result();  //Stage > 해당 방에 있는 Result 함수 호출 (오버라이드)
        ui.HideArrow();  //UIManager >  함정방 피해 입을 대상 화살표 숨기기
    }//RoomResult

    public void RoomEnd()
    {
        List<int> m = Max(field);                  //다음 방 선 정하기 (가장 큰 파워 낸 대상 선정)
        int idx = m.Count;
        ctrl.firstOrder = m[idx-1];

        for (int i = 0; i < Controler.Player_num; i++)              //field초기화
        {
            field[i].ResetState();
        }

        for (int i = 0; i < Controler.Player_num; i++)  //본인 차례에 행동할 수 있는지 확인
        {
            ctrl.players[i].moreturn = true;
        }
    }//RoomEnd

    public void FloorEnd()                      
    {
        ctrl.floor.Initialize();                //ctrl.floor[] 초기화
        for (int i = 0; i < 5; i++)             //지난 방 정보 삭제
        {
            stage.rooms.RemoveAt(0);
        }
        
        //power 초기화
        for (int i = 0; i < Controler.Player_num; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                ctrl.players[i].inven.powers[j].ResetState();
            }
        }
    }//FloorEnd


    //보스방 메소드
    public List<Player> Oberon()
    {
        List<Player> pl = new List<Player>();
        for (int i = 0; i < field.Count; i++)       //파워 5를 1로 바꾼다. ( 검카드 사용시, 이미 파워5로 대체 됨 )
        {
            if (field[i].fPower.power == 5)
            {
                field[i].fPower.power = 1;
                field[i].fPower.InfoReset();
            }
        }
        pl = Min();
        
        return pl;
    }//Oberon


    //탈출 관련 함수들
    public void Exit()                                  //hp > 0 일단 모두 탈출
    {
        for (int i = 0; i < Controler.Player_num; i++)
        {
            if (ctrl.players[i].hp > 0)
            {
                ctrl.players[i].exit = true;
            }
        }
    }

    public void EndResult()
    {
        List<Player> exitPls = new List<Player>();      //탈출한 플레이어들
        List<Player> loosers = new List<Player>();      //탈출 못 한 플레이어들
        List<Player> hpminers = new List<Player>();     //hp가장 낮은 사람도 탈출불가

        for (int i = 0; i < Controler.Player_num; i++)
        {
            if (ctrl.players[i].exit)                 //탈출 성공한 대상 확인 및 저장
                exitPls.Add(ctrl.players[i]);  
            else                                      //탈출 하지 못한 대상 확인 및 저장
                loosers.Add(ctrl.players[i]);
        }

        if (exitPls.Count > 1)                          //탈출자 2명 이상 있을 경우 ->  hp최소 탈출실패
        {
            hpminers = HpMin(exitPls);                  //hp 낮은 대상 찾아서 저장
            for (int i = 0; i < hpminers.Count; i++)
            {
                hpminers[i].exit = false;               //탈출 비활성
                loosers.Add(hpminers[i]);               //탈출 못한 대상에 저장
                exitPls.Remove(hpminers[i]);            //탈출 대상 명단에 hp 낮은 대상 삭제
            }
        }

        List<Player> goldMaxexit = exitPls;
        if (exitPls.Count > 1)                          //탈출자가 있을 경우
            goldMaxexit = GoldMax(exitPls);             //탈출자 중 골드 제일 많은 사람들
        //1등 = HpMax(goldMaxexit);                     //여럿일 경우 Hp 제일 많은 사람들
        //2~3등 줄세우기


        ctrl.winners = goldMaxexit;                    //금화 많은 대상 저장
        
        for (int i = 0; i < loosers.Count; i++)        //탈출하지 못한 대상 저장
        {
            ctrl.non_Escapers.Add(loosers[i]);     //hp <= 0 || 최소값
        }
    }


    //연산자
    int Sum()                                    //몬스터를 처치하기 위해 power의 수를 덧셈
    {
        int result = 0;

        for (int i = 0; i < field.Count; i++)
        {
            result += field[i].fPower.power;
        }
        return result;
    }//Sum

    List<int> Max(List<Field> arry)              //max[0]에는 큰 값을 저장, 나머지는 동일한 max 값을 가진 player를 저장
    {
        List<int> maxs = new List<int>();       

        int max = arry[0].fPower.power;
        for (int i = 0; i < arry.Count; i++)
        {
            max = arry[i].fPower.power >= max ? arry[i].fPower.power : max;   //함정일 때 필요
        }
        maxs.Add(max);

        int j = 0;
        for (int i = 0; i < ctrl.usingPwrOrder.Count; i++)  //동일한 max 값을 가진 player를 찾기 (금화방에서 max를 낸 player끼리 금화를 나누기 위해 필요)
        {
            j = ctrl.usingPwrOrder[i];
            if (max == field[j].fPower.power)
            {
                maxs.Add(j);
            }
        }
        return maxs;
    }//Max

    List<int> SndMax(List<int> max)              //금화방에서 두 번째로 큰 수를 가진 player를 찾아서 금화를 나누기위해 필요
    {
        List<int> sndMax = new List<int>();
        List<Field> temp = new List<Field>();

        int j = 0;                      
        for (int i = 0; i < ctrl.usingPwrOrder.Count; i++)
        {
            j = ctrl.usingPwrOrder[i];              //파워카드 낸 순서대로 Max값 보다 작은 수의 필드리스트
            if (field[j].fPower.power < max[0])     
                temp.Add(field[j]);
        }
        if(temp!=null)
            sndMax = Max(temp);                         //두 번째 큰 수를 낸 player들을 찾기

        return sndMax;
    }//SndMax

    List<Player> Min()                           //피해토큰 얻을 때 최소값을 낸 player 다수 일경우 모두에게 적용해야함.
    {
        List<Player> mins = new List<Player>();           //최소값을 낸 player를 저장

        int min = field[0].fPower.power;
        for (int i = 0; i < field.Count; i++)                   //min값 찾기
        {
            if(field[i].fPwrObj.gameObject.activeSelf)          //활성화 되어있는 것 중에서 최솟값 찾기
                min = field[i].fPower.power <= min ? field[i].fPower.power : min;
        }
        for (int i = 0; i < field.Count; i++)       //최솟값을 낸 player들 찾기
        {
            if (min == field[i].fPower.power)
            {
                mins.Add(ctrl.players[i]);            //최솟값 낸 player 나 players 저장      
            }
        }
        return mins;
    }//Min

    List<Player> HpMin(List<Player> Pls)          
    {
        List<Player> mins = new List<Player>();         //최소값을 낸 player를 저장

        int min = Pls[0].hp;
        for (int i = 0; i < Pls.Count; i++)             //min값 찾기
        {
            min = Pls[i].hp <= min ? Pls[i].hp : min;
        }
        for (int i = 0; i < Pls.Count; i++)             //최솟값을 낸 player들 찾기
        {
            if (min == Pls[i].hp)
            {
                mins.Add(Pls[i]);
            }
        }
        return mins;
    }//Min

    List<Player> GoldMax(List<Player> players)   //함정방에서 금화를 가장 많이 가진 사람의 금화 갯수를 깍기위해 필요
    {
        List<Player> goldMaxPlayers = new List<Player>();

        int gold_max = players[0].gold;  //최대 금화를 가진 player 를 찾기 위해 필요
        for (int i = 0; i < players.Count; i++)  
        {
            gold_max = gold_max <= players[i].gold ? players[i].gold : gold_max;  //금화를 가장 많이 가지고 있는 player 찾기
        }
        for (int i = 0; i < players.Count; i++)  //최대 금화 가진 player를 goldMaxPlayers에 저장
        {
            if (gold_max == players[i].gold)
            {
                goldMaxPlayers.Add(players[i]);
            }
        }

        return goldMaxPlayers;
    }//GoldMax

    List<Player> HpMax(List<Player> players)     //함정방에서 피해토큰은 가장 적게 받은 사람은 hp가 가장 많은 사람이므로 hpmax인 player 찾기
    {
        int hp_max = players[0].hp;
        List<Player> hpMaxPlayers = new List<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            hp_max= hp_max <= players[i].hp  ? players[i].hp : hp_max;
        }
        for (int i = 0; i < players.Count; i++)
        {
            if(hp_max== players[i].hp)
            {
                hpMaxPlayers.Add(players[i]);
            }
        }
        return hpMaxPlayers;
    }//HpMax
}