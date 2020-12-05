using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAct : MonoBehaviour
{
    //float[] Max = {29, 63};
    float Max = 7f;
    //float[] Min = {26, 61};
    float Min = 5f;
    float currPosition;
    float direction = 3.0f;
    public int idx;

    private void OnEnable()
    {
        currPosition = transform.localPosition.y;
    }

    void Update()
    {
        currPosition += Time.deltaTime * direction;
        if (currPosition >= Max)
        {
            direction *= -1;
            currPosition = Max;
        }
        else if(currPosition <= Min)
        {
            direction *= -1;
            currPosition = Min;
        }

        transform.localPosition = new Vector3(12, currPosition, 0);
    }
}
