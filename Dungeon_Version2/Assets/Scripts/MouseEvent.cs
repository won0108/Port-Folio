using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEvent : MonoBehaviour
{
    GameObject[] power_objs;
    Transform[] item_objs;
    int playerIdx;

    Controler ctrl;
    GameManager gm;
    Button bt;

    private void Awake()
    {
        ctrl = GameObject.Find("GameControler").GetComponent<Controler>();  //하이러키에서 찾고 연결
        gm = GameObject.Find("GameControler").GetComponent<GameManager>();  //하이러키에서 찾고 연결
        bt = gameObject.GetComponent<Button>();   //하이러키에서 버튼과 연결
    }

    private void Update()
    {
        //맵이 확대되었을 때 선택 불가능 _ 색 변화는 되는 중..
        if (ctrl.ui.Isbig)
        {
            bt.interactable = false;
        }
        else
        {
            bt.interactable = true;
        }
    }


    //마우스를 올렸을 때 사용할 수 있는 카드 정보 나타냄
    private void OnMouseEnter()
    {
        if(!ctrl.ui.Isbig)
        {
            power_objs = ctrl.players[ctrl.turn].inven.power_objs;  //해당 순서 대상 파워카드 정보 가져옴
            item_objs = ctrl.players[ctrl.turn].inven.item_objs;    //해당 순서 대상 아이템 카드 가져옴

            Room crrRoom = ctrl.floor[ctrl.roomNum];  //현재 방 저장

            if (crrRoom.rTyp==Stage.roomType.BOSS)  //현재 방이 보스방인 경우
            {
                ByBossType(crrRoom.bRoom.boName);   //함수 호출
            }
            else
            {
                AtUsual();
            }
        }
    }

    private void OnMouseExit()  //마우스를 카드에서 제거했을 때 흰색으로 변동
    {
        if (!ctrl.ui.Isbig)
        {
            foreach (GameObject power_obj in power_objs)
            {
                if (power_obj.GetComponent<MeshRenderer>().material.color == Color.green)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.white;
                power_obj.GetComponent<Power>().InfoReset();
            }
            foreach (Transform item_obj in item_objs)
            {
                if (item_obj.GetComponent<MeshRenderer>().material.color == Color.green)
                    item_obj.GetComponent<MeshRenderer>().material.color = Color.white;
                item_obj.GetComponent<Item>().InfoReset();
            }
        }
    }


    //마우스 클릭시, 선택한 카드 필드로 이동
    private void InfoToField()
    {
        if (CompareTag("POWER")  //파워 카드일 때
            && GetComponent<Power>().PlayerIdx == ctrl.turn
            && GetComponent<Power>().canUse)
        {
            Power p = GetComponent<Power>();                    //선택된 오브젝트의 파워 데이터를 받아온다.
            p.used = true;

            gm.field[ctrl.turn].fPower.power = p.power;             //필드에 파워 정보를 넘겨준다.

            StartCoroutine(MoveFromTo(this.gameObject, gm.field[ctrl.turn].fPwrObj.gameObject));  //코루틴 시작

            ctrl.players[ctrl.turn].moreturn = false;
            ctrl.usingPwrOrder.Add(ctrl.turn);
        }
        else if (CompareTag("ITEM")  //아이템 카드일 때
            && GetComponent<Item>().PlayerIdx == ctrl.turn
            && GetComponent<Item>().canUse)
        {
            GetComponent<Item>().used = true;

            StartCoroutine(MoveFromTo(this.gameObject, gm.field[ctrl.turn].fItmObj.gameObject));  //코루틴 시작
        }
    }

    void SelectEnd()
    {
        if (gm.field[ctrl.turn].fItmObj.gameObject.activeSelf)
            ctrl.state = Controler.State.ItemUsed;
        else if (gm.field[ctrl.turn].fPwrObj.gameObject.activeSelf)
        {
            ctrl.state = Controler.State.NextTurn;
        }
    }

    IEnumerator MoveFromTo(GameObject fromobj, GameObject toobj)  //플레이어 카드가 필드로 움직이는 효과
    {
        fromobj.GetComponent<MeshRenderer>().material.color = Color.grey;

        toobj.transform.position = fromobj.transform.position;  //사용할 카드의 위치 확인
        toobj.SetActive(true);

        if (fromobj.CompareTag("ITEM"))  //아이템 카드 정보 가져오기
        {
            Item fitem = toobj.GetComponent<Item>();
            fitem.itName = fromobj.GetComponent<Item>().itName;
            fitem.fxType = fromobj.GetComponent<Item>().fxType;
            fitem.fxValue = fromobj.GetComponent<Item>().fxValue;

            string str = fitem.itName.ToString();
            fitem.ItInfoText.text = str;
        }

        float dis = 1f;
        //float dis = (toobj.transform.parent.position - fromobj.transform.position).sqrMagnitude;
        while (dis > 0)
        {
            Vector3 newpo = Vector3.MoveTowards(toobj.transform.position, toobj.transform.parent.position, 2f);  //필드로 카드 이동
            toobj.transform.position = newpo;

            dis -= Time.deltaTime;

            for (int i = 0; i < 5; i++) //사용한 카드 비활성
            {
                ctrl.players[ctrl.turn].inven.powers[i].canUse = false;
            }
            for (int i = 0; i < 4; i++)
            {
                ctrl.players[ctrl.turn].inven.item[i].canUse = false;
            }
            yield return null;
        }
        SelectEnd();
    }


    //방 종류별 OnMouseEnter 함수
    void AtOberon()  //오베론이 보스일 때
    {
        if (gameObject.CompareTag("POWER")
           && gameObject.GetComponent<Power>().PlayerIdx == ctrl.turn)
        {
            foreach (GameObject power_obj in power_objs)  //사용 가능한 파워카드 초록색으로 변동
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                {
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
                    if (_power.power == 5)  //파워카드가 5일 때 파워가 1로 변경
                        _power.PInfoText.text = 1.ToString();
                }
            }
            foreach (Transform item_obj in item_objs)  //아이템 카드 중 사용 가능한 카드 초록색으로 변동
            {
                Item _item = item_obj.GetComponent<Item>();
                if (_item.canUse && _item.itName == Item.ItName.SWORD)
                {
                    item_obj.GetComponent<MeshRenderer>().material.color = Color.green;
                    _item.ItInfoText.text = 1.ToString();  //아이템카드 검카드가 1로 변경
                }
            }
        }
        else if (gameObject.CompareTag("ITEM")
            && gameObject.GetComponent<Item>().PlayerIdx == ctrl.turn)
        {
            foreach (Transform item_obj in item_objs) //아이템 카드 중 사용 가능한 카드 초록색으로 변동
            {
                Item _item = item_obj.GetComponent<Item>();
                if (_item.canUse)
                {
                    item_obj.GetComponent<MeshRenderer>().material.color = Color.green;
                    if (_item.itName == Item.ItName.SWORD)
                        _item.ItInfoText.text = 1.ToString();
                }
            }
            foreach (GameObject power_obj in power_objs)  //사용할 수 있는 모든 파워 카드 초록색으로 변동
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
    }

    void AtDragonKing()
    {
        //즉시 탈출 조건
        HasItem(Item.ItName.KEY);

        if (gameObject.CompareTag("POWER")
            && gameObject.GetComponent<Power>().PlayerIdx == ctrl.turn)  //사용가능한 파워 카드 모두 초록색으로 변경
        {
            foreach (GameObject power_obj in power_objs)
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            
        }//"POWER"
        else if (gameObject.CompareTag("ITEM")
             && gameObject.GetComponent<Item>().PlayerIdx == ctrl.turn)  //사용 가능한 아이템 가능 모두 초록색으로 변경
        {
            foreach (GameObject power_obj in power_objs)
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            foreach (Transform item_obj in item_objs)
            {
                Item _item = item_obj.GetComponent<Item>();
                if (_item.canUse && _item.itName!=Item.ItName.KEY)  //열쇠 카드 제외 사용 가능한 카드 모두 초록색 변경
                    item_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }//"ITEM"
    }

    void AtUsual()
    {
        if (gameObject.CompareTag("POWER")
            && gameObject.GetComponent<Power>().PlayerIdx == ctrl.turn)  //해당 순서 대상 파워카드에 마우스 올렸을 때 사용가능하다면 색이 초록색으로 변동
        {
            foreach (GameObject power_obj in power_objs)
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }//"POWER"
        else if (gameObject.CompareTag("ITEM")   //해당 순서 대상 아이템카드에 마우스를 올렸을 때 사용할 수 있다면 초록색으로 변경
             && gameObject.GetComponent<Item>().PlayerIdx == ctrl.turn)
        {
            foreach (GameObject power_obj in power_objs)
            {
                Power _power = power_obj.GetComponent<Power>();
                if (_power.canUse)
                    power_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            foreach (Transform item_obj in item_objs)
            {
                Item _item = item_obj.GetComponent<Item>();
                if (_item.canUse)
                    item_obj.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }//"ITEM"
    }

    void HasItem(Item.ItName check)
    {
        foreach (Transform item_obj in item_objs)  //키 카드일 때 빨간색으로 변동
        {
            Item _item = item_obj.GetComponent<Item>();
            if (_item.itName == check)
            {
                item_obj.GetComponent<MeshRenderer>().material.color = Color.red;
                _item.ItInfoText.text = "EXIT";  //카드 Text 변경
            }
        }
    }

    void ByBossType(Stage.BoName boname)  //특정 보스일 때 함수 호출
    {
        switch (boname)
        {
            case Stage.BoName.OBERON:
                AtOberon();

                break;
            case Stage.BoName.DRAGONKING:
                AtDragonKing();

                break;
            default:
                AtUsual();
                break;
        }
    }
}