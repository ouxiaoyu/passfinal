using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class MovableCube : MonoBehaviour
{
    public AudioClip successClip;
    public AudioClip failClip;
    public AudioSource source;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
/*    public Animator animator21;
    public Animator animator22;
    public Animator animator23;
    public Animator animator24;*/
    public Animator skill1;
    public Animator skill2;
    public Animator skill3;
    public Animator skill4;
    public Animator perfect;
    public GameObject bg;

    GameObject[] scoreArray = new GameObject[60];
    GameObject[] bestArray = new GameObject[60];

    public GameObject whiteUI;
    public GameObject darkUI;

    Boolean turnRight = false;
    Boolean turnLeft = false;
    Vector3 rotateCenter;
    Vector3[] offset = new Vector3[4] { 
        new Vector3(-5, 5, 0), 
        new Vector3(5, 5, 0),
        new Vector3(-5, -5, 0), 
        new Vector3(5, -5, 0)};
    Vector3[] cubeOffset = new Vector3[8] { 
        new Vector3(-10, 10, 0), 
        new Vector3(0, 10, 0),
        new Vector3(10, 10, 0),
        new Vector3(-10, 0, 0),
        new Vector3(10, 0, 0),
        new Vector3(-10, -10, 0),
        new Vector3(0, -10, 0),
        new Vector3(10, -10, 0)};
    Boolean[] checkExist;
    int rotateAngle;
    int checkAngle = 0;
    public Button leftButton;
    public Button rightButton;
    public static int score = 0;
    public static Boolean isUpdate = true;
    //public static Boolean setMovableCube = false;
    Boolean isClearing = false;

    int[,] updateArray = new int[2,25];
    static int[,] previousArray = new int[2,25];
    static Boolean isFailed = false;
    public static int wallColor;
    static Boolean isProtected = false;
    static Boolean isColorful = false;


    public static Color[] colorArray = new Color[] {
                            new Color(239 / 255.0F, 27 / 255.0F, 57 / 255.0F, 255 / 255.0F),
                            new Color(255 / 255.0F, 162 / 255.0F, 32 / 255.0F, 255 / 255.0F),
                            new Color(74 / 255.0F, 211 / 255.0F, 0 / 255.0F, 255 / 255.0F),
                            //new Color(0  / 255.0F, 233 / 255.0F, 190 / 255.0F, 255 / 255.0F),
                            new Color(28 / 255.0F, 145 / 255.0F, 239 / 255.0F, 255 / 255.0F),
                            //new Color(169 / 255.0F, 78 / 255.0F, 255 / 255.0F, 255 / 255.0F),
                            //new Color(255 / 255.0F, 32 / 255.0F, 215 / 255.0F, 255 / 255.0F),
    };
    public static String[] colorString = new String[] {
                            "Red",
                            "Orange",
                            "Green",
                            //"LightBlue",
                            "Blue",
                            //"Purple",
                            //"Pink",
    };

    void Start()
    {
        for (int i = 0; i < 60; i++)
        {
            scoreArray[i] = GameObject.Find("Score (" + (i / 6).ToString() + (i % 6).ToString() + ")");
            bestArray[i] = GameObject.Find("Best (" + (i / 6).ToString() + (i % 6).ToString() + ")");
            bestArray[i].SetActive(false);
            scoreArray[i].SetActive(false);

        }
        scoreArray[0].SetActive(true);
        SetBest();
    }
    public void SetBest()
    {
#if UNITY_WEBGL
        GameObject.Find("BestLabel").SetActive(false);
#endif
#if UNITY_ANDROID
        WriteFile(Application.persistentDataPath, "Best.txt", LoadFile(Application.persistentDataPath, "Best.txt"));
        SetSprite(false, int.Parse(LoadFile(Application.persistentDataPath, "Best.txt")));

#endif
#if !UNITY_WEBGL
        File.WriteAllText("Assets//Resources//Best.txt", File.ReadAllText("Assets//Resources//Best.txt"), Encoding.Default);
        SetSprite(false, int.Parse(File.ReadAllText("Assets//Resources//Best.txt")));
#endif

    }

    void SetSprite(Boolean isScore, int num)
    {
        GameObject[] temp = bestArray;
        if (isScore)
        {
            temp = scoreArray;
        }
        for (int i = 0; i < 60; i++)
        {
            temp[i].SetActive(false);
        }
        if (num < 10 && num >= 0)
        {
            temp[6 * num].SetActive(true);
            temp[6 * num].GetComponent<Animator>().Play("Score", 0, 0f);
        }
        else if (num < 100)
        {
            temp[6 * (num/10) + 1].SetActive(true);
            temp[6 * (num%10) + 2].SetActive(true);
            if (temp[6 * (num / 10) + 1].GetComponent<Animator>() != null && temp[6 * (num % 10) + 2].GetComponent<Animator>() != null)
            {
                temp[6 * (num / 10) + 1].GetComponent<Animator>().Play("Score", 0, 0f);
                temp[6 * (num % 10) + 2].GetComponent<Animator>().Play("Score", 0, 0f);
            }
        }
        else if (num < 1000)
        {
            temp[6 * (num/100) + 3].SetActive(true);
            temp[6 * ((num-100*(num/100))/10) + 4].SetActive(true);
            temp[6 * (num % 10) + 5].SetActive(true);
            if (temp[6 * (num / 100) + 3].GetComponent<Animator>() != null && temp[6 * ((num - 100 * (num / 100)) / 10) + 4].GetComponent<Animator>() != null && temp[6 * (num % 10) + 5].GetComponent<Animator>() != null)
            {
                temp[6 * (num / 100) + 3].GetComponent<Animator>().Play("Score", 0, 0f);
                temp[6 * ((num - 100 * (num / 100)) / 10) + 4].GetComponent<Animator>().Play("Score", 0, 0f);
                temp[6 * (num % 10) + 5].GetComponent<Animator>().Play("Score", 0, 0f);
            }
        }
        else
        {
            temp[0].SetActive(true);
            temp[0].GetComponent<Animator>().Play("Score", 0, 0f);
        }
    }
    String LoadFile(string path, string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            return "0";
        }
        string info;
        info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return info;
    }

    void WriteFile(string path, string name,String info)
    {
        StreamWriter sw = new StreamWriter(path + "//" + name, false);
        sw.Write(info);
        sw.Close();
        sw.Dispose();
    }

    void Update()
    {
        //UpdateShape();
       
        if (Time.timeScale == 0)
        {
            if (turnRight)
            {
                gameObject.transform.RotateAround(rotateCenter, Vector3.forward, -(rotateAngle - checkAngle));
            }
            else if (turnLeft)
            {
                gameObject.transform.RotateAround(rotateCenter, Vector3.forward, rotateAngle - checkAngle);
            }
            turnRight = false;
            turnLeft = false;
            checkAngle = 0;
            leftButton.interactable = true;
            rightButton.interactable = true;
            if (isClearing)
            {
                
                AddScore();
                isUpdate = true;
                UpdateShape();
                perfect.Play("Pop", 0, 0f);
                SuccessSoundPlay();                
                isClearing = false;
            }
        }
    }

    void FixedUpdate()
    {

        UpdateShape();


        float interval = Time.fixedDeltaTime;
        //UnityEngine.Debug.Log(Time.fixedDeltaTime);
        if (turnRight)
        {
            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, -15 * 50 * interval);
            checkAngle += 15;
        }
        else if (turnLeft)
        {

            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, 15 * 50 * interval);
            checkAngle += 15;
        }

        if (checkAngle == rotateAngle)
        {
            turnRight = false;
            turnLeft = false;
            checkAngle = 0;
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        var name = collider.name;
        UnityEngine.Debug.Log("Name is " + name);

        String cubeColor = gameObject.tag;
        if (isColorful)
        {
            cubeColor = GameObject.Find("Shape").tag;
        }
        if ((isProtected && collider.tag == "Finish")|| (isProtected && collider.tag == "Clear" && cubeColor != GameObject.Find("Shape").tag))
        {
            isProtected = false;
            isColorful = false;
            isClearing = true;
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
            Time.timeScale = 0;
        }else if (isProtected && collider.tag == "Clear" && cubeColor == GameObject.Find("Shape").tag)
        {
            isProtected = false;
            isColorful = false;
            isClearing = true;
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
            Time.timeScale = 0;
        }
        else if ((collider.tag == "Finish" || (collider.tag == "Clear" && !isColorful && cubeColor != GameObject.Find("Shape").tag)))
        {
            isProtected = false;
            isColorful = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
            Time.timeScale = 0;
            int temp = score;
            score = 0;
            SetSprite(true, score);
            isFailed = true;

            animator1.SetBool("isAppear", true);
            animator2.SetBool("isAppear", true);
            animator3.SetBool("isAppear", true);
            animator4.SetBool("isAppear", true);
/*            animator21.SetBool("isAppear", true);
            animator22.SetBool("isAppear", true);
            animator23.SetBool("isAppear", true);
            animator24.SetBool("isAppear", true);*/
            bg.SetActive(true);
            FailSoundPlay();

#if UNITY_ANDROID
            if (temp > int.Parse(LoadFile(Application.persistentDataPath, "Best.txt"))) {
                WriteFile(Application.persistentDataPath, "Best.txt", temp.ToString());
                SetSprite(false, temp);
            }

#endif
#if !UNITY_WEBGL
            if (temp > int.Parse(File.ReadAllText("Assets//Resources//Best.txt")))
            {
                File.WriteAllText("Assets//Resources//Best.txt", temp.ToString(), Encoding.Default);
                SetSprite(false, temp);
            }
#endif
            //SetBest();
        }
        else if (collider.tag == "Clear" && cubeColor == GameObject.Find("Shape").tag)
        {
            isProtected = false;
            isColorful = false;
            isClearing = true;
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
            Time.timeScale = 0;
        }
        else if (collider.tag == "Red")
        {

            gameObject.GetComponent<Renderer>().material.color = colorArray[0];
            gameObject.tag = "Red";
        }
        else if (collider.tag == "Orange")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[1];
            gameObject.tag = "Orange";

        }
        else if (collider.tag == "Green")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[2];
            gameObject.tag = "Green";

        }
        else if (collider.tag == "Blue")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[3];
            gameObject.tag = "Blue";
        }
        else if (collider.tag == "SpeedUp")
        {
            Wall.isSpeedUp = true;
            skill1.Play("PopSkillAnimation", 0, 0f);
        }
        else if (collider.tag == "SpeedDown")
        {
            Wall.isSpeedDown = true;
            skill2.Play("PopSkillAnimation", 0, 0f);
        }
        else if (collider.tag == "Protection")
        {
            isProtected = true;
            skill3.Play("PopSkillAnimation", 0, 0f);
        }
        else if (collider.tag == "Colorful")
        {
            isColorful = true;
            skill4.Play("PopSkillAnimation", 0, 0f);
        }

    }

    Boolean[] GetNear()
    { 
        Boolean[] c = new Boolean[cubeOffset.Length];
        for (int i = 0; i < cubeOffset.Length; i++)
        {
            float posy = gameObject.transform.position.y + cubeOffset[i].y;
            float posx = gameObject.transform.position.x + cubeOffset[i].x;
            if( posx < -25 || posx > 25)
            {
                c[i] = false;
                continue;
            }
            if (posy < 10 || posy > 60)
            {
                c[i] = false;
                continue;
            }

            if ( updateArray[0 , Mathf.RoundToInt( ((55-posy)/10*5) + ((20+posx) / 10))] == 1 )
            {
                c[i] = true;
            }
            else
            {
                c[i] = false;
            }
        }

/*        if (gameObject.transform.position.y <= 14)
        {
            c[5] = true;
            c[6] = true;
            c[7] = true;
        }*/

        return c;
    }

    int GetCenter(int[] a)
    {
        if (a[0] != 0)
        {
            return 0;
        }
        if (a[1] != 0)
        {
            return 1;
        }
        if (a[2] != 0)
        {
            return 2;
        }
        if (a[3] != 0)
        {
            return 3;
        }
        return -1;
    }

    int[] GetAngle(Boolean isRight)
    {
        int[] a = new int[4];

        if (isRight)
        {
            if (checkExist[1] && !checkExist[0] && !checkExist[3])
            {
                a[0] = 180;
            }
            else if (checkExist[1] && checkExist[0] && !checkExist[3])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[4] && !checkExist[2] && !checkExist[1])
            {
                a[1] = 180;
            }
            else if (checkExist[4] && checkExist[2] && !checkExist[1])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[3] && !checkExist[5] && !checkExist[6])
            {
                a[2] = 180;
            }
            else if (checkExist[3] && checkExist[5] && !checkExist[6])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[6] && !checkExist[7] && !checkExist[4])
            {
                a[3] = 180;
            }
            else if (checkExist[6] && checkExist[7] && !checkExist[4])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else if (!isRight)
        {
            if (checkExist[3] && !checkExist[0] && !checkExist[1])
            {
                a[0] = 180;
            }
            else if (checkExist[3] && checkExist[0] && !checkExist[1])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[1] && !checkExist[2] && !checkExist[4])
            {
                a[1] = 180;
            }
            else if (checkExist[1] && checkExist[2] && !checkExist[4])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[6] && !checkExist[5] && !checkExist[3])
            {
                a[2] = 180;
            }
            else if (checkExist[6] && checkExist[5] && !checkExist[3])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[4] && !checkExist[7] && !checkExist[6])
            {
                a[3] = 180;
            }
            else if (checkExist[4] && checkExist[7] && !checkExist[6])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else
        {
            a[0] = 0;
            a[1] = 0;
            a[2] = 0;
            a[3] = 0;
        }
        return a;
    }

    public void RotateRight() 
    {        
        if (turnLeft)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(true))];
        rotateAngle = GetAngle(true)[GetCenter(GetAngle(true))];
