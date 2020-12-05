using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text End;  //게임결과 노출 변수
    
    private void Update()
    {
        OnApplicationQuit();
    }

    void OnApplicationQuit()  //게임결과
    {
        if (Score.Player_s > Score.Enemy_s)  //플레이어 승리
        {
            End.text = "Win";
        }
        else if (Score.Player_s == Score.Enemy_s)  //무승부
        {
            End.text = "Draw";

        }
        else  //플레이서 패자
        {
            End.text = "Lose";
        }
    }
}
