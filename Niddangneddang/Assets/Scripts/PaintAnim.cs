using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour  //적용하지 않은 스크립트
{
    private Animation anim;
    private Animator anima;
    bool awaken;
    Clicktile hit;
    GameObject paint;

    void Start()
    {

        anim = GetComponent<Animation>();
        anima = gameObject.GetComponent<Animator>();


}

    // Update is called once per frame
    void Update()
    {
        //anim.SetTrigger("Active");
    }
}