//|| Math.Abs(GetNextXPoition(GetCenter(GetAngle(true)), rotateAngle, true))> 25 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) < 10 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) > 60
        if (GetCenter(GetAngle(true))==-1)
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        } 
        turnRight = true;
        
    }

    public void RotateLeft()
    {
        if (turnRight)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(false))];
        rotateAngle = GetAngle(false)[GetCenter(GetAngle(false))];
// || Math.Abs(GetNextXPoition(GetCenter(GetAngle(false)), rotateAngle, false)) > 25 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) < 10 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) > 60 
        if (GetCenter(GetAngle(false)) == -1)
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        }
        turnLeft = true;
    }

    float GetNextXPoition(int center,int angle,Boolean isRight)
    {
        if (isRight) 
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10;
                case 1: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x;
                case 2: if (angle == 180) return gameObject.transform.position.x - 10; else return gameObject.transform.position.x; 
                case 3: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; 
                default: return 0; 
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.x-10; else return gameObject.transform.position.x; 
                case 1: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; 
                case 2: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10; 
                case 3: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x;
                default: return 0; 
            }
        }
    }

    float GetNextYPoition(int center, int angle, Boolean isRight)
    {
        if (isRight)
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y ;
                case 1: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10;
                case 2: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; 
                case 3: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y;
                default: return 0;
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10; 
                case 1: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y; 
                case 2: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y; 
                case 3: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; 
                default: return 0; 
            }
        }
    }
    
    void AddScore()
    {
        
        score++;
        SetSprite(true, score);
    }

    void UpdateShape()
    {
        if (isUpdate)
        {
            Time.timeScale = 0;
            if (isFailed)
            {
                updateArray = (int[,])previousArray.Clone();
                isFailed = false;
                //UnityEngine.Debug.Log("isFail");
            }
            else
            {
                wallColor = UnityEngine.Random.Range(4, 8);
                updateArray = (int[,])CreateShape(wallColor).Clone();
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        UnityEngine.Debug.Log(wallColor + ":" + i + " " + j + ":" + updateArray[i, j]);
                    }
                }

            }
            //wall
            Wall.isReset = true;
            //fixedcube
            FixedCube.array = updateArray;
            FixedCube.isUpdate = true;
            //shape
            Shape.array = updateArray;
            Shape.wall_color = wallColor;
            Shape.isUpdate = true;
            //color
            ColorChanger.array = updateArray;
            ColorChanger.isUpdate = true;
            //movable cube
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            SetMovableCube(updateArray);
            gameObject.GetComponent<Renderer>().material.color = new Color(0 / 255.0F, 0 / 255.0F, 0 / 255.0F, 255 / 255.0F);

            isUpdate = false;
            previousArray = (int[,])updateArray.Clone();
        }
        if (gameObject.GetComponent<Rigidbody>() != null && (Wall.isReset || ColorChanger.isUpdate))
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";

        }
            if (gameObject.GetComponent<Rigidbody>() == null && !Wall.isReset && !ColorChanger.isUpdate)
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().angularDrag = 0;
            }
        
        Time.timeScale = 1;
    }
    

    public void SetMovableCube(int[,] array) {
        for (int i = 0; i < 25; i++)
        {
            if(array[0,i]==2)
            {
                gameObject.transform.localPosition = new Vector3(-20 + 10 * (i % 5), 55 - 10 * (i / 5), 115);
            }
        }
    }

    public void SuccessSoundPlay()
    {
        source.PlayOneShot(successClip);
    }
    public void FailSoundPlay()
    {
        source.PlayOneShot(failClip);
    }

        public int[,] CreateShape(int wall_color)
    {
        int probability = 20;
        int[,] shape = new int[2, 25];
        int[] grow_dir = { -1, 1, -5, 5 };
        int[] move_dir = { -1, 1, -5, 5, -4, 4, -6, 6 };
        int num_one = UnityEngine.Random.Range(0, 5) + 5;
        int index_two = 0;
        int[] index = new int[num_one];
        int center = UnityEngine.Random.Range(0, 25);
        index[0] = center;
        shape[0, center] = 1;
        int it = 1;
        while (it < index.Length + 1)
        {
            //            System.out.println("it"+it);
            int prev;
            prev = index[UnityEngine.Random.Range(0, it)];

            int direction = grow_dir[UnityEngine.Random.Range(0, 4)];

            int next = prev + direction;
            //            System.out.println("it "+it);
            //            System.out.println("prev "+prev);
            //            System.out.println("next "+next);
            Boolean isEdge = (direction == 1 || direction == -1) && ((prev + next) % 10 == 9);
            //            System.out.println("isEdge "+isEdge);
            if (next >= 0 && next < 25 && !isEdge && shape[0, next] == 0)
            {
                if (it == num_one)
                {
                    index_two = next;
                    shape[0, next] = 2;
                }
                else
                {
                    index[it] = next;
                    shape[0, next] = 1;
                }
                it++;
            }

        }
        //获取此情况下2能到达的index

        int[] visit = new int[25];
        for (int i = 0; i < 25; i++)
        {
            visit[i] = -1;
        }
        //        Boolean isThreeFound = false;
        //        Boolean isColorFound = false;

        Queue<int> nodeQueue = new Queue<int>();
        List<int> accessible = new List<int>();
        int node = index_two;
        nodeQueue.Enqueue(node);
        while (nodeQueue != null && nodeQueue.Count > 0)
        {

            node = nodeQueue.Dequeue();
            if (node != index_two)
            {
                accessible.Add(node);
            }

            List<int> children = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                int direction = move_dir[i];
                int next = node + direction;
                Boolean isNearOne = false;
                Boolean isEdge_1 = false;
                Boolean isEdge_4 = false;
                Boolean isEdge_6 = false;
                if (next >= 0 && next < 25)
                {
                    if (visit[next] == -1 && shape[0, next] != 1 && shape[0, next] != 2)
                    {
                        switch (direction)
                        {
                            case -1:
                                isEdge_1 = (node % 5 == 0);
                                if (isEdge_1)
                                {
                                    break;
                                }
                                if ((node - 6) >= 0 && (node + 5) < 25)
                                {
                                    isNearOne = (shape[0, node - 6] == 1 && shape[0, node - 5] == 1) || (shape[0, node + 4] == 1 && shape[0, node + 5] == 1);
                                }
                                else if ((node + 5) >= 25)
                                {
                                    isNearOne = shape[0, node - 6] == 1 && shape[0, node - 5] == 1;
                                }
                                else
                                {
                                    isNearOne = shape[0, node + 4] == 1 && shape[0, node + 5] == 1;
                                }
                                break;
                            case 1:
                                isEdge_1 = (node % 5 == 4);
                                if (isEdge_1)
                                {
                                    break;
                                }
                                if ((node - 5) >= 0 && (node + 6) < 25)
                                {
                                    isNearOne = (shape[0, node - 4] == 1 && shape[0, node - 5] == 1) || (shape[0, node + 6] == 1 && shape[0, node + 5] == 1);
                                }
                                else if ((node + 6) > 25)
                                {
                                    isNearOne = shape[0, node - 4] == 1 && shape[0, node - 5] == 1;
                                }
                                else
                                {
                                    isNearOne = shape[0, node + 6] == 1 && shape[0, node + 5] == 1;
                                }
                                break;
                            case -5:
                                if ((node % 5) == 0)
                                {
                                    isNearOne = shape[0, node - 4] == 1 && shape[0, node + 1] == 1;
                                }
                                else if ((node % 5) == 4)
                                {
                                    isNearOne = shape[0, node - 6] == 1 && shape[0, node - 1] == 1;
                                }
                                else
                                {
                                    isNearOne = (shape[0, node - 4] == 1 && shape[0, node + 1] == 1) || (shape[0, node - 6] == 1 && shape[0, node - 1] == 1);
                                }
                                break;
                            case 5:
                                if ((node % 5) == 0)
                                {
                                    isNearOne = shape[0, node + 6] == 1 && shape[0, node + 1] == 1;
                                }
                                else if ((node % 5) == 4)
                                {
                                    isNearOne = shape[0, node + 4] == 1 && shape[0, node - 1] == 1;
                                }
                                else
                                {
                                    isNearOne = (shape[0, node + 6] == 1 && shape[0, node + 1] == 1) || (shape[0, node + 4] == 1 && shape[0, node - 1] == 1);
                                }
                                break;
                            case -4:
                                isEdge_4 = (node + next) % 10 == 4;
                                if (isEdge_4)
                                {
                                    break;
                                }
                                isNearOne = (shape[0, node - 5] == 1) ^ (shape[0, node + 1] == 1);
                                break;
                            case 4:
                                isEdge_4 = (node + next) % 10 == 4;
                                if (isEdge_4)
                                {
                                    break;
                                }
                                isNearOne = (shape[0, node - 1] == 1) ^ (shape[0, node + 5] == 1);
                                break;
                            case -6:
                                isEdge_6 = (node + next) % 10 == 4;
                                if (isEdge_6)
                                {
                                    break;
                                }
                                isNearOne = (shape[0, node - 5] == 1) ^ (shape[0, node - 1] == 1);
                                break;
                            case 6:
                                isEdge_6 = (node + next) % 10 == 4;
                                if (isEdge_6)
                                {
                                    break;
                                }
                                isNearOne = (shape[0, node + 1] == 1) ^ (shape[0, node + 5] == 1);
                                break;
                            default:
                                break;

                        }
                        if (!isEdge_1 && !isEdge_4 && !isEdge_6 && isNearOne)
                        {
                            children.Add(next);
                            visit[next] = node;
                        }

                    }
                }
            }
            if (node != index_two && children.Count >= 2)
            {
                return CreateShape(wall_color);
            }
            if (children != null && children.Count > 0)
            {
                foreach (int child in children)
                {
                    nodeQueue.Enqueue(child);
                }
            }
        }

        int num_color = 0;
        List<int> start = new List<int>();
        for (int i = 0; i < 25; i++)
        {
            if (visit[i] == index_two)
            {
                start.Add(i);
                num_color++;
            }
        }
        if (accessible.Count < 3 + num_color)
        {
            return CreateShape(wall_color);
        }
        List<int> longPath = new List<int>();
        List<int> shortPath = new List<int>();
        int index_three_a = 0;
        int index_three_b = 0;
        int index_color;
        int num_color_type = 4;

        int[] tool = new int[] { 8, 9, 10, 11 };
        int index_tool;

        int another_color = UnityEngine.Random.Range(0, num_color_type) + 4;
        while (another_color == wall_color)
        {
            another_color = UnityEngine.Random.Range(0, num_color_type) + 4;
        }
        //wall_color
        //UnityEngine.Debug.Log("Num_Color: " + num_color);

        if (num_color == 1)
        {
            foreach (int i in accessible)
            {
                longPath.Add(i);
            }

            int removedNode1 = UnityEngine.Random.Range(0, accessible.Count);
            index_color = accessible[removedNode1];
            accessible.RemoveAt(removedNode1);

            shape[1, index_color] = wall_color;
            if (UnityEngine.Random.Range(0, probability)< score)//skilllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll
            {
                int tool_num = tool[UnityEngine.Random.Range(0, tool.Length)];
                index_tool = accessible[UnityEngine.Random.Range(0, accessible.Count)];
                shape[1, index_tool] = tool_num;
            }

            accessible.Add(index_color);

            int removedNode2 = UnityEngine.Random.Range(0, accessible.Count);
            index_three_a = accessible[removedNode2];
            accessible.RemoveAt(removedNode2);
            index_three_b = accessible[UnityEngine.Random.Range(0, accessible.Count)];
            shape[0, index_three_a] = 3;
            shape[0, index_three_b] = 3;
        }
        else
        {
            int temp = accessible[accessible.Count - 1];
            while (temp != index_two)
            {
                longPath.Add(temp);
                temp = visit[temp];
            }
            foreach (int i in accessible)
            {
                if (!longPath.Contains(i))
                {
                    shortPath.Add(i);
                }
            }
            

            int p = UnityEngine.Random.Range(0, 4);
            int index_wall_color = 0;
            int index_another_color = 0;
            switch (p)
            {
                case 0:
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_another_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    index_three_b = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 1:
                    index_another_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_wall_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    index_three_b = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 2:
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_b = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_another_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 3:
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count / 2)];
                    index_another_color = longPath[UnityEngine.Random.Range(longPath.Count / 2, longPath.Count)];
                    int removedNode3 = UnityEngine.Random.Range(0, longPath.Count);
                    index_three_a = longPath[removedNode3];
                    longPath.RemoveAt(removedNode3);
                    index_three_b = longPath[UnityEngine.Random.Range(0, (longPath.Count))];
                    break;
                default:
                    break;
            }
            UnityEngine.Debug.Log("color" + index_wall_color);
            UnityEngine.Debug.Log("color" + index_another_color);
            if (UnityEngine.Random.Range(0, probability) < score)//skilllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll
            {                
                index_tool = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                while (index_tool==index_another_color||index_tool==index_wall_color )
                {
                    if (longPath.Count==2 && p==3) {
                        index_tool = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    }
                    index_tool = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                }

                int tool_num = tool[UnityEngine.Random.Range(0, tool.Length)];
                shape[1, index_tool] = tool_num;
            }
            

            shape[0, index_three_a] = 3;
            shape[0, index_three_b] = 3;
            shape[1, index_wall_color] = wall_color;
            shape[1, index_another_color] = another_color;
        }
        return shape;

        /*return new int[,] {{0,0,0,0,0,
                                    0,0,0,0,0,
                                    0,0,2,0,0,
                                    0,0,1,0,0,
                                    3,1,1,1,3},
                                    {0,0,0,0,0,
                                    0,0,0,0,0,
                                    0,0,0,0,0,
                                    0,8,0,wall_color,0,
                                    0,0,0,0,0}
                                    };*/
    }
    public void darkChanger()
    {
        darkUI.SetActive(true);
        whiteUI.SetActive(false);
        SetBest();
        SetSprite(true, score);

    }
    public void whiteChanger()
    {
        darkUI.SetActive(false);
        whiteUI.SetActive(true);
        SetBest();
        SetSprite(true, score);
    }
}
