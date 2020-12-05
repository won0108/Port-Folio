using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    float timelimit;  //제한시간 변수
    public Text timetext;  //남은 시간 노출 변수
    bool bPaused=false;  //일시정지 변수

    void Start()
    {
        timelimit = 10f;  //게임 제한시간
        OnApplicationPause(bPaused);  //true 면 일시정지, false 계속 진행
    }

   
    void Update()
    {
        timetext.text = (int)timelimit + "SEC"; //실시간으로 남은시간 노출
        Invoke("time", 3);  //3초 대기 후 time 함수 실행
    }

    void time()
    {
        //시간이 0초가 아니면 감소
        if (timelimit > 0)
        {
            timelimit -= Time.deltaTime;
        }
        else if (timelimit <= 0) //제한시간이 0초 이하면 일시정지 및 GameOver 화면으로 전환
        {
            OnApplicationPause(bPaused);
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)  //제한시간 0일 때, 타일 클릭 거부
        {
            bPaused = true;
            Time.timeScale = 0;
            gameObject.GetComponent<Clicktile>().enabled = false;
        }
        else
        {
            if (bPaused)  //제한시간 남았을 때, 클릭 가능
            {
                bPaused = false;
                Time.timeScale = 1;
                gameObject.GetComponent<Clicktile>().enabled = true;
            }
        }
    }
    
}
