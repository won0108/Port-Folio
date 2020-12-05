using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicktile : MonoBehaviour
{
    private AudioSource musicPlayer1; // 음악플레이어
    public AudioClip sounds1; //타일 클릭 시, 효과음
    
    private void Start()
    {
        musicPlayer1 = GetComponent<AudioSource>(); //unity 툴에 업로드한 배경음악 가져오기
    }

    void Update()
    {
        Invoke("PlayS", 3);  //3초 대기 후 PlayS 함수 실행
    }


    void PlayS()
    {
        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 클릭시 플레이어 타일로 변환
        {
            //화면에 레이저를 쏜다
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;  //레이저와 부딪힌 객체

            if (Physics.Raycast(ray, out hit)) //레이저와 부딪힌 객체 있음
            {
                //아무도 클릭하지 않은 곳 플레이어가 클릭 시, 플레이어 점수 변동 및 타일 색 변경
                if (hit.transform.GetComponent<MeshRenderer>().material.color == Color.white)
                {
                    hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                    playSound1(sounds1, musicPlayer1);  //효과음 재생
                    Score.Player_s += 1;  //플레이어 점수 증가
                }
                //컴퓨터가 클릭한 곳 플레이어가 클릭할 시, 플레이어와 컴퓨터 점수 변동 및 타일 색 변경
                else if (hit.transform.GetComponent<MeshRenderer>().material.color == Color.blue)
                {
                    hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                    playSound1(sounds1, musicPlayer1); //효과음 재생
                    Score.Player_s += 1;  //플레이어 점수 증가
                    Score.Enemy_s -= 1;  //적 점수 감소
                }

            }

        }
    }


    public static void playSound1(AudioClip clip, AudioSource audioPlayer)
    {

        audioPlayer.Stop();  //사운드 잠시 멈춤
        audioPlayer.clip = clip;
        audioPlayer.loop = false; //반복재생 비허용
        audioPlayer.time = 0; //재생위치 초로 환산
        audioPlayer.Play();  //사운드 재생
    }
}
