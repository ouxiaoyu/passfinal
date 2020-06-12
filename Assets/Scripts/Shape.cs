using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public static Boolean isUpdate = false;
    GameObject[] shapeArray = new GameObject[25];
    GameObject[] boundaryArray = new GameObject[10];
    public static int[] array;
    public static Color c = Color.red;
    public static String name = "Red";
    void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            shapeArray[i] = GameObject.Find("Cube (" + (i + 1) + ")");
        }
        for (int i = 0; i < 10; i++)
        {
            boundaryArray[i] = GameObject.Find("Boundary (" + (i + 1) + ")");
        }
    }

    void Update()
    {
        if (isUpdate)
        {
            gameObject.gameObject.tag = name;
            for (int i = 0; i < array.Length; i++)
            {
                shapeArray[i].GetComponent<Renderer>().material.color = c;

                if (array[i] == 1 || array[i] == 3)
                {
                    shapeArray[i].SetActive(false);
                }
                else
                {
                    shapeArray[i].SetActive(true);
                }

            }
            for (int i = 0; i < boundaryArray.Length; i++)
            {
                boundaryArray[i].GetComponent<Renderer>().material.color = c;
            }
            isUpdate = false;
        }
    }
}
