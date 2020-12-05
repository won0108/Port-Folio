using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //도움말 UI
    //결과창 : 1등 ~ 꼴등(탈출 불가한 사람) 
    //함정방 : 피해받는 대상 표시

    public Controler ctrl;  //인스펙터 창에서 GameControler와 스크립트 연결

    public LoadMap.LoadMap_State mapState;
    public bool Isbig;

    public LoadRoom.LoadRoom_State stLoad;
    public Text FloorNumberText;   //몇 층이고, 몇번째 방인지 노출

    [Header("SelectArea")]
    //public GameObject select;
    public Transform[] plaObj;
    public List<Transform> selectAreas;
    public float[,] item_effect = new float[,] { { -1.8f, -38f }, { -72.5f, -0.1f }, { -1.8f, 38f }, { 72.5f, 0f } };

    public Select.Select_State areaState;

    [Header("ArrowIndication")]
    public Transform Indication;
    public GameObject arrowImgPrefab;
    public float[,] arrow_Pos = new float[,] { { 0, -24}, { -57, 0}, { 0, 24}, { 57, 0} };    //57~54, -24~-23
    public int[] arrow_QuaterZ = { 90, 0, 270, 180 };
    public List<Transform> arrows = new List<Transform>();

    [Header("Ending Result")]
    public Transform EndingScore;
    public Text EscaperTest;
    public Text NonEscaperTest;


    private void Awake()
    {
        //selectList = new List<GameObject>();
        //select.SetActive(false);
        
        Indication = transform.Find("Indication").GetComponent<Transform>();  //하이러키에서 indication 을 찾아 위치를 가져온다
        FloorNumberText = Indication.GetComponentInChildren<Text>();  //indication 의 하위 text까지 전부 가져온다

        EndingScore = transform.Find("Ending").GetComponent<Transform>();   //하이러키에서 Ending 찾아서 위치 가져오기
    }

    private void Start()
    {
        LoadArrow();
        LoadSelectArea();
    }
    
    void LoadArrow()  //함정방에 의해 피해입을 플레이어를 가리킴
    {
        Transform Players = GameObject.Find("Players").transform;  //하이러키 Players 찾기
        plaObj = new Transform[Controler.Player_num];  //플레이어 위치 가져오기

        for (int i = 0; i < Controler.Player_num; i++)  //화살표 표시할 위치 가져오기
        {
            plaObj[i] = Players.GetChild(i);
            Transform tr = plaObj[i].Find("Info/Arrow");
            arrows.Add(tr);
        }
    }

    void LoadSelectArea()  //훔치기나 배신 카드 사용시, 피해입을 플레이어 선택위한 정보 저장
    {
        //Debug.Log("Load Areas");
        //Debug.Log($"{Controler.Player_num}");

        Transform Players = GameObject.Find("Players").transform;  //하이러키에서 Players 찾아서 위치 가져오기
        plaObj = new Transform[Controler.Player_num];

        for (int i = 0; i < Controler.Player_num; i++)
        {
            plaObj[i] = Players.GetChild(i);
            Transform tr = plaObj[i].Find("Info/Select");  //하이러키에서 Info/Select 위치 찾아서 위치 가져오기
            selectAreas.Add(tr);  //피해입힐 대상 저장
        }
    }


    public void FloorNumber()  //몇번째 층과 방을 text로 노출
    {
        FloorNumberText.text = "Floor  " + ctrl.floorNum.ToString()
            + "\n" + "Room " + (ctrl.roomNum+1).ToString();
    }

    public void ShowTappedPlayers(List<Player> trapped)  //함정 방에 걸린 player 활성화
    {
        for (int i = 0; i < trapped.Count; i++)
        {
            int idx = trapped[i].inven.pIdx;

            arrows[idx].gameObject.SetActive(true);
        }
    }

    public void HideArrow()  //함정 방에의해 피해입을 플레이어 가리키는 화살표 비활성
    {
        for (int i = 0; i < Controler.Player_num; i++)
        {
            if (arrows[i].gameObject.activeSelf)
            {
                arrows[i].gameObject.SetActive(false);
            }
        }
    }

    
    public void ShowSelectArea()  //피해주는 아이템 사용한 대상제외 모든 플레이어 SelectAreas 활성화
    {
        for (int i = 0; i < Controler.Player_num; i++)
        {
            if ( i != ctrl.turn)
            {
                selectAreas[i].gameObject.SetActive(true);
            }
        }
    }

    public void HideSelectArea()  //모든 플레이어 SelectAreas 비활성
    {
        for (int i = 0; i < Controler.Player_num; i++)
        {
            selectAreas[i].gameObject.SetActive(false);
        }
    }


    public void ShowUpResult()
    {
        List<string> str = new List<string>();

        for (int i = 0; i < ctrl.winners.Count; i++)  //금화를 가장 많이 가진 대상
        {
            int idx = ctrl.winners[i].inven.pIdx;     //해당 플레이어 찾기
            str.Add($" Player{ idx + 1} \n");         //해당 플레이어 저장
        }

        string display ="";
        foreach (var st in str)                      //탈출한 대상 노출
        {
            display = display.ToString() + st.ToString() ;
        }
        EscaperTest.text = display;
        str.Clear();

        for (int i = 0; i < ctrl.non_Escapers.Count; i++)   //탈출 못한 대상
        {
            int idx = ctrl.non_Escapers[i].inven.pIdx;
            str.Add($" Player {idx + 1} \n");
        }

        NonEscaperTest.text += "\n";
        for (int i = 0; i < str.Count; i++)   //탈출 못한 대상 노출
        {
            NonEscaperTest.text += str[i];
        }
        //Debug.Log(str.Count);

        EndingScore.gameObject.SetActive(true);  //엔딩 GameObject 활성화
    }
}
