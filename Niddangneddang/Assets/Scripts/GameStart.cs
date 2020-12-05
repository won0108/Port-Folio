using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    
    public void ChangeFirstScene()  //SampleScene 호출
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    
    
}
