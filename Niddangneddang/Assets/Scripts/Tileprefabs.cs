using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tileprefabs : MonoBehaviour
{
    public GameObject tile;
    public Transform tileParent;
    public List<GameObject> _tilelist = new List<GameObject>();   //plane을 이용하여 맵생성

    private float _xpos = 0f;  //타일이 생성될 기본 위치 설정
    private float _ypos = 0f;

    void Start() //한번만 호출
    {
        arr_tileInit();
    }

    void Update()
    {

    }
    void arr_tileInit()  //타일 생성
    {

        tile.transform.Translate(Vector3.zero); //생성할 프리팹 타일 위치 x,y,z 0으로 고정 (움직임 없음)

        for (int i = 0; i < 10; i++) //세로로 10개 복사
        {
            for (int j = 0; j < 10; j++) //가로로 10개 복사
            {
                GameObject _obj = Instantiate(tile) as GameObject; //타일 복제
                _obj.transform.parent = tileParent.transform; //복제한 타일위치 기준
                _obj.transform.localScale = new Vector3(1f, 1f, 1f);  //x,y,z 가 1 size 로 초기화
                _obj.transform.localRotation = Quaternion.Euler(90f, 90f, -90f); //타일이 카메라에 노출되도록 위치 조정
                _obj.transform.localPosition = new Vector3(_xpos, _ypos, 0f);  // x,y,z 0인 _obj gameobjet
                _tilelist.Add(_obj); //gameobject 생성
                _xpos += 10f; //첫번째 생성된 tile 위치 기준으로 x 좌표 10 이동

            }
            _xpos = 0f; //x 좌표 초기화
            _ypos += 10f;  //y 좌표 위로 10만큼 이동
        }

    }
}
