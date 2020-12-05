using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Power : MonoBehaviour
{
    public bool canUse;          //사용할수 있는지 확인
    public int power;            //1~5
    public bool used;           //true : 사용된 카드 . // false : 사용 안 한 카드
    public int PlayerIdx;       //Max(), SndMAx()에서 사용
    public int UIPlace;

    public Text PInfoText;

    private void OnEnable()
    {
        PInfoText.text = (power).ToString();  //파워 Text 1~5
    }

    private void Update()
    {
        if (used)  //카드 사용시, 회색으로 색 변경
        { GetComponent<MeshRenderer>().material.color = Color.grey; }
    }

    public void ResetState()        //파워 초기화
    {
        used = false;  //사용안함
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void InfoReset()
    {
        PInfoText.text = (power).ToString();
    }
}
