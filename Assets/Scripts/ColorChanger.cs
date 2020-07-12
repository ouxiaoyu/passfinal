using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static Boolean isUpdate = false;
    GameObject[,] colorArray = new GameObject[4, 25];
    GameObject[,] itemArray = new GameObject[4, 25];


    public static int[,] array;

    void Start()
    {
        for (int j = 0; j < 4; j++)
        {
            String name = null;
            String item = null;
            switch (j)
            {
                case 0: name = "Red"; item = "SpeedUp"; break;
                case 1: name = "Orange"; item = "SpeedDown"; break;
                case 2: name = "Green"; item = "Protection"; break;
                case 3: name = "Blue"; item = "Colorful"; break;
            }
            for (int i = 0; i < 25; i++)
            {
                colorArray[j,i] = GameObject.Find(name+" (" + (i + 1) + ")");
                colorArray[j,i].SetActive(false);
                if (item != null)
                {
                    itemArray[j, i] = GameObject.Find(item + " (" + (i + 1) + ")");
                    itemArray[j, i].SetActive(false);
                }
                
            }
        }
        
    }

    void Update()
    {
        if (isUpdate)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    colorArray[j, i].SetActive(false);
                }
            }
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    itemArray[j, i].SetActive(false);
                }
            }
            for (int i = 0; i < 25; i++)
            {
                switch (array[1,i])
                {
                    case 4:
                        colorArray[0,i].SetActive(true);
                        break;
                    case 5:
                        colorArray[1,i].SetActive(true);
                        break;
                    case 6:
                        colorArray[2,i].SetActive(true);
                        break;
                    case 7:
                        colorArray[3,i].SetActive(true);
                        break;
                    case 8:
                        itemArray[0,i].SetActive(true);
                        break;
                    case 9:
                        itemArray[1,i].SetActive(true);
                        break;
                    case 10:
                        itemArray[2,i].SetActive(true);
                        break;
                    case 11:
                        itemArray[3, i].SetActive(true);
                        break;
                    default: break;
                }
            }
            isUpdate = false;
            //MovableCube.setMovableCube = true;
        }
    }


}
