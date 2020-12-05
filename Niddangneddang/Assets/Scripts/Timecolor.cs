using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timecolor : MonoBehaviour
{
    public Tileprefabs tp;  //생성한 tilemap 불러오는 변수
    //private Tileprefabs ottp;
    float timeSpan; //누적 시간
    float checkTime; //비교시간

    void Start()  //생성한 tilemap 불러오기
    {

        GameObject ottp = GameObject.Find("_obj");
        if (ottp != null)
            tp = ottp.GetComponent<Tileprefabs>();

        timeSpan = 0.0f;
        checkTime = 0.2f;  //0.2초 간격으로 컴퓨터 명령 실행
        
    }

    
    void Update()
    {
        timeSpan += Time.deltaTime;  //시간 측정
        
        // 누적시간이 2초 지날 시, ColorChange() 메소드 실행
        if(timeSpan>checkTime)
        {
            Invoke("ColorChange", 3);
            timeSpan = 0;  //누적시간 초기화
        }

    }

    void ColorChange ()  //컴퓨터 땅으로 변경
    {
            int i = Random.Range(0, 100);  //빙고판 랜덤 선택

            //플레이어가 클릭한 곳 컴퓨터가 클릭할 시, 플레이어와 컴퓨터 점수 변동 및 타일색 변경
            if (tp._tilelist[i].GetComponent<MeshRenderer>().material.color == Color.red)
            {
                tp._tilelist[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                Score.Player_s -= 1;
                Score.Enemy_s += 1;
            }
            //아무도 클릭하지 않은 곳 컴퓨터가 클릭 시, 컴퓨터 점수 변동 및 타일 색 변경
            else if (tp._tilelist[i].GetComponent<MeshRenderer>().material.color == Color.white)
             {
                 tp._tilelist[i].GetComponent<MeshRenderer>().material.color = Color.blue;
                Score.Enemy_s += 1;
             }
    }
}
