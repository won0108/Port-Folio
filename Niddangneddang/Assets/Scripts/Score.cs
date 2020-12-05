using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int Player_s;  //플레이어 점수
    public static int Enemy_s;  //적 점수
    public Text Player_t;  //플레어어 점수 text
    public Text Enemy_t;  //적 점수 text


    void Start()
    {
        Player_s = 0;  //점수 초기화
        Enemy_s = 0;
    }

    void Update()  //획득한 점수 실시간으로 노출
    {
        Player_t.text = "P" + "\n"+ "L" + "\n" + "A" + "\n"+ "Y" + "\n"+ "E" +"\n"+"R" +"\n"+ Player_s + "\n";
        //Enemy_t.text = "컴퓨터 : " + Enemy_s + "칸";
        Enemy_t.text = "C" +"\n\n" + "O" + "\n\n" + "M" + "\n\n" +Enemy_s + "\n";

    }
}
