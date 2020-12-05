using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public XmlNodeList MonNodes;
    public XmlNodeList GoRNodes;
    public XmlNodeList ItmNodes;
    public XmlNodeList ItRNodes;
    public XmlNodeList TpRNodes;
    public XmlNodeList BsRNodes;
    public XmlNodeList PlCNodes;

    private void Awake()
    {
        CreateXml();
    }

    void CreateXml()
    {
        //Load
        TextAsset textAsset = (TextAsset)Resources.Load("Dungeon_Data");  //"~" 안에 쓰여진 파일 로드
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);  //xmlDoc에 로드한 텍스트 저장

        //Read
        MonNodes = xmlDoc.SelectNodes("Basic/Monster-set/Monster");  //몬스터 하위 내용 전부 저장
        GoRNodes = xmlDoc.SelectNodes("Basic/GoldRoom-set/GoldRoom");  //골드 룸 하위 내용 전부 저장
        ItmNodes = xmlDoc.SelectNodes("Basic/Item-set/Item");  //아이탬 하위 내용 전부 저장
        ItRNodes = xmlDoc.SelectNodes("Basic/ItemRoom-set/ItemRoom");  //아이탬 룸 하위내용 전부 저장
        TpRNodes = xmlDoc.SelectNodes("Basic/TrapRoom-set/TrapRoom");  //트랩룸 하위내용 전부 저장
        BsRNodes = xmlDoc.SelectNodes("Basic/Boss-set/Boss");  //보스룸 하위내용 전부 저장
        PlCNodes = xmlDoc.SelectNodes("Basic/PlayerClass-set/Class");  //플레이어 직업 하위내용 전부 저장
    }


}
