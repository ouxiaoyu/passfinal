using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : MonoBehaviour
{
    public static Boolean isReset = false;
    public static Boolean isSpeedUp = false;
    public static Boolean isSpeedDown = false;
    float fixedSpeed = 100;
    float fastSpeed = 150;
    float slowSpeed = 50;
    float speed = 100;
   
    void Start()
    {
        speed = fixedSpeed;
        Reset();
    }

    void Update()
    {
        if (isReset)
        {
            Reset();
            isReset = false;
        }

        //change speed
        if (isSpeedUp && !isSpeedDown)
        {
            speed = fastSpeed;
            Invoke("ResetSpeed", 0.5f);
        }
        else if (isSpeedDown && !isSpeedUp)
        {
            speed = slowSpeed;
            Invoke("ResetSpeed", 1f);
        }
        else if (isSpeedDown && isSpeedUp)
        {
            speed = fixedSpeed;
            isSpeedUp = false;
            isSpeedDown = false;
        }




        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(0, 0, 0), speed * (float)Math.Log10(MovableCube.score+10) * Time.deltaTime);   
    }
     void Reset()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 675);
    }

    public void ResetSpeed()
    {
        speed = fixedSpeed;
        isSpeedUp = false;
        isSpeedDown = false;
    }
}
