using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject GameStart;
    public GameObject PlayerSelect;
    public GameObject PBtn1;
    public GameObject PBtn2;
    public GameObject PBtn3;
    public GameObject StartBtn;
    public List<GameObject> Btn = new List<GameObject>(); //gameobject 배열 생성
    
    void Start()
    {
        Btn.Add(PBtn1); //2명이서 게임할 때
        Btn.Add(PBtn2); //3명
        Btn.Add(PBtn3); //4명
        GameStart.SetActive(false);
        
    }

    public void SelectPBtn1()  //플레이어 두명 선택할 때
    {
        if (Btn[0].activeSelf == false) //0번째 버튼 클릭 > 버튼 비활성이면
        {
            Controler.Player_num = 2; //Controler class에 Player 수 2명 전달
            PlayerSelect.SetActive(false); //인원수 선택 창이 비활성
            GameStart.SetActive(true); //GameStart 창이 활성

        }
    }

    public void SelectPBtn2()  //플레이어 3명 선택 시
    {
        if (Btn[1].activeSelf == false)  
        {
            Controler.Player_num = 3; 
            PlayerSelect.SetActive(false);
            GameStart.SetActive(true);

        }
    }

    public void SelectPBtn3()
    {
        if (Btn[2].activeSelf == false)  
        {
            Controler.Player_num = 4; 
            PlayerSelect.SetActive(false);
            GameStart.SetActive(true);

        }
    }

    public void Start_button() //인스펙터 창에 StartBtn 클릭 시, onclick 함수 호출 및 자동 실행
    {
        SceneManager.LoadScene("SampleScene");
    }

   
}
